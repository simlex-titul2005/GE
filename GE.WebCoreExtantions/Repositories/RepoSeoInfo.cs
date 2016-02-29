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
                    var seoInfo = conn.Query<SxSeoInfo>(@"SELECT*FROM D_SEO_INFO AS dsi ORDER BY dsi.RawUrl");
                    return seoInfo.AsQueryable();
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
                if(data!=null)
                {
                    data.Keywords = conn.Query<SxSeoKeyword>(@"SELECT*FROM D_SEO_KEYWORD AS dsk WHERE dsk.SeoInfoId=@SEO_INFO_ID", new { SEO_INFO_ID = data.Id }).ToArray();
                }

                return data;
            }
        }
    }
}
