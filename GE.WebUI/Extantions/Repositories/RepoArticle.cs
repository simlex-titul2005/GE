using GE.WebUI.Models;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using GE.WebCoreExtantions;
using GE.WebUI.Models.Abstract;

namespace GE.WebUI.Extantions.Repositories
{
    public static partial class RepositoryExtantions
    {
        public static VMFGBlock ForGamersBlock(this WebCoreExtantions.Repositories.RepoArticle repo, string gameTitle=null)
        {
            var viewModel = new VMFGBlock() { SelectedGameTitle= gameTitle };
            dynamic[] result = null;
            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                result = conn.Query<dynamic>(Resources.Sql_Articles.FGBGameMenu).ToArray();
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
                    ArticleTypes = result.Where(t => t.GameId == game.Id).Select(t => new VMFGBArticleType
                    {
                        Name = t.ArticleTypeName,
                        Description = t.ArticleTypeDesc
                    }).ToArray()
                };
            }

            var query = Resources.Sql_Articles.PreviewMaterials;
            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                var articles = conn.Query<VMPreviewArticle>(query, new { GAME_TITLE = gameTitle, ARTICLE_TYPE_NAME = (string)null, LETTERS_COUNT = 200 }).ToArray();
                viewModel.Articles = articles;
            }

            return viewModel;
        }

        public static VMPreviewArticle[] PreviewMaterials(this WebCoreExtantions.Repositories.RepoArticle repo, string gameTitle, string articleType, int lettersCount=200)
        {
            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                var data=conn.Query<VMPreviewArticle>(Resources.Sql_Articles.PreviewMaterials, new { GAME_TITLE = gameTitle, ARTICLE_TYPE_NAME = articleType, LETTERS_COUNT=lettersCount }).ToArray();
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

        public static VMDetailArticle GetByTitleUrl(this WebCoreExtantions.Repositories.RepoArticle repo, string titleUrl)
        {
            var query = @"SELECT da.*,
       dm.*,
       dg.TitleUrl       AS GameTitleUrl,CASE 
            WHEN dm.Foreword IS NOT NULL THEN dm.Foreword
            ELSE SUBSTRING(dbo.FUNC_STRIP_HTML(dm.Html), 0, 200) +
                 '...'
       END                    AS Foreword,
       (
           SELECT ISNULL(SUM(1), 0)
           FROM   D_LIKE AS dl
           WHERE  dl.Direction = 1
                  AND dl.MaterialId = dm.Id
                  AND dl.ModelCoreType = dm.ModelCoreType
       )                 AS LikeUpCount,
       (
           SELECT ISNULL(SUM(1), 0)
           FROM   D_LIKE AS dl
           WHERE  dl.Direction = 0
                  AND dl.MaterialId = dm.Id
                  AND dl.ModelCoreType = dm.ModelCoreType
       )                 AS LikeDownCount,
       anu.NikName AS UserNikName
FROM   D_ARTICLE         AS da
       JOIN DV_MATERIAL  AS dm
            ON  dm.Id = da.Id
            AND dm.ModelCoreType = da.ModelCoreType
       LEFT JOIN D_GAME  AS dg
            ON  dg.Id = da.GameId
       JOIN  AspNetUsers AS anu ON anu.Id = dm.UserId
WHERE  dm.TitleUrl = @TITLE_URL";

            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                return conn.Query<VMDetailArticle>(query, new { TITLE_URL = titleUrl }).FirstOrDefault();
            }
        }
    }
}