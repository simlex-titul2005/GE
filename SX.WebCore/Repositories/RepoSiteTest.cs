using Dapper;
using SX.WebCore.Abstract;
using SX.WebCore.Providers;
using System.Data.SqlClient;
using System.Linq;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.Repositories
{
    public sealed class RepoSiteTest<TDbContext> : SxDbRepository<int, SxSiteTest, TDbContext> where TDbContext : SxDbContext
    {
        public override IQueryable<SxSiteTest> Query(SxFilter filter)
        {
            var query = QueryProvider.GetSelectString();
            query += " FROM D_SITE_TEST AS dst ";

            object param = null;
            query += getSiteTestWhereString(filter, out param);

            query += QueryProvider.GetOrderString("dst.DateCreate", SortDirection.Desc, filter.Orders);

            query += " OFFSET " + filter.PagerInfo.SkipCount + " ROWS FETCH NEXT " + filter.PagerInfo.PageSize + " ROWS ONLY";

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxSiteTest>(query, param: param);
                return data.AsQueryable();
            }
        }

        public override int Count(SxFilter filter)
        {
            var query = @"SELECT COUNT(1) FROM D_SITE_TEST AS dst ";

            object param = null;
            query += getSiteTestWhereString(filter, out param);

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<int>(query, param: param).Single();
                return data;
            }
        }

        private static string getSiteTestWhereString(SxFilter filter, out object param)
        {
            param = null;
            string query = null;
            query += " WHERE (dst.Title LIKE '%'+@title+'%' OR @title IS NULL) ";

            var title = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Title != null ? (string)filter.WhereExpressionObject.Title : null;

            param = new
            {
                title = title
            };

            return query;
        }
    }
}
