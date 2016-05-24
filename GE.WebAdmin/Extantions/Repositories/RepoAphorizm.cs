using GE.WebCoreExtantions;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using GE.WebCoreExtantions.Repositories;

namespace GE.WebAdmin.Extantions.Repositories
{
    public static partial class RepositoryExtantions
    {
        public static Aphorism GetByTitleUrl(this RepoAphorism repo, string titleUrl)
        {
            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                var query = @"SELECT*FROM D_APHORISM AS da
JOIN DV_MATERIAL AS dm ON dm.ID = da.ID AND dm.ModelCoreType = da.ModelCoreType
WHERE dm.TitleUrl=@TITLE_URL";

                return conn.Query<Aphorism>(query, new { TITLE_URL = titleUrl }).FirstOrDefault();
            }
        }
    }
}