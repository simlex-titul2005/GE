using GE.WebCoreExtantions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Dapper;
using SX.WebCore.Abstract;
using GE.WebAdmin.Models;
using SX.WebCore;
using SX.WebCore.HtmlHelpers;

namespace GE.WebAdmin.Extantions.Repositories
{
    public static partial class RepositoryExtantions
    {
        public static IQueryable<VMPicture> QueryForAdmin(this GE.WebCoreExtantions.Repositories.RepoPicture repo, SxFilter filter, IDictionary<string, SxExtantions.SortDirection> order = null)
        {
            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                var query = @"SELECT dp.Id,
       dp.Caption,
       dp.[Description],
       dp.Width,
       dp.Height
FROM   D_PICTURE AS dp";

                //where
                if(filter.Additional!=null && filter.Additional[0]!=null)
                {
                    var addi = (VMPicture)filter.Additional[0];
                    query += @" WHERE";
                    query += whereClause(addi);
                }

                
                //order
                query += @" ORDER BY";
                fillOrder(order, ref query);
                query += @" dp.DateCreate DESC";

                if (filter != null && filter.SkipCount.HasValue && filter.PageSize.HasValue)
                    query += " OFFSET " + filter.SkipCount + " ROWS FETCH NEXT " + filter.PageSize + " ROWS ONLY";

                var data = conn.Query<VMPicture>(query);

                return data.AsQueryable();
            }
        }

        public static int FilterCount(this GE.WebCoreExtantions.Repositories.RepoPicture repo, Filter filter)
        {
            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                var query = @"SELECT COUNT(1) FROM D_PICTURE as dp";
                if (filter.Additional != null && filter.Additional[0] != null)
                {
                    var addi = (VMPicture)filter.Additional[0];
                    query += @" WHERE";
                    query += whereClause(addi);
                }
                var data = conn.Query<int>(query).Single();
                return (int)data;
            }
        }

        private static string whereClause(VMPicture filter)
        {
            string whereClause = null;
            if (filter.Id != Guid.Empty)
                whereClause += @" AND dp.Id LIKE N'%" + filter.Id + "%'";
            if (filter.Caption != null)
                whereClause += @" AND dp.Caption LIKE N'%" + filter.Caption + "%'";
            if (filter.Description != null)
                whereClause += @" AND dp.Description LIKE N'%" + filter.Description + "%'";
            if (filter.Width != 0)
                whereClause += @" AND dp.Width <=" + filter.Width;
            if (filter.Height != 0)
                whereClause += @" AND dp.Height <=" + filter.Height;
            whereClause = whereClause != null ? whereClause.Substring(4, whereClause.Length - 4) : null;
            return whereClause;
        }
        private static void fillOrder(IDictionary<string, SxExtantions.SortDirection> order, ref string query)
        {
            if (order != null && order.ContainsKey("Id") && order["Id"] != SxExtantions.SortDirection.Unknown)
                query += " dp.Id " + order["Id"] + ",";
            if (order != null && order.ContainsKey("Caption") && order["Caption"] != SxExtantions.SortDirection.Unknown)
                query += " dp.Caption " + order["Caption"] + ",";
            if (order != null && order.ContainsKey("Description") && order["Description"] != SxExtantions.SortDirection.Unknown)
                query += " dp.Description " + order["Description"] + ",";
            if (order != null && order.ContainsKey("Width") && order["Width"] != SxExtantions.SortDirection.Unknown)
                query += " dp.Width " + order["Width"] + ",";
            if (order != null && order.ContainsKey("Height") && order["Height"] != SxExtantions.SortDirection.Unknown)
                query += " dp.Height " + order["Height"] + ",";
        }
    }
}