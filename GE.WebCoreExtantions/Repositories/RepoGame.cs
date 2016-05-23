using System.Linq;
using Dapper;
using System.Data.SqlClient;
using SX.WebCore.Abstract;

namespace GE.WebCoreExtantions.Repositories
{
    public sealed class RepoGame : SxDbRepository<int, Game, DbContext>
    {
        public override IQueryable<Game> All
        {
            get
            {
                using (var conn = new SqlConnection(base.ConnectionString))
                {
                    var result = conn.Query<Game>(@"SELECT *
FROM   D_GAME                  dg
ORDER BY
       dg.[TITLE]");
                    return result.AsQueryable();
                }
            }
        }
    }
}
