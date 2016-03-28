using GE.WebCoreExtantions;
using System;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using GE.WebAdmin.Models;
using SX.WebCore.HtmlHelpers;
using SX.WebCore.Providers;

namespace GE.WebAdmin.Extantions.Repositories
{
    public static partial class RepositoryExtantions
    {
        public static IQueryable<VMPicture> QueryForAdmin(this WebCoreExtantions.Repositories.RepoPicture repo, Filter filter)
        {
            var query = QueryProvider.GetSelectString(new string[] { "dp.Id", "dp.Caption", "dp.[Description]", "dp.Width", "dp.Height" });
            query += " FROM D_PICTURE AS dp";

            object param = null;
            query += getPictureWhereString(filter, out param);

            query += QueryProvider.GetOrderString("dp.DateCreate", SxExtantions.SortDirection.Desc, filter.Orders);

            if (filter != null && filter.SkipCount.HasValue && filter.PageSize.HasValue)
                query += " OFFSET " + filter.SkipCount + " ROWS FETCH NEXT " + filter.PageSize + " ROWS ONLY";

            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                var data = conn.Query<VMPicture>(query, param: param);
                return data.AsQueryable();
            }
        }

        public static int FilterCount(this WebCoreExtantions.Repositories.RepoPicture repo, Filter filter)
        {
            var query = @"SELECT COUNT(1) FROM D_PICTURE as dp";

            object param = null;
            query += getPictureWhereString(filter, out param);

            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                var data = conn.Query<int>(query, param: param).Single();
                return data;
            }
        }

        private static string getPictureWhereString(Filter filter, out object param)
        {
            param = null;
            string query = null;
            query += " WHERE (dp.Caption LIKE '%'+@caption+'%' OR @caption IS NULL)";
            query += " AND (dp.[Description] LIKE '%'+@desc+'%' OR @desc IS NULL)";
            query += " AND (dp.Width >=@w OR @w=0)";
            query += " AND (dp.Height >=@h OR @h=0)";

            var caption = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Caption != null ? (string)filter.WhereExpressionObject.Caption : null;
            var desc = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Description != null ? (string)filter.WhereExpressionObject.Description : null;
            var w = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Width != null ? (int)filter.WhereExpressionObject.Width : 0;
            var h = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Height != null ? (int)filter.WhereExpressionObject.Height : 0;

            param = new
            {
                caption = caption,
                desc = desc,
                w = w,
                h = h
            };

            return query;
        }
    }
}