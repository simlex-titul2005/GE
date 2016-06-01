using Dapper;
using SX.WebCore.Abstract;
using SX.WebCore.Providers;
using System.Data.SqlClient;
using System.Linq;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.Repositories
{
    public sealed class RepoSiteTestBlock<TDbContext> : SxDbRepository<int, SxSiteTestBlock, TDbContext> where TDbContext : SxDbContext
    {
        public override IQueryable<SxSiteTestBlock> Query(SxFilter filter)
        {
            var query = QueryProvider.GetSelectString();
            query += @" FROM D_SITE_TEST_BLOCK AS dstb
JOIN D_SITE_TEST AS dst ON dst.Id = dstb.TestId ";

            object param = null;
            query += getSiteTestBlockWhereString(filter, out param);

            query += QueryProvider.GetOrderString("dstb.DateCreate", SortDirection.Desc, filter.Orders);

            query += " OFFSET " + filter.PagerInfo.SkipCount + " ROWS FETCH NEXT " + filter.PagerInfo.PageSize + " ROWS ONLY";

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxSiteTestBlock, SxSiteTest, SxSiteTestBlock>(query, (b, t)=> {
                    b.Test = t;
                    return b;
                }, param: param, splitOn:"TestId");
                return data.AsQueryable();
            }
        }

        public override int Count(SxFilter filter)
        {
            var query = @"SELECT COUNT(1) FROM D_SITE_TEST AS dst ";

            object param = null;
            query += getSiteTestBlockWhereString(filter, out param);

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<int>(query, param: param).Single();
                return data;
            }
        }

        private static string getSiteTestBlockWhereString(SxFilter filter, out object param)
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
