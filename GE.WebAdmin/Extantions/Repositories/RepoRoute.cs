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

                query += " WHERE (dr.Name LIKE '%'+@name+'%' OR @name IS NULL)";
                query += " AND (dr.Controller LIKE '%'+@c+'%' OR @c IS NULL)";
                query += " AND (dr.Action LIKE '%'+@a+'%' OR @a IS NULL)";

                query += QueryProvider.GetOrderString("dr.Name", SortDirection.Asc, filter.Orders);

                if (filter != null && filter.SkipCount.HasValue && filter.PageSize.HasValue)
                    query += " OFFSET " + filter.SkipCount + " ROWS FETCH NEXT " + filter.PageSize + " ROWS ONLY";

                var name = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Name != null ? filter.WhereExpressionObject.Name : null;
                var controller = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Controller != null ? filter.WhereExpressionObject.Controller : null;
                var action = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Action != null ? filter.WhereExpressionObject.Action : null;
                var data = conn.Query<VMRoute>(query, new
                {
                    name = (string)name,
                    c=(string)controller,
                    a=(string)action
                });

                return data.AsQueryable();
            }
        }

        public static int FilterCount(this WebCoreExtantions.Repositories.RepoRoute repo, Filter filter)
        {
            var name = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Name != null ? filter.WhereExpressionObject.Name : null;
            var controller = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Controller != null ? filter.WhereExpressionObject.Controller : null;
            var action = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Action != null ? filter.WhereExpressionObject.Action : null;
            var query = @"SELECT COUNT(1) FROM D_ROUTE AS dr";
            query += " WHERE (dr.Name LIKE '%'+@name+'%' OR @name IS NULL)";

            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                var data = conn.Query<int>(query, new
                {
                    name = (string)name,
                    c = (string)controller,
                    a = (string)action
                }).Single();

                return data;
            }
        }
    }
}