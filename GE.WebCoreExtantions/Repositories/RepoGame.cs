using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;

namespace GE.WebCoreExtantions.Repositories
{
    public sealed class RepoGame : SX.WebCore.Abstract.SxDbRepository<int, Game, DbContext>
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
