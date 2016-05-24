using SX.WebCore;
using SX.WebCore.Abstract;
using System;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace GE.WebCoreExtantions.Repositories
{
    public sealed class RepoRedirect : SxDbRepository<Guid, SxRedirect, DbContext>
    {
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
