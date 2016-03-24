using GE.WebUI.Models;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using GE.WebCoreExtantions;

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

        public static Article[] Last(this WebCoreExtantions.Repositories.RepoArticle repo, int amount)
        {
            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                var results = conn.Query<Article>(Resources.Sql_Articles.LastArticles, new { AMOUNT = amount }).ToArray();
                return results;
            }
        }

        public static VMDetailArticle GetByTitleUrl(this WebCoreExtantions.Repositories.RepoArticle repo, string titleUrl)
        {
            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                var query = @"SELECT da.*,
       dm.*,
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
       )                 AS LikeDownCount
FROM   D_ARTICLE         AS da
       JOIN DV_MATERIAL  AS dm
            ON  dm.Id = da.Id
            AND dm.ModelCoreType = da.ModelCoreType
WHERE  dm.TitleUrl = @TITLE_URL";

                return conn.Query<VMDetailArticle>(query, new { TITLE_URL = titleUrl }).FirstOrDefault();
            }
        }
    }
}