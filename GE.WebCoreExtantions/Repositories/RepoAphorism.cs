using Dapper;
using SX.WebCore.Abstract;
using System.Data.SqlClient;
using System.Linq;

namespace GE.WebCoreExtantions.Repositories
{
    public sealed class RepoAphorism : SxDbRepository<int, Aphorism, DbContext>
    {
        public string[] Categories
        {
            get
            {
                var query = @"SELECT da.Category
FROM   D_APHORISM AS da
GROUP BY
       da.Category
ORDER BY
       da.Category";
                using (var conn = new SqlConnection(ConnectionString))
                {
                    var data = conn.Query<string>(query).ToArray();
                    return data;
                }
            }
        }
    }
}
