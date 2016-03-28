using GE.WebCoreExtantions;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace GE.WebAdmin.Extantions.Repositories
{
    public static partial class RepositoryExtantions
    {
        public static Article GetByTitleUrl(this WebCoreExtantions.Repositories.RepoArticle repo, string titleUrl)
        {
            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                var query = @"SELECT*FROM D_ARTICLE AS da
JOIN DV_MATERIAL AS dm ON dm.ID = da.ID AND dm.ModelCoreType = da.ModelCoreType
WHERE dm.TitleUrl=@TITLE_URL";

                return conn.Query<Article>(query, new { TITLE_URL = titleUrl }).FirstOrDefault();
            }
        }
    }
}