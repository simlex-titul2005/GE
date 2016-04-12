using System.Linq;
using SX.WebCore.Abstract;
using System.Data.SqlClient;
using Dapper;
using SX.WebCore.Providers;

namespace SX.WebCore.Repositories
{
    public sealed class RepoManualGroup<TDbContext> : SxDbRepository<string, SxManualGroup, TDbContext> where TDbContext : SxDbContext
    {
        public sealed override IQueryable<SxManualGroup> Query(SxFilter filter)
        {
            var query = QueryProvider.GetSelectString(new string[] { "dmg.*" });
            query += " FROM D_MANUAL_GROUP AS dmg LEFT JOIN D_MANUAL_GROUP AS dmg2 ON dmg2.Id = dmg.ParentGroupId ";

            object param = null;
            query += getManualGroupWhereString(filter, out param);

            using (var connection = new SqlConnection(ConnectionString))
            {
                var data = connection.Query<SxManualGroup>(query, param: param);
                return data.AsQueryable();
            }
        }

        private static string getManualGroupWhereString(SxFilter filter, out object param)
        {
            param = null;
            string query = null;
            query += " WHERE (dmg.Title LIKE '%'+@title+'%' OR @title IS NULL) ";

            var title = filter!=null && filter.WhereExpressionObject != null && filter.WhereExpressionObject.Title != null ? (string)filter.WhereExpressionObject.Title : null;

            param = new
            {
                title = title
            };

            return query;
        }

        public sealed override SxManualGroup Update(SxManualGroup model, params string[] propertiesForChange)
        {
            var oldModel = GetByKey(model.Id);
            if (oldModel == null)
                return Create(model);
            else
            {
                return base.Update(model, propertiesForChange);
            }
        }

        public SxManualGroup[] GetFindTable(SxFilter filter)
        {
            var query = @"SELECT *
FROM   D_MANUAL_GROUP AS dmg
ORDER BY dmg.Title ";
            query += " OFFSET " + filter.PagerInfo.SkipCount + " ROWS FETCH NEXT " + filter.PagerInfo.PageSize + " ROWS ONLY";

            using (var connection = new SqlConnection(ConnectionString))
            {
                var data = connection.Query<SxManualGroup>(query);
                return data.ToArray();
            }

        }
    }
}
