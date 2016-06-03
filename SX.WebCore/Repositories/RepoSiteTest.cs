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
        public override IQueryable<SxSiteTest> All
        {
            get
            {
                var query = @"SELECT * FROM D_SITE_TEST AS dst";
                using (var conn = new SqlConnection(ConnectionString))
                {
                    var data = conn.Query<SxSiteTest>(query);
                    return data.AsQueryable();
                }
            }
        }

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
            query += " AND (dst.Description LIKE '%'+@desc+'%' OR @desc IS NULL) ";

            var title = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Title != null ? (string)filter.WhereExpressionObject.Title : null;
            var desc = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Description != null ? (string)filter.WhereExpressionObject.Description : null;

            param = new
            {
                title = title,
                desc=desc
            };

            return query;
        }

        public dynamic[] RandomList()
        {
            var query = @"SELECT TOP(3) dst.Title,
       dst.[Description],
       COUNT(DISTINCT(dstb.Id))   AS StepsCount,
       COUNT(DISTINCT(dstq.Id))   AS QuestionsCount
FROM   D_SITE_TEST                AS dst
       JOIN D_SITE_TEST_BLOCK     AS dstb
            ON  dstb.TestId = dst.Id
       JOIN D_SITE_TEST_QUESTION  AS dstq
            ON  dstq.BlockId = dstb.Id
GROUP BY
       dst.Title,
       dst.[Description]
ORDER BY NEWID()";
            using (var conn = new SqlConnection(ConnectionString))
            {

                var data = conn.Query(query);
                return data.ToArray();
            }
        }
    }
}
