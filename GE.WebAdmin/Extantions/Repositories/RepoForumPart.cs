using System.Data.SqlClient;
using System.Linq;
using Dapper;
using GE.WebAdmin.Models;
using GE.WebCoreExtantions.Repositories;
using GE.WebCoreExtantions;
using SX.WebCore.Providers;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace GE.WebAdmin.Extantions.Repositories
{
    public static partial class RepositoryExtantions
    {
        public static VMForumPart[] QueryForAdmin(this RepoForumPart repo, Filter filter)
        {
            var query = QueryProvider.GetSelectString(new string[] {
                    "dfp.Id", "dfp.Title", "dfp.Html"
                });
            query += @" FROM D_FORUM_PART dfp ";

            object param = null;
            query += getForumPartWhereString(filter, out param);

            query += QueryProvider.GetOrderString("dfp.DateCreate", SortDirection.Desc, filter.Orders);

            query += " OFFSET " + filter.PagerInfo.SkipCount + " ROWS FETCH NEXT " + filter.PagerInfo.PageSize + " ROWS ONLY";

            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                var data = conn.Query<VMForumPart>(query, param: param);
                return data.ToArray();
            }
        }

        public static int FilterCount(this RepoForumPart repo, Filter filter)
        {
            var query = @"SELECT COUNT(1) FROM D_FORUM_PART dfp";

            object param = null;
            query += getForumPartWhereString(filter, out param);

            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                var data = conn.Query<int>(query, param: param).Single();

                return data;
            }
        }

        private static string getForumPartWhereString(Filter filter, out object param)
        {
            param = null;
            string query = null;
            query += " WHERE (dfp.Title LIKE '%'+@title+'%' OR @title IS NULL)";
            query += " AND (dfp.Html LIKE '%'+@html+'%' OR @html IS NULL)";

            var title = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Title != null ? (string)filter.WhereExpressionObject.Title : null;
            var html = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Html != null ? (string)filter.WhereExpressionObject.Html : null;

            param = new
            {
                title = title,
                html = html
            };

            return query;
        }
    }
}