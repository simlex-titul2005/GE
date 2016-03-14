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

namespace GE.WebAdmin.Extantions.Repositories
{
    public static partial class RepositoryExtantions
    {
        public static IQueryable<VMSeoInfo> QueryForAdmin(this GE.WebCoreExtantions.Repositories.RepoSeoInfo repo, SxFilter filter)
        {
            var f = (Filter)filter;
            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                var query = @"SELECT
	dsi.Id,
	dsi.RawUrl
FROM D_SEO_INFO AS dsi
ORDER BY dsi.RawUrl";
                if (f != null && f.SkipCount.HasValue && f.PageSize.HasValue)
                    query += " OFFSET " + filter.SkipCount + " ROWS FETCH NEXT " + filter.PageSize + " ROWS ONLY";

                var data = conn.Query<VMSeoInfo>(query);

                return data.AsQueryable();
            }
        }

        //найти страницу по rawUrl
        public static SxSeoInfo GetByRawUrl(this GE.WebCoreExtantions.Repositories.RepoSeoInfo repo, string rawUrl)
        {
            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                var query = @"SELECT dsi.Id FROM D_SEO_INFO AS dsi WHERE dsi.RawUrl=@RAW_URL";
                var data = conn.Query<SxSeoInfo>(query, new { @RAW_URL = rawUrl });
                return data.SingleOrDefault();
            }
        }
    }
}