using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using SX.WebCore.Abstract;
using SX.WebCore;

namespace GE.WebCoreExtantions.Repositories
{
    public sealed class RepoSeoInfo : SxDbRepository<int, SxSeoInfo, DbContext>
    {
        public override IQueryable<SxSeoInfo> All
        {
            get
            {
                using (var conn = new SqlConnection(base.ConnectionString))
                {
                    var query = @"SELECT 
	dsi.*
FROM D_SEO_INFO AS dsi
ORDER BY dsi.RawUrl";
                    var data = conn.Query<SxSeoInfo>(query);

                    return data.AsQueryable();
                }
            }
        }

        public SxSeoInfo GetByRawUrl(string rawUrl)
        {
            using (var conn = new SqlConnection(base.ConnectionString))
            {
                var query = @"SELECT*FROM D_SEO_INFO AS dsi
WHERE dsi.RawUrl=@RAW_URL";
                var data = conn.Query<SxSeoInfo>(query, new { RAW_URL = rawUrl }).FirstOrDefault();

                return data;
            }
        }
    }
}
