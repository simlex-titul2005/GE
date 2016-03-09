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
    public sealed class RepoClickType : SxDbRepository<int, SxClickType, DbContext>
    {
        public override IQueryable<SxClickType> Query(SxFilter filter)
        {
            var f = (Filter)filter;
            using (var conn = new SqlConnection(base.ConnectionString))
            {
                var query = @"SELECT
	dct.Id, dct.Name, dct.[Description]
FROM D_CLICK_TYPE AS dct ORDER BY dct.Name";
                if (f != null && f.SkipCount.HasValue && f.PageSize.HasValue)
                    query += " OFFSET " + filter.SkipCount + " ROWS FETCH NEXT " + filter.PageSize + " ROWS ONLY";

                var data = conn.Query<SxClickType>(query);

                return data.AsQueryable();
            }
        }

        public override int Count(SxFilter filter)
        {
            var f = (Filter)filter;
            using (var conn = new SqlConnection(base.ConnectionString))
            {
                var query = @"SELECT COUNT(1) FROM D_CLICK_TYPE AS dct";
                var data = conn.Query<int>(query).Single();

                return (int)data;
            }
        }
    }
}
