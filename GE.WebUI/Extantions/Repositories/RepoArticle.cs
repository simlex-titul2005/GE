using GE.WebUI.Models;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using GE.WebUI.Models.Abstract;
using static SX.WebCore.Enums;
using System;
using SX.WebCore;

namespace GE.WebUI.Extantions.Repositories
{
    public static partial class RepositoryExtantions
    {
        private static readonly string _queryPreviewMaterials = @"SELECT TOP(8) da.Id,
       dm.Title,
       dm.TitleUrl,
       dm.DateCreate,
       dm.DateOfPublication,
       dm.ViewsCount,
       dm.CommentsCount,
       CASE 
            WHEN dm.Foreword IS NOT NULL THEN dm.Foreword
            ELSE SUBSTRING(dbo.FUNC_STRIP_HTML(dm.Html), 0, @lettersCount) +
                 '...'
       END                    AS Foreword,
       anu.NikName            AS UserName,
       dg.Title               AS GameTitle
FROM   D_ARTICLE              AS da
       JOIN DV_MATERIAL       AS dm
            ON  dm.Id = da.Id
            AND dm.ModelCoreType = da.ModelCoreType
            AND (dm.Show = 1 AND dm.DateOfPublication <= GETDATE())
       LEFT JOIN AspNetUsers  AS anu
            ON  anu.Id = dm.UserId
       LEFT JOIN D_GAME       AS dg
            ON  dg.Id = da.GameId
WHERE  (@gameTitle IS NULL)
       OR  (
               @gameTitle IS NOT NULL
               AND @categoryId IS NULL
               AND dg.TitleUrl = @gameTitle
           )
       OR  (
               @gameTitle IS NOT NULL
               AND @categoryId IS NOT NULL
               AND dg.TitleUrl = @gameTitle
               AND dm.CategoryId = @categoryId
           )
ORDER BY
       dm.DateCreate DESC";

        public static VMFGBlock ForGamersBlock(this WebCoreExtantions.Repositories.RepoArticle repo, string gameTitle=null)
        {
            var viewModel = new VMFGBlock() { SelectedGameTitle= gameTitle };
            dynamic[] result = null;
            var query = @"SELECT da.GameId,
       dg.FrontPictureId,
       dg.Title,
       dg.TitleUrl,
       dg.[Description],
       dm.CategoryId,
       dmc.Title                 AS CategoryTitle
FROM   DV_MATERIAL               AS dm
       JOIN D_ARTICLE            AS da
            ON  da.Id = dm.Id
            AND da.ModelCoreType = dm.ModelCoreType
       JOIN D_GAME               AS dg
            ON  dg.Id = da.GameId
            AND dg.Show = 1
            AND dg.FrontPictureId IS NOT NULL
       JOIN D_MATERIAL_CATEGORY  AS dmc
            ON  dmc.Id = dm.CategoryId
WHERE  dm.Show = 1
       AND dm.DateOfPublication <= GETDATE()
GROUP BY
       da.GameId,
       dg.FrontPictureId,
       dg.Title,
       dg.TitleUrl,
       dg.[Description],
       dm.CategoryId,
       dmc.Title";
            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                result = conn.Query<dynamic>(query).ToArray();
            }

            var games = result
                .Select(x => new
                {
                    Id = x.GameId,
                    Description = x.Description,
                    FrontPictureId = x.FrontPictureId,
                    Title = x.Title,
                    TitleUrl=x.TitleUrl
                }).Distinct().ToArray();

            viewModel.Games = new VMFGBGame[games.Length];
            for (int i = 0; i < games.Length; i++)
            {
                var game = games[i];
                viewModel.Games[i] = new VMFGBGame
                {
                    Id = game.Id,
                    Description = game.Description,
                    FrontPictureId = game.FrontPictureId,
                    Title = game.Title,
                    TitleUrl=game.TitleUrl,
                    MaterialCategories=result.Where(t=>t.GameId == game.Id)
                    .Select(t=>new VMMaterialCategory { Id=t.CategoryId, Title=t.CategoryTitle }).ToArray()
                };
            }

            query = _queryPreviewMaterials;
            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                var articles = conn.Query<VMPreviewArticle>(query, new { gameTitle = gameTitle, categoryId = (string)null, lettersCount = 200 }).ToArray();
                viewModel.Articles = articles;
            }

            return viewModel;
        }

        public static VMPreviewArticle[] PreviewMaterials(this WebCoreExtantions.Repositories.RepoArticle repo, string gameTitle, string categoryId, int lettersCount=200)
        {
            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                var data=conn.Query<VMPreviewArticle>(_queryPreviewMaterials, new { gameTitle = gameTitle, categoryId = categoryId, lettersCount=lettersCount }).ToArray();
                return data;
            }
        }

        public static VMDetailMaterial[] Last(this WebCoreExtantions.Repositories.RepoArticle repo, int amount)
        {
            var query = @"SELECT TOP(@amount) *
FROM   (
           SELECT TOP(@amount) dm.DateCreate,
                  dm.Title,
                  dm.TitleUrl,
                  dm.ModelCoreType
           FROM   DV_MATERIAL     AS dm
                  JOIN D_ARTICLE  AS da
                       ON  da.ModelCoreType = dm.ModelCoreType
                       AND da.Id = dm.Id
           WHERE  dm.Show = 1
                  AND dm.DateOfPublication <= GETDATE()
           ORDER BY
                  dm.DateCreate DESC
           UNION
           SELECT TOP(@amount) dm.DateCreate,
                  dm.Title,
                  dm.TitleUrl,
                  dm.ModelCoreType
           FROM   DV_MATERIAL  AS dm
                  JOIN D_NEWS  AS dn
                       ON  dn.ModelCoreType = dm.ModelCoreType
                       AND dn.Id = dm.Id
           WHERE  dm.Show = 1
                  AND dm.DateOfPublication <= GETDATE()
           ORDER BY
                  dm.DateCreate DESC
       ) x
ORDER BY
       x.DateCreate DESC";
            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                var results = conn.Query<VMDetailMaterial>(query, new { amount = amount }).ToArray();
                return results;
            }
        }

        public static VMDetailArticle GetByTitleUrl(this WebCoreExtantions.Repositories.RepoArticle repo, int year, string month, string day, string titleUrl)
        {
            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                var data=conn.Query<VMDetailArticle>("get_material_by_url @year, @month, @day, @title_url, @mct", new { year=year, month=month, day=day, title_url = titleUrl, mct=ModelCoreType.Article }).SingleOrDefault();
                if(data!=null)
                    data.Videos = conn.Query<SxVideo>("get_material_videos @mid", new { mid = data.Id }).ToArray();
                return data;
            }
        }

        public static VMLastNews[] GetByDateMaterial(this WebCoreExtantions.Repositories.RepoArticle repo, int mid, ModelCoreType mct, bool dir, int amount)
        {
            using (var connection = new SqlConnection(repo.ConnectionString))
            {
                var data = connection.Query<VMLastNews, VMUser, VMLastNews>("select* from get_other_materials(@mid, @mct, @dir, @amount) ORDER BY DateCreate", (m, u) => {
                    m.Author = u;
                    return m;
                }, new { mid = mid, mct = mct, dir= dir, amount= amount }, splitOn: "UserId").ToArray();
                return data;
            }
        }

        public static VMLastNews[] GetPopular(this WebCoreExtantions.Repositories.RepoArticle repo, ModelCoreType mct, int mid, int amount)
        {
            var queryForByDateMaterials = @"SELECT TOP(@amount)
       dm.DateCreate,
       dm.DateOfPublication,
       dm.Title,
       dm.TitleUrl,
       dm.ModelCoreType,
       COUNT(dc.Id)            AS CommentsCount,
       COUNT(dl.Id)            AS LikesCount,
       SUM(dm.ViewsCount)         ViewsCount
FROM   DV_MATERIAL             AS dm
       LEFT JOIN D_COMMENT     AS dc
            ON  dc.ModelCoreType = dm.ModelCoreType
            AND dc.MaterialId = dm.Id
       LEFT JOIN D_USER_CLICK  AS duc
            ON  duc.MaterialId = dm.Id
            AND duc.ModelCoreType = dm.ModelCoreType
       LEFT JOIN D_LIKE        AS dl
            ON  dl.UserClickId = duc.Id
WHERE  dm.ModelCoreType = @mct
       AND dm.Show = 1
       AND dm.DateOfPublication <= GETDATE()
       AND dm.Id NOT IN (@mid)
GROUP BY
       dm.DateCreate,
       dm.DateOfPublication,
       dm.Title,
       dm.TitleUrl,
       dm.ModelCoreType
HAVING COUNT(dc.Id) > 0 OR COUNT(dl.Id) > 0 OR COUNT(dm.ViewsCount) > 0
ORDER BY
       CommentsCount DESC,
       LikesCount DESC,
       ViewsCount DESC";

            using (var connection = new SqlConnection(repo.ConnectionString))
            {
                var data = connection.Query<VMLastNews>(queryForByDateMaterials, new { mct = mct, mid = mid, amount = amount }).ToArray();
                return data;
            }
        }
    }
}