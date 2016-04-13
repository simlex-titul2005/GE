using System.Linq;
using SX.WebCore.Abstract;
using System.Data.SqlClient;
using Dapper;
using SX.WebCore.Providers;
using static SX.WebCore.Enums;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.Repositories
{
    public sealed class RepoMaterialCategory<TDbContext> : SxDbRepository<string, SxMaterialCategory, TDbContext> where TDbContext : SxDbContext
    {
        public sealed override IQueryable<SxMaterialCategory> Query(SxFilter filter)
        {
            var query = QueryProvider.GetSelectString(new string[] { "dmc.*" });
            query += " FROM D_MATERIAL_CATEGORY AS dmc ";

            object param = null;
            query += getMaterialGroupWhereString(filter, out param);

            query += QueryProvider.GetOrderString("DateCreate", SortDirection.Desc);

            using (var connection = new SqlConnection(ConnectionString))
            {
                var data = connection.Query<SxMaterialCategory>(query, param: param);
                return data.AsQueryable();
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

        public sealed override SxMaterialCategory Update(SxMaterialCategory model, params string[] propertiesForChange)
        {
            var oldModel = GetByKey(model.Id);
            if (oldModel == null)
                return Create(model);
            else
            {
                return base.Update(model, propertiesForChange);
            }
        }

        //public SxManualGroup[] GetFindTable(SxFilter filter)
        //{
        //    var query = @"SELECT *
        //FROM   D_MANUAL_GROUP AS dmg
        //ORDER BY dmg.Title ";
        //    query += " OFFSET " + filter.PagerInfo.SkipCount + " ROWS FETCH NEXT " + filter.PagerInfo.PageSize + " ROWS ONLY";

        //    using (var connection = new SqlConnection(ConnectionString))
        //    {
        //        var data = connection.Query<SxManualGroup>(query);
        //        return data.ToArray();
        //    }

        //}
    }
}
