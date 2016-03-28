using GE.WebCoreExtantions;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using GE.WebAdmin.Models;
using SX.WebCore;
using SX.WebCore.HtmlHelpers;
using SX.WebCore.Providers;

namespace GE.WebAdmin.Extantions.Repositories
{
    public static partial class RepositoryExtantions
    {
        public static IQueryable<VMSeoInfo> QueryForAdmin(this WebCoreExtantions.Repositories.RepoSeoInfo repo, Filter filter)
        {
            var query = QueryProvider.GetSelectString(new string[] { "dsi.Id", "dsi.RawUrl" });
            query += " FROM D_SEO_INFO AS dsi ";

            object param = null;
            query += getSeoInfoWhereString(filter, out param);

            query += QueryProvider.GetOrderString("dsi.RawUrl", SxExtantions.SortDirection.Asc, filter.Orders);

            if (filter != null && filter.SkipCount.HasValue && filter.PageSize.HasValue)
                query += " OFFSET " + filter.SkipCount + " ROWS FETCH NEXT " + filter.PageSize + " ROWS ONLY";

            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                var data = conn.Query<VMSeoInfo>(query, param: param);
                return data.AsQueryable();
            }
        }

        public static int FilterCount(this WebCoreExtantions.Repositories.RepoSeoInfo repo, Filter filter)
        {
            var query = @"SELECT COUNT(1) FROM D_SEO_INFO as dsi";

            object param = null;
            query += getSeoInfoWhereString(filter, out param);

            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                var data = conn.Query<int>(query, param: param).Single();
                return data;
            }
        }

        private static string getSeoInfoWhereString(Filter filter, out object param)
        {
            param = null;
            string query = null;
            query += " WHERE (dsi.RawUrl LIKE '%'+@raw_url+'%' OR @raw_url IS NULL) ";

            var rawUrl = filter.WhereExpressionObject != null && filter.WhereExpressionObject.RawUrl != null ? (string)filter.WhereExpressionObject.RawUrl : null;

            param = new
            {
                raw_url = rawUrl
            };

            return query;
        }

        //найти страницу по rawUrl
        public static SxSeoInfo GetByRawUrl(this GE.WebCoreExtantions.Repositories.RepoSeoInfo repo, string rawUrl)
        {
            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                var query = @"SELECT dsi.Id FROM D_SEO_INFO AS dsi WHERE dsi.RawUrl=@RAW_URL";
                var data = conn.Query<SxSeoInfo>(query, new { @RAW_URL = rawUrl });
                return data.SingleOrDefault();
            }
        }
    }
}