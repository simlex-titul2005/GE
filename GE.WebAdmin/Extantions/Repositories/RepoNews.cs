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
        public static News GetByTitleUrl(this GE.WebCoreExtantions.Repositories.RepoNews repo, string titleUrl)
        {
            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                var query = @"SELECT*FROM D_ARTICLE AS da
JOIN DV_MATERIAL AS dm ON dm.ID = da.ID AND dm.ModelCoreType = da.ModelCoreType
WHERE dm.TitleUrl=@TITLE_URL";

                return conn.Query<News>(query, new { TITLE_URL = titleUrl }).FirstOrDefault();
            }
        }

        public static VMNews[] QueryForAdmin(this RepoNews repo, Filter filter)
        {
            var query = QueryProvider.GetSelectString(new string[] { "da.Id", "dm.DateCreate", "dm.Title", "dm.SeoInfoId" });
            query += " FROM D_NEWS AS da JOIN DV_MATERIAL AS dm ON dm.ID = da.ID AND dm.ModelCoreType = da.ModelCoreType LEFT JOIN D_GAME AS dg ON dg.ID = da.GameId ";

            object param = null;
            query += getNewsWhereString(filter, out param);

            query += QueryProvider.GetOrderString("dm.DateCreate", SortDirection.Desc, filter.Orders);

            if (filter != null && filter.SkipCount.HasValue && filter.PageSize.HasValue)
                query += " OFFSET " + filter.SkipCount + " ROWS FETCH NEXT " + filter.PageSize + " ROWS ONLY";

            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                var data = conn.Query<VMNews>(query, param: param);
                return data.ToArray();
            }
        }

        public static int FilterCount(this RepoNews repo, Filter filter)
        {
            var query = @"SELECT COUNT(1) FROM D_NEWS AS da JOIN DV_MATERIAL AS dm ON dm.ID = da.ID AND dm.ModelCoreType = da.ModelCoreType ";

            object param = null;
            query += getNewsWhereString(filter, out param);

            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                var data = conn.Query<int>(query, param: param).Single();
                return data;
            }
        }

        private static string getNewsWhereString(Filter filter, out object param)
        {
            param = null;
            string query = null;
            query += " WHERE (dm.Title LIKE '%'+@title+'%' OR @title IS NULL) ";

            var title = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Title != null ? (string)filter.WhereExpressionObject.Title : null;

            param = new
            {
                title = title
            };

            return query;
        }
    }
}