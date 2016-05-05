using System.Linq;
using SX.WebCore.Abstract;
using System.Data.SqlClient;
using Dapper;
using SX.WebCore.Providers;
using static SX.WebCore.HtmlHelpers.SxExtantions;
using System.Collections.Generic;

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

        public sealed override SxMaterialCategory  Update(SxMaterialCategory model, bool changeDateUpdate = true, params string[] propertiesForChange)
        {
            var oldModel = GetByKey(model.Id);
            if (oldModel == null)
            {
                return Create(model);
            }
            else
            {
                return base.Update(model, changeDateUpdate, propertiesForChange);
            }
        }

        public sealed override void Delete(params object[] id)
        {
            var key = (string)id[0];
            List<string> idForDel = new List<string>();

            SxMaterialCategory[] all = null;
            var query = @"SELECT dmc.Id, dmc.ParentCategoryId FROM D_MATERIAL_CATEGORY dmc";
            using (var connection = new SqlConnection(ConnectionString))
            {
                all = connection.Query<SxMaterialCategory>(query).ToArray();
            }
            
            collectIdForDelete(key, all, ref idForDel);

            string keys = string.Empty;
            idForDel.ForEach(x=> { keys += ",'" + x+"'"; });
            keys = keys.Substring(1);
            query = @"BEGIN TRANSACTION
UPDATE DV_MATERIAL SET CategoryId=NULL WHERE CategoryId IN (" + keys + @")
DELETE FROM D_MATERIAL_CATEGORY WHERE Id in (" + keys + @")
COMMIT TRANSACTION";

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Execute(query);
            }
        }

        private static void collectIdForDelete(string key, SxMaterialCategory[] all, ref List<string> idForDel)
        {
            idForDel.Add(key);
            var cur = all.SingleOrDefault(x => x.Id == key);
            var childs = all.Where(x => x.ParentCategoryId == key).ToArray();

            if (!childs.Any())
                return;

            for (int i = 0; i < childs.Length; i++)
            {
                var child = childs[i];
                collectIdForDelete(child.Id, all, ref idForDel);
            }
        }
    }
}
