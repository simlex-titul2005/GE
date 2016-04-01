using Dapper;
using GE.WebAdmin.Models;
using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using SX.WebCore.Providers;
using System.Data.SqlClient;
using System.Linq;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace GE.WebAdmin.Extantions.Repositories
{
    public static partial class RepositoryExtantions
    {
        public static VMSeoKeyword[] QueryForAdmin(this RepoSeoKeywords repo, Filter filter)
        {
            var query = QueryProvider.GetSelectString(new string[] {
                    "dsk.Id", "dsk.Value"
                });
            query += @" FROM D_SEO_KEYWORD AS dsk ";

            object param = null;
            query += getSeoKeywordsWhereString(filter, out param);

            query += QueryProvider.GetOrderString("dsk.Value", SortDirection.Asc, filter.Orders);

            if (filter != null && filter.SkipCount.HasValue && filter.PageSize.HasValue)
                query += " OFFSET " + filter.SkipCount + " ROWS FETCH NEXT " + filter.PageSize + " ROWS ONLY";

            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                try {
                    var data = conn.Query<VMSeoKeyword>(query, param: param);
                    return data.ToArray();
                }
                catch
                {
                    return new VMSeoKeyword[0];
                }
            }
        }
        public static int FilterCount(this RepoSeoKeywords repo, Filter filter)
        {
            var query = @"SELECT COUNT(1) FROM D_SEO_KEYWORD AS dsk";

            object param = null;
            query += getSeoKeywordsWhereString(filter, out param);

            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                var data = conn.Query<int>(query, param: param).Single();
                return data;
            }
        }

        private static string getSeoKeywordsWhereString(Filter filter, out object param)
        {
            param = null;
            string query = null;
            query += " WHERE (dsk.SeoInfoId=@siid)";
            query += " AND (dsk.Value LIKE '%'+@val+'%' OR @val IS NULL)";

            var val = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Value != null ? (string)filter.WhereExpressionObject.Value : null;
            var siid = filter.WhereExpressionObject != null && filter.WhereExpressionObject.SeoInfoId != null ? (int?)filter.WhereExpressionObject.SeoInfoId : null;

            param = new
            {
                siid = siid,
                val = val
            };

            return query;
        }
    }
}