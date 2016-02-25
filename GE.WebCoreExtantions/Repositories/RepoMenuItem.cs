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
    public sealed class RepoMenuItem : SxDbRepository<int, SxMenuItem, DbContext>
    {
        public override IQueryable<SxMenuItem> All
        {
            get
            {
                using (var conn = new SqlConnection(base.ConnectionString))
                {
                    var result = conn.Query<SxMenuItem>(@"SELECT*FROM D_MENU_ITEM AS dmi");
                    return result.AsQueryable();
                }
            }
        }
    }
}
