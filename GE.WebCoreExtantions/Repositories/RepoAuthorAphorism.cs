using Dapper;
using SX.WebCore;
using SX.WebCore.Abstract;
using SX.WebCore.Providers;
using System.Data.SqlClient;
using System.Linq;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace GE.WebCoreExtantions.Repositories
{
    public sealed class RepoAuthorAphorism : SxDbRepository<int, AuthorAphorism, DbContext>
    {

        public override AuthorAphorism[] Query(SxFilter filter)
        {
            var query = SxQueryProvider.GetSelectString();
            query += @" FROM D_AUTHOR_APHORISM AS daa";

            object param = null;
            query += getAuthorAphorismsWhereString(filter, out param);

            var defaultOrder = new SxOrder { FieldName = "daa.Name", Direction = SortDirection.Asc };
            query += SxQueryProvider.GetOrderString(defaultOrder, filter.Order);

            query += " OFFSET " + filter.PagerInfo.SkipCount + " ROWS FETCH NEXT " + filter.PagerInfo.PageSize + " ROWS ONLY";

            using (var conn = new SqlConnection(base.ConnectionString))
            {
                var data = conn.Query<AuthorAphorism>(query, param: param);
                return data.ToArray();
            }
        }

        public override int Count(SxFilter filter)
        {
            var query = @"SELECT COUNT(1) FROM D_AUTHOR_APHORISM AS daa ";

            object param = null;
            query += getAuthorAphorismsWhereString(filter, out param);

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<int>(query, param: param).Single();
                return data;
            }
        }

        public override void Delete(params object[] id)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Execute("del_author_aphorism @authorId", new { authorId = id[0] });
            }
        }

        private static string getAuthorAphorismsWhereString(SxFilter filter, out object param)
        {
            param = null;
            string query = null;
            query += " WHERE (daa.Name LIKE '%'+@name+'%' OR @name IS NULL) ";
            query += " AND (daa.Description LIKE '%'+@desc+'%' OR @desc IS NULL) ";

            var name = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Name != null ? (string)filter.WhereExpressionObject.Name : null;
            var desc = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Description != null ? (string)filter.WhereExpressionObject.Description : null;

            param = new
            {
                name = name,
                desc = desc
            };

            return query;
        }
    }
}
