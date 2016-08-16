using GE.WebUI.Models;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using static SX.WebCore.Enums;
using SX.WebCore.ViewModels;

namespace GE.WebUI.Extantions.Repositories
{
    public static partial class RepositoryExtantions
    {
        private static readonly string _queryPreviewMaterials = @"get_preview_materials @lettersCount, @gameTitle, @categoryId";

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
       dmc.Title                 AS CategoryTitle,
       dbo.get_comments_count(dm.Id, dm.ModelCoreType) as CommentsCount
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
       dm.Id,
       dm.ModelCoreType,
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
                var categories = result.Where(x => x.GameId == game.Id).Select(x => new { Id = x.CategoryId, Title = x.CategoryTitle }).Distinct().OrderBy(x=>x.Title);
                viewModel.Games[i] = new VMFGBGame
                {
                    Id = game.Id,
                    Description = game.Description,
                    FrontPictureId = game.FrontPictureId,
                    Title = game.Title,
                    TitleUrl=game.TitleUrl,
                    MaterialCategories= categories.Select(x=>new SxVMMaterialCategory { Id=x.Id, Title=x.Title}).ToArray()
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

        public static VMMaterial[] PreviewMaterials(this WebCoreExtantions.Repositories.RepoArticle repo, string gameTitle, string categoryId, int lettersCount=200)
        {
            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                var data=conn.Query<VMMaterial>(_queryPreviewMaterials, new { gameTitle = gameTitle, categoryId = categoryId, lettersCount=lettersCount }).ToArray();
                return data;
            }
        }

        public static VMMaterial[] Last(this WebCoreExtantions.Repositories.RepoArticle repo, int amount)
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
                var data = conn.Query<VMMaterial>(query, new { amount = amount });
                return data.ToArray();
            }
        }

        public static VMMaterial[] GetPopular(this WebCoreExtantions.Repositories.RepoArticle repo, ModelCoreType mct, int mid, int amount)
        {
            using (var connection = new SqlConnection(repo.ConnectionString))
            {
                var data = connection.Query<VMMaterial>("dbo.get_popular_materials @mid, @mct, @amount", new { mct = mct, mid = mid, amount = amount }).ToArray();
                return data;
            }
        }
    }
}