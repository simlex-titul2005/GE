using Dapper;
using SX.WebCore;
using SX.WebCore.Abstract;
using SX.WebCore.Providers;
using System.Data.SqlClient;
using System.Linq;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace GE.WebCoreExtantions.Repositories
{
    public sealed class RepoAphorism : SxDbRepository<int, Aphorism, DbContext>
    {

        public override IQueryable<Aphorism> Query(SxFilter filter)
        {
            var f = (Filter)filter;
            var query = QueryProvider.GetSelectString(new string[] { "dm.*" });
            query += @" FROM D_APHORISM AS da
JOIN DV_MATERIAL AS dm ON dm.Id = da.Id AND dm.ModelCoreType = da.ModelCoreType
JOIN D_MATERIAL_CATEGORY AS dmc ON dmc.Id = dm.CategoryId";

            object param = null;
            query += getAphorismsWhereString(f, out param);

            query += QueryProvider.GetOrderString("dm.DateCreate", SortDirection.Desc);

            query += " OFFSET " + filter.PagerInfo.SkipCount + " ROWS FETCH NEXT " + filter.PagerInfo.PageSize + " ROWS ONLY";

            using (var conn = new SqlConnection(base.ConnectionString))
            {
                var data = conn.Query<Aphorism, SxMaterialCategory, Aphorism>(query, (a, c) => {
                    a.Category = c;
                    return a;
                }, param: param, splitOn: "CategoryId");

                return data.AsQueryable();
            }
        }

        public override int Count(SxFilter filter)
        {
            var f = (Filter)filter;
            var query = @"SELECT COUNT(1) FROM D_APHORISM AS da
JOIN DV_MATERIAL AS dm ON dm.Id = da.Id AND dm.ModelCoreType = da.ModelCoreType
JOIN D_MATERIAL_CATEGORY AS dmc ON dmc.Id = dm.CategoryId ";

            object param = null;
            query += getAphorismsWhereString(f, out param);

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<int>(query, param: param).Single();
                return data;
            }
        }

        private static string getAphorismsWhereString(Filter filter, out object param)
        {
            param = null;
            string query = null;
            query += " WHERE dm.CategoryId=@cid OR @cid IS NULL ";

            var cid = filter.WhereExpressionObject != null && filter.WhereExpressionObject.CategoryId != null ? (string)filter.WhereExpressionObject.CategoryId : null;

            param = new
            {
                cid = cid
            };

            return query;
        }
    }
}
