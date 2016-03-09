using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using SX.WebCore.Abstract;
using SX.WebCore;

namespace GE.WebCoreExtantions.Repositories
{
    public sealed class RepoClick : SxDbRepository<Guid, SxClick, DbContext>
    {
        public override int Count(SxFilter filter)
        {
            var f = (Filter)filter;
            using (var conn = new SqlConnection(base.ConnectionString))
            {
                var query = @"SELECT COUNT(1) FROM D_CLICK AS dc";
                var data = conn.Query<int>(query).Single();

                return (int)data;
            }
        }
    }
}
