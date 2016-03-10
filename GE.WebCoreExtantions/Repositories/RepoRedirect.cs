using SX.WebCore;
using SX.WebCore.Abstract;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace GE.WebCoreExtantions.Repositories
{
    public sealed class RepoRedirect : SxDbRepository<Guid, SxRedirect, DbContext>
    {
        public IQueryable<SxRedirect> QueryForAdmin(SxFilter filter)
        {
            var f = (Filter)filter;
            using (var conn = new SqlConnection(base.ConnectionString))
            {
                var query = @"select dr.Id, dr.OldUrl, dr.NewUrl, dr.DateCreate from D_REDIRECT dr
order by dr.DateCreate desc";
                if (f != null && f.SkipCount.HasValue && f.PageSize.HasValue)
                    query += " OFFSET " + filter.SkipCount + " ROWS FETCH NEXT " + filter.PageSize + " ROWS ONLY";

                var data = conn.Query<SxRedirect>(query);

                return data.AsQueryable();
            }
        }

        public override int Count(SxFilter filter)
        {
            var f = (Filter)filter;
            using (var conn = new SqlConnection(base.ConnectionString))
            {
                var query = @"SELECT COUNT(1) FROM D_REDIRECT AS dr";
                var data = conn.Query<int>(query).Single();
                return (int)data;
            }
        }

        public SxRedirect GetRedirectUrl(string rawUrl)
        {
            using (var conn = new SqlConnection(base.ConnectionString))
            {
                var query = @"SELECT dr.NewUrl FROM D_REDIRECT AS dr where dr.OldUrl=@OLD_URL";
                var data = conn.Query<SxRedirect>(query, new { OLD_URL = rawUrl }).SingleOrDefault();
                return data;
            }
        }
    }
}
