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
    public sealed class RepoMenu : SxDbRepository<int, SxMenu, DbContext>
    {
        public override IQueryable<SxMenu> All
        {
            get
            {
                using (var conn = new SqlConnection(base.ConnectionString))
                {
                    var result = conn.Query<SxMenu>(@"SELECT *
FROM   D_MENU AS dm
ORDER BY
       dm.NAME");
                    return result.AsQueryable();
                }
            }
        }
    }
}
