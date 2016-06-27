using System.Data.SqlClient;
using System.Linq;
using Dapper;
using GE.WebCoreExtantions.Repositories;
using SX.WebCore.Providers;
using SX.WebCore;
using static SX.WebCore.HtmlHelpers.SxExtantions;
using SX.WebCore.ViewModels;

namespace GE.WebAdmin.Extantions.Repositories
{
    public static partial class RepositoryExtantions
    {
        public static SxVMMaterialTag[] QueryForAdmin(this RepoMaterialTag repo, SxFilter filter)
        {
            var query = SxQueryProvider.GetSelectString(new string[] { "dmt.Id", "dmt.DateCreate", "dmt.MaterialId", "dmt.ModelCoreType" });
            query += " FROM D_MATERIAL_TAG AS dmt";

            object param = null;
            query += getMaterialTagWhereString(filter, out param);

            var defaultOrder = new SxOrder { FieldName = "dmt.DateCreate", Direction = SortDirection.Desc };
            query += SxQueryProvider.GetOrderString(defaultOrder, filter.Order);
            query += " OFFSET " + filter.PagerInfo.SkipCount + " ROWS FETCH NEXT " + filter.PagerInfo.PageSize + " ROWS ONLY";

            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                var data = conn.Query<SxVMMaterialTag>(query, param: param);
                return data.ToArray();
            }
        }

        public static int FilterCount(this RepoMaterialTag repo, SxFilter filter)
        {
            var query = @"SELECT COUNT(1) FROM D_MATERIAL_TAG AS dmt";

            object param = null;
            query += getMaterialTagWhereString(filter, out param);

            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                var data = conn.Query<int>(query, param: param).Single();
                return data;
            }
        }

        private static string getMaterialTagWhereString(SxFilter filter, out object param)
        {
            param = null;
            string query = null;
            query += " WHERE (dmt.Id LIKE '%'+@id+'%' OR @id IS NULL)";
            query += " AND (dmt.MaterialId=@mid AND dmt.ModelCoreType=@mct) ";

            var id = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Id != null ? (string)filter.WhereExpressionObject.Id : null;

            param = new
            {
                id = id,
                mid = filter.MaterialId,
                mct = (byte)filter.ModelCoreType
            };

            return query;
        }
    }
}