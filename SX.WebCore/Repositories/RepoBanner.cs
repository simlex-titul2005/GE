using Dapper;
using SX.WebCore.Abstract;
using SX.WebCore.Providers;
using System;
using System.Data.SqlClient;
using System.Linq;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.Repositories
{
    public sealed class RepoBanner<TDbContext> : SxDbRepository<Guid, SxBanner, TDbContext> where TDbContext : SxDbContext
    {
        public override IQueryable<SxBanner> Query(SxFilter filter)
        {
            var query = QueryProvider.GetSelectString();
            query += " FROM D_BANNER AS db ";

            object param = null;
            query += getBannerWhereString(filter, out param);

            query += QueryProvider.GetOrderString("db.DateCreate", SortDirection.Desc, filter.Orders);

            query += " OFFSET " + filter.PagerInfo.SkipCount + " ROWS FETCH NEXT " + filter.PagerInfo.PageSize + " ROWS ONLY";

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxBanner>(query, param: param);
                return data.AsQueryable();
            }
        }

        public override int Count(SxFilter filter)
        {
            var query = @"SELECT COUNT(1) FROM D_BANNER AS db ";

            object param = null;
            query += getBannerWhereString(filter, out param);

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<int>(query, param: param).Single();
                return data;
            }
        }

        private static string getBannerWhereString(SxFilter filter, out object param)
        {
            param = null;
            string query = null;
            query += " WHERE (db.Url LIKE '%'+@url+'%' OR @url IS NULL) ";
            query += " AND (db.Title LIKE '%'+@title+'%' OR @title IS NULL) ";

            var title = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Title != null ? (string)filter.WhereExpressionObject.Title : null;
            var url = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Url != null ? (string)filter.WhereExpressionObject.Url : null;

            param = new
            {
                title = title,
                url=url
            };

            return query;
        }
    }
}
