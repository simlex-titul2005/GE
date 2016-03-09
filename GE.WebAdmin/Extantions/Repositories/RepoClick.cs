using GE.WebCoreExtantions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Dapper;
using GE.WebAdmin.Models;
using SX.WebCore.Abstract;

namespace GE.WebAdmin.Extantions.Repositories
{
    public static partial class RepositoryExtantions
    {
        public static IEnumerable<VMClick> QueryForAdmin(this GE.WebCoreExtantions.Repositories.RepoClick repo, SxFilter filter)
        {
            var f = (Filter)filter;
            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                var query = @"SELECT
	dc.Id, dc.UrlRef, dc.RawUrl, dc.LinkTarget, dct.Name AS ClickTypeName
FROM D_CLICK AS dc
JOIN D_CLICK_TYPE AS dct ON dct.Id = dc.ClickTypeId
ORDER BY dc.DateCreate";
                if (f != null && f.SkipCount.HasValue && f.PageSize.HasValue)
                    query += " OFFSET " + filter.SkipCount + " ROWS FETCH NEXT " + filter.PageSize + " ROWS ONLY";

                var data = conn.Query<VMClick>(query);

                return data.AsQueryable();
            }
        }
    }
}