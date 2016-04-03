using GE.WebCoreExtantions;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using GE.WebAdmin.Models;
using SX.WebCore;
using SX.WebCore.HtmlHelpers;
using SX.WebCore.Providers;
using static SX.WebCore.Enums;
using GE.WebCoreExtantions.Repositories;

namespace GE.WebAdmin.Extantions.Repositories
{
    public static partial class RepositoryExtantions
    {
        public static VMSeoInfo[] QueryForAdmin(this RepoSeoInfo repo, Filter filter)
        {
            var query = QueryProvider.GetSelectString(new string[] { "dsi.Id", "dsi.RawUrl" });
            query += " FROM D_SEO_INFO AS dsi ";

            object param = null;
            query += getSeoInfoWhereString(filter, out param);

            query += QueryProvider.GetOrderString("dsi.RawUrl", SxExtantions.SortDirection.Asc, filter.Orders);

            query += " OFFSET " + filter.PagerInfo.SkipCount + " ROWS FETCH NEXT " + filter.PagerInfo.PageSize + " ROWS ONLY";

            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                var data = conn.Query<VMSeoInfo>(query, param: param);
                return data.ToArray();
            }
        }

        public static int FilterCount(this RepoSeoInfo repo, Filter filter)
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
            query += " WHERE (dsi.RawUrl LIKE '%'+@raw_url+'%' OR @raw_url IS NULL)";
            query += " AND (dsi.RawUrl IS NOT NULL) ";

            var rawUrl = filter.WhereExpressionObject != null && filter.WhereExpressionObject.RawUrl != null ? (string)filter.WhereExpressionObject.RawUrl : null;

            param = new
            {
                raw_url = rawUrl
            };

            return query;
        }

        /// <summary>
        /// Найти страницу по RawUrl
        /// </summary>
        /// <param name="repo"></param>
        /// <param name="rawUrl"></param>
        /// <returns></returns>
        public static SxSeoInfo GetByRawUrl(this RepoSeoInfo repo, string rawUrl)
        {
            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                var query = @"SELECT dsi.Id FROM D_SEO_INFO AS dsi WHERE dsi.RawUrl=@RAW_URL";
                var data = conn.Query<SxSeoInfo>(query, new { @RAW_URL = rawUrl });
                return data.SingleOrDefault();
            }
        }

        /// <summary>
        /// Удалить seo-теги материала
        /// </summary>
        /// <param name="repo"></param>
        /// <param name="mid"></param>
        /// <param name="mct"></param>
        public static void DeleteMaterialSeoInfo(this RepoSeoInfo repo, int mid, ModelCoreType mct)
        {
            var query = "DELETE FROM D_SEO_INFO WHERE Id IN (SELECT dsi.Id FROM D_SEO_INFO AS dsi WHERE MaterialId=@mid AND ModelCoreType=@mct)";
            using (var connection = new SqlConnection(repo.ConnectionString))
            {
                connection.Execute(query, new { mid = mid, mct = mct });
            }
        }

        /// <summary>
        /// Seo-теги материала
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="mct"></param>
        /// <returns></returns>
        public static SxSeoInfo GetMaterialSeoInfo(this RepoSeoInfo repo, int mid, ModelCoreType mct)
        {
            var query = "SELECT * FROM D_SEO_INFO AS dsi WHERE MaterialId=@mid AND ModelCoreType=@mct";
            using (var connection = new SqlConnection(repo.ConnectionString))
            {
                var data=connection.Query<SxSeoInfo>(query, new { mid = mid, mct = mct }).SingleOrDefault();
                return data;
            }
        }
    }
}