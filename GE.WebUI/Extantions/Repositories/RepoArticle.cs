using GE.WebUI.Models;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using GE.WebCoreExtantions;

namespace GE.WebUI.Extantions.Repositories
{
    public static partial class RepositoryExtantions
    {
        public static VMFGBlock ForGamersBlock(this GE.WebCoreExtantions.Repositories.RepoArticle repo)
        {
            var viewModel = new VMFGBlock();
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
                    ArticleTypes = result.Where(t => t.GameId == game.Id).Select(t => new VMFGBArticleType
                    {
                        Name = t.ArticleTypeName,
                        Description = t.ArticleTypeDesc
                    }).ToArray()
                };
            }

            return viewModel;
        }

        public static VMPreviewArticle[] PreviewMaterials(this GE.WebCoreExtantions.Repositories.RepoArticle repo, int gameId, string articleType, int lettersCount=200)
        {
            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                return conn.Query<VMPreviewArticle>(Resources.Sql_Articles.PreviewMaterials, new { GAME_ID = gameId, ARTICLE_TYPE_NAME = articleType, LETTERS_COUNT=lettersCount }).ToArray();
            }
        }

        public static Article[] Last(this GE.WebCoreExtantions.Repositories.RepoArticle repo, int amount)
        {
            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                var results = conn.Query<Article>(Resources.Sql_Articles.LastArticles, new { AMOUNT = amount }).ToArray();
                return results;
            }
        }

        public static VMDetailArticle GetByTitleUrl(this GE.WebCoreExtantions.Repositories.RepoArticle repo, string titleUrl)
        {
            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                var query = @"SELECT*FROM D_ARTICLE AS da
JOIN DV_MATERIAL AS dm ON dm.ID = da.ID AND dm.ModelCoreType = da.ModelCoreType
WHERE dm.TitleUrl=@TITLE_URL";

                return conn.Query<VMDetailArticle>(query, new { TITLE_URL = titleUrl }).FirstOrDefault();
            }
        }
    }
}