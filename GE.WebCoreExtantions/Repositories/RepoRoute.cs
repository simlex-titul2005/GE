using SX.WebCore;
using SX.WebCore.Abstract;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace GE.WebCoreExtantions.Repositories
{
    public sealed class RepoRoute : SxDbRepository<Guid, SxRoute, DbContext>
    {
        public override IQueryable<SxRoute> All
        {
            get
            {
                using (var conn = new SqlConnection(base.ConnectionString))
                {
                    var result = conn.Query<SxRoute>(@"SELECT*FROM D_ROUTE AS dr ORDER BY dr.CONTROLLER, dr.[ACTION]");
                    return result.AsQueryable();
                }
            }
        }
    }
}
