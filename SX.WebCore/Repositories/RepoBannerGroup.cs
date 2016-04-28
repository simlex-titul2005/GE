using Dapper;
using SX.WebCore.Abstract;
using SX.WebCore.Providers;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.Repositories
{
    public sealed class RepoBannerGroup<TDbContext> : SxDbRepository<Guid, SxBannerGroup, TDbContext> where TDbContext : SxDbContext
    {
        public override IQueryable<SxBannerGroup> Query(SxFilter filter)
        {
            var query = QueryProvider.GetSelectString();
            query += " FROM D_BANNER_GROUP AS dbg ";

            object param = null;
            query += getBannerGroupWhereString(filter, out param);

            query += QueryProvider.GetOrderString("dbg.DateCreate", SortDirection.Desc, filter.Orders);

            query += " OFFSET " + filter.PagerInfo.SkipCount + " ROWS FETCH NEXT " + filter.PagerInfo.PageSize + " ROWS ONLY";

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxBannerGroup>(query, param: param);
                return data.AsQueryable();
            }
        }

        public override int Count(SxFilter filter)
        {
            var query = @"SELECT COUNT(1) FROM D_BANNER_GROUP AS dbg ";

            object param = null;
            query += getBannerGroupWhereString(filter, out param);

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<int>(query, param: param).Single();
                return data;
            }
        }

        private static string getBannerGroupWhereString(SxFilter filter, out object param)
        {
            param = null;
            string query = null;
            query += " WHERE (dbg.Title LIKE '%'+@title+'%' OR @title IS NULL) ";
            query += " AND (dbg.Description LIKE '%'+@desc+'%' OR @desc IS NULL) ";

            var title = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Title != null ? (string)filter.WhereExpressionObject.Title : null;
            var desc = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Description != null ? (string)filter.WhereExpressionObject.Description : null;

            param = new
            {
                title = title,
                desc = desc
            };

            return query;
        }

        public void AddBanners(Guid bannerGroupId, Guid[] bannersId)
        {
            var query = @"BEGIN TRANSACTION

DELETE 
FROM   D_BANNER_GROUP_LINK
WHERE  BannerGroupId = @bgid

INSERT INTO D_BANNER_GROUP_LINK
SELECT bgid.[GUID],
       @bgid
FROM   @bnsid AS bgid

COMMIT TRANSACTION";

            var cmd = new SqlCommand(query);
            cmd.Parameters.AddWithValue("bgid", bannerGroupId);
            cmd.Parameters.Add(getGuidTablePar(bannersId));

            using (var conn = new SqlConnection(ConnectionString))
            {
                using (cmd)
                {
                    cmd.Connection = conn;
                    conn.Open();
                    @cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }

        }

        private static SqlParameter getGuidTablePar(Guid[] bannersId)
        {
            var par = new SqlParameter();
            par.ParameterName = "bnsid";
            par.SqlDbType = SqlDbType.Structured;
            par.TypeName = "GUID_LIST";
            var table = new DataTable();
            table.Columns.Add(new DataColumn("GUID"));
            for (int i = 0; i < bannersId.Length; i++)
            {
                table.Rows.Add(bannersId[i]);
            }
            par.Value = table;

            return par;
        }
    }
}
