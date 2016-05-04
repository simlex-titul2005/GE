using System.Data.SqlClient;
using System.Linq;
using Dapper;
using GE.WebAdmin.Models;
using static SX.WebCore.HtmlHelpers.SxExtantions;
using SX.WebCore.Providers;
using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;

namespace GE.WebAdmin.Extantions.Repositories
{
    public static partial class RepositoryExtantions
    {
        public static VMVideo[] QueryForAdmin(this RepoVideo repo, Filter filter)
        {
            var query = QueryProvider.GetSelectString();
            query += @" FROM D_VIDEO dv ";

            object param = null;
            query += getVideoWhereString(filter, out param);

            query += QueryProvider.GetOrderString("dv.DateCreate", SortDirection.Desc, filter.Orders);

            query += " OFFSET " + filter.PagerInfo.SkipCount + " ROWS FETCH NEXT " + filter.PagerInfo.PageSize + " ROWS ONLY";

            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                var data = conn.Query<VMVideo>(query, param: param).ToArray();
                return data.ToArray();
            }
        }
        public static int FilterCount(this RepoVideo repo, Filter filter)
        {
            var query = @"SELECT COUNT(1) FROM D_VIDEO dv ";

            object param = null;
            query += getVideoWhereString(filter, out param);

            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                var data = conn.Query<int>(query, param: param).Single();

                return data;
            }
        }

        private static string getVideoWhereString(Filter filter, out object param)
        {
            param = null;
            string query = null;
            query += " WHERE (dv.Title LIKE '%'+@title+'%' OR @title IS NULL) ";
            query += " AND (dv.Url LIKE '%'+@url+'%' OR @url IS NULL) ";

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