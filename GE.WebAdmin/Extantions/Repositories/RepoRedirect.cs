using GE.WebCoreExtantions;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using GE.WebCoreExtantions.Repositories;
using SX.WebCore.HtmlHelpers;
using SX.WebCore.Providers;
using GE.WebAdmin.Models;

namespace GE.WebAdmin.Extantions.Repositories
{
    public static partial class RepositoryExtantions
    {
        public static VMRedirect[] QueryForAdmin(this RepoRedirect repo, Filter filter)
        {
            var query = QueryProvider.GetSelectString(new string[] { "dr.Id", "dr.OldUrl", "dr.NewUrl", "dr.DateCreate" });
            query += " FROM   D_REDIRECT dr";

            object param = null;
            query += getRedirectWhereString(filter, out param);

            query += QueryProvider.GetOrderString("dr.DateCreate", SxExtantions.SortDirection.Desc, filter.Orders);

            query += " OFFSET " + filter.PagerInfo.SkipCount + " ROWS FETCH NEXT " + filter.PagerInfo.PageSize + " ROWS ONLY";

            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                var data = conn.Query<VMRedirect>(query, param: param);
                return data.ToArray();
            }
        }

        public static int FilterCount(this RepoRedirect repo, Filter filter)
        {
            var query = @"SELECT COUNT(1) FROM   D_REDIRECT dr";

            object param = null;
            query += getRedirectWhereString(filter, out param);

            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                var data = conn.Query<int>(query, param: param).Single();
                return data;
            }
        }

        private static string getRedirectWhereString(Filter filter, out object param)
        {
            param = null;
            string query = null;
            query += " WHERE (dr.OldUrl LIKE '%'+@old_url+'%' OR @old_url IS NULL)";
            query += " AND (dr.NewUrl LIKE '%'+@new_url+'%' OR @new_url IS NULL)";

            var oldUrl = filter.WhereExpressionObject != null && filter.WhereExpressionObject.OldUrl != null ? (string)filter.WhereExpressionObject.OldUrl : null;
            var newUrl = filter.WhereExpressionObject != null && filter.WhereExpressionObject.NewUrl != null ? (string)filter.WhereExpressionObject.NewUrl : null;

            param = new
            {
                old_url = oldUrl,
                new_url = newUrl
            };

            return query;
        }
    }
}