using System.Data.SqlClient;
using System.Linq;
using Dapper;
using SX.WebCore.Abstract;
using SX.WebCore;

namespace GE.WebCoreExtantions.Repositories
{
    public sealed class RepoClickType : SxDbRepository<int, SxClickType, DbContext>
    {
        public override IQueryable<SxClickType> Query(SxFilter filter)
        {
            var query = @"SELECT
	dct.Id, dct.Name, dct.[Description]
FROM D_CLICK_TYPE AS dct ORDER BY dct.Name";
            query += " OFFSET " + filter.PagerInfo.SkipCount + " ROWS FETCH NEXT " + filter.PagerInfo.PageSize + " ROWS ONLY";
            using (var conn = new SqlConnection(base.ConnectionString))
            {
                var data = conn.Query<SxClickType>(query);
                return data.AsQueryable();
            }
        }

        public override int Count(SxFilter filter)
        {
            var query = @"SELECT COUNT(1) FROM D_CLICK_TYPE AS dct";

            using (var conn = new SqlConnection(base.ConnectionString))
            {
                var data = conn.Query<int>(query).Single();
                return (int)data;
            }
        }
    }
}
