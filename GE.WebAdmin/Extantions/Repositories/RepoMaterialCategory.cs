using System.Linq;
using GE.WebAdmin.Models;
using SX.WebCore.Providers;
using static SX.WebCore.HtmlHelpers.SxExtantions;
using System.Data.SqlClient;
using Dapper;
using SX.WebCore;

namespace GE.WebAdmin.Extantions.Repositories
{
    public static partial class RepositoryExtantions
    {
        public static VMMaterialCategory[] QueryForAdmin(this WebCoreExtantions.Repositories.RepoMaterialCategory repo, SxFilter filter)
        {
            var query = SxQueryProvider.GetSelectString(new string[] { "dmc.*" });
            query += " FROM D_MATERIAL_CATEGORY AS dmc ";

            object param = null;
            query += getMaterialGroupWhereString(filter, out param);

            var defaultOrder = new SxOrder { FieldName = "DateCreate", Direction = SortDirection.Desc };
            query += SxQueryProvider.GetOrderString(defaultOrder, filter.Order);

            using (var connection = new SqlConnection(repo.ConnectionString))
            {
                var data = connection.Query<VMMaterialCategory>(query, param: param);
                return data.ToArray();
            }
        }

        private static string getMaterialGroupWhereString(SxFilter filter, out object param)
        {
            param = null;
            string query = null;
            query += " WHERE dmc.ModelCoreType=@mct ";

            var mct = filter.ModelCoreType;

            param = new
            {
                mct = mct
            };

            return query;
        }
    }
}