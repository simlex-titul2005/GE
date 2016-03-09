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
    public sealed class RepoRequest : SxDbRepository<Guid, SxRequest, DbContext>
    {
        public IQueryable<SxRequest> QueryForAdmin(SxFilter filter)
        {
            var f = (Filter)filter;
            using (var conn = new SqlConnection(base.ConnectionString))
            {
                var query = @"select dr.SessionId, dr.UrlRef, dr.Browser, dr.ClientIP, dr.UserAgent, dr.RequestType, dr.DateCreate, dr.RawUrl from D_REQUEST dr
order by dr.DateCreate desc";
                if (f != null && f.SkipCount.HasValue && f.PageSize.HasValue)
                    query += " OFFSET " + filter.SkipCount + " ROWS FETCH NEXT " + filter.PageSize + " ROWS ONLY";

                var data = conn.Query<SxRequest>(query);

                return data.AsQueryable();
            }
        }

        public override int Count(SxFilter filter)
        {
            var f = (Filter)filter;
            using (var conn = new SqlConnection(base.ConnectionString))
            {
                var query = @"SELECT COUNT(1) FROM D_REQUEST AS dr";
                var data = conn.Query<int>(query).Single();
                return (int)data;
            }
        }
    }
}
