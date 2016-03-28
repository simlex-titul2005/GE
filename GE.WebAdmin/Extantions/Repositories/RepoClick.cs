using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using GE.WebAdmin.Models;
using static SX.WebCore.HtmlHelpers.SxExtantions;
using SX.WebCore.Providers;
using GE.WebCoreExtantions;

namespace GE.WebAdmin.Extantions.Repositories
{
    public static partial class RepositoryExtantions
    {
        public static IEnumerable<VMClick> QueryForAdmin(this WebCoreExtantions.Repositories.RepoClick repo, Filter filter)
        {
            var query = QueryProvider.GetSelectString(new string[] {
                    "dc.Id", "dc.UrlRef", "dc.RawUrl", "dc.LinkTarget", "dct.Name AS ClickTypeName", "dc.DateCreate"
                });
            query += @" FROM D_CLICK AS dc JOIN D_CLICK_TYPE  AS dct ON dct.Id = dc.ClickTypeId";

            object param = null;
            query += getClickWhereString(filter, out param);

            query += QueryProvider.GetOrderString("dc.DateCreate", SortDirection.Desc, filter.Orders);

            if (filter != null && filter.SkipCount.HasValue && filter.PageSize.HasValue)
                query += " OFFSET " + filter.SkipCount + " ROWS FETCH NEXT " + filter.PageSize + " ROWS ONLY";

            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                var data = conn.Query<VMClick>(query, param: param);

                return data.AsEnumerable();
            }
        }

        public static int FilterCount(this WebCoreExtantions.Repositories.RepoClick repo, Filter filter)
        {
            var query = @"SELECT COUNT(1) FROM D_CLICK AS dc JOIN D_CLICK_TYPE  AS dct ON dct.Id = dc.ClickTypeId";

            object param = null;
            query += getClickWhereString(filter, out param);

            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                var data = conn.Query<int>(query, param: param).Single();

                return data;
            }
        }

        private static string getClickWhereString(Filter filter, out object param)
        {
            param = null;
            string query = null;
            query += " WHERE (dc.UrlRef LIKE '%'+@url_ref+'%' OR @url_ref IS NULL)";
            query += " AND (dc.RawUrl LIKE '%'+@raw_url+'%' OR @raw_url IS NULL)";
            query += " AND (dct.Name LIKE '%'+@ctm+'%' OR @ctm IS NULL) ";

            var urlRef = filter.WhereExpressionObject != null && filter.WhereExpressionObject.UrlRef != null ? (string)filter.WhereExpressionObject.UrlRef : null;
            var rawUrl = filter.WhereExpressionObject != null && filter.WhereExpressionObject.RawUrl != null ? (string)filter.WhereExpressionObject.RawUrl : null;
            var ctm = filter.WhereExpressionObject != null && filter.WhereExpressionObject.ClickTypeName != null ? (string)filter.WhereExpressionObject.ClickTypeName : null;

            param = new
            {
                url_ref = urlRef,
                raw_url=rawUrl,
                ctm=ctm
            };

            return query;
        }
    }
}