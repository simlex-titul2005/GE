﻿using Dapper;
using GE.WebAdmin.Models;
using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using SX.WebCore.Providers;
using System;
using System.Data.SqlClient;
using System.Linq;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace GE.WebAdmin.Extantions.Repositories
{
    public static partial class RepositoryExtantions
    {
        public static VMBanner[] QueryForAdmin(this RepoBanner repo, Filter filter, bool? forGroup = null)
        {
            var query = QueryProvider.GetSelectString();
            query += " FROM D_BANNER AS db ";

            object param = null;
            query += getBannerWhereString(filter, out param, forGroup);

            query += QueryProvider.GetOrderString("db.DateCreate", SortDirection.Desc, filter.Orders);

            query += " OFFSET " + filter.PagerInfo.SkipCount + " ROWS FETCH NEXT " + filter.PagerInfo.PageSize + " ROWS ONLY";

            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                var data = conn.Query<VMBanner>(query, param: param);
                return data.ToArray();
            }
        }

        public static int FilterCount(this RepoBanner repo, Filter filter, bool? forGroup = null)
        {
            var query = @"SELECT COUNT(1) FROM D_BANNER AS db ";

            object param = null;
            query += getBannerWhereString(filter, out param, forGroup);

            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                var data = conn.Query<int>(query, param: param).Single();
                return data;
            }
        }

        private static string getBannerWhereString(Filter filter, out object param, bool? forGroup = null)
        {
            param = null;
            string query = null;
            query += " WHERE (db.Url LIKE '%'+@url+'%' OR @url IS NULL) ";
            query += " AND (db.Title LIKE '%'+@title+'%' OR @title IS NULL) ";
            if (forGroup.HasValue && filter.WhereExpressionObject != null && filter.WhereExpressionObject.BannerGroupId != null)
            {
                //for group banners
                if (forGroup == true)
                    query += " AND (db.Id IN (SELECT dbgl.BannerId FROM D_BANNER_GROUP_LINK dbgl WHERE dbgl.BannerGroupId=@bgid)) ";
                else if (forGroup == false)
                    query += " AND (db.Id NOT IN (SELECT dbgl.BannerId FROM D_BANNER_GROUP_LINK dbgl WHERE dbgl.BannerGroupId=@bgid)) ";
            }

            var title = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Title != null ? (string)filter.WhereExpressionObject.Title : null;
            var url = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Url != null ? (string)filter.WhereExpressionObject.Url : null;

            param = new
            {
                title = title,
                url = url,
                bgid = filter.WhereExpressionObject != null && filter.WhereExpressionObject.BannerGroupId != null ? (Guid)filter.WhereExpressionObject.BannerGroupId : (Guid?)null
            };

            return query;
        }
    }
}