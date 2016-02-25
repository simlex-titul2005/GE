using GE.WebCoreExtantions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Dapper;

namespace GE.WebAdmin.Extantions.Repositories
{
    public static partial class RepositoryExtantions
    {
        public static IEnumerable<ArticleType> GetArticleTypesByGameId(this GE.WebCoreExtantions.Repositories.RepoArticleType repo, int gameId)
        {
            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                var result = conn.Query<ArticleType>(Resources.Sql_ArticleTypes.GetArticleTypesByGameId, new { GAME_ID = gameId });
                return result;
            }
        }
    }
}