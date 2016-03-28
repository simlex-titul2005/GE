using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using GE.WebAdmin.Models;
using SX.WebCore.Abstract;
using static SX.WebCore.HtmlHelpers.SxExtantions;
using SX.WebCore.Providers;
using GE.WebCoreExtantions;

namespace GE.WebAdmin.Extantions.Repositories
{
    public static partial class RepositoryExtantions
    {
        public static IEnumerable<VMRoute> QueryForAdmin(this WebCoreExtantions.Repositories.RepoRoute repo, Filter filter)
        {
            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                var query = QueryProvider.GetSelectString();
                query += @" FROM D_ROUTE AS dr";

                object param = null;
                query += getRoutetWhereString(filter, out param);

                query += QueryProvider.GetOrderString("dr.Name", SortDirection.Asc, filter.Orders);

                if (filter != null && filter.SkipCount.HasValue && filter.PageSize.HasValue)
                    query += " OFFSET " + filter.SkipCount + " ROWS FETCH NEXT " + filter.PageSize + " ROWS ONLY";

                var data = conn.Query<VMRoute>(query, param: param);

                return data.AsQueryable();
            }
        }

        public static int FilterCount(this WebCoreExtantions.Repositories.RepoRoute repo, Filter filter)
        {
            var query = @"SELECT COUNT(1) FROM D_ROUTE AS dr";

            object param = null;
            query += getRoutetWhereString(filter, out param);

            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                var data = conn.Query<int>(query, param: param).Single();

                return data;
            }
        }

        private static string getRoutetWhereString(Filter filter, out object param)
        {
            param = null;
            string query = null;
            query += " WHERE (dr.Name LIKE '%'+@name+'%' OR @name IS NULL)";
            query += " AND (dr.Controller LIKE '%'+@c+'%' OR @c IS NULL)";
            query += " AND (dr.Action LIKE '%'+@a+'%' OR @a IS NULL)";

            var name = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Name != null ? (string)filter.WhereExpressionObject.Name : null;
            var c = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Controller != null ? (string)filter.WhereExpressionObject.Controller : null;
            var a = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Action != null ? (string)filter.WhereExpressionObject.Action : null;

            param = new
            {
                name = name,
                c = c,
                a = a
            };

            return query;
        }
    }
}