using GE.WebAdmin.Models;
using GE.WebCoreExtantions;
using SX.WebCore.Providers;
using System.Data.SqlClient;
using System.Linq;
using static SX.WebCore.HtmlHelpers.SxExtantions;
using Dapper;

namespace GE.WebAdmin.Extantions.Repositories
{
    public static partial class RepositoryExtantions
    {
        public static IQueryable<VMMenu> QueryForAdmin(this WebCoreExtantions.Repositories.RepoMenu repo, Filter filter)
        {
            var query = QueryProvider.GetSelectString(new string[] { "dm.Id", "dm.Name" });
            query += " FROM D_MENU AS dm ";

            object param = null;
            query += getMenuWhereString(filter, out param);

            query += QueryProvider.GetOrderString("dm.Name", SortDirection.Asc, filter.Orders);

            if (filter != null && filter.SkipCount.HasValue && filter.PageSize.HasValue)
                query += " OFFSET " + filter.SkipCount + " ROWS FETCH NEXT " + filter.PageSize + " ROWS ONLY";

            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                var data = conn.Query<VMMenu>(query, param: param);
                return data.AsQueryable();
            }
        }

        public static int FilterCount(this WebCoreExtantions.Repositories.RepoMenu repo, Filter filter)
        {
            var query = @"SELECT COUNT(1) FROM D_MENU as dm";

            object param = null;
            query += getMenuWhereString(filter, out param);

            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                var data = conn.Query<int>(query, param: param).Single();
                return data;
            }
        }

        private static string getMenuWhereString(Filter filter, out object param)
        {
            param = null;
            string query = null;
            query += " WHERE (dm.Name LIKE '%'+@name+'%' OR @name IS NULL) ";

            var name = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Name != null ? (string)filter.WhereExpressionObject.Name : null;

            param = new
            {
                name = name
            };

            return query;
        }
    }
}