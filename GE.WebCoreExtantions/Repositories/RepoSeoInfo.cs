using System.Data.SqlClient;
using System.Linq;
using Dapper;
using SX.WebCore.Abstract;
using SX.WebCore;
using static SX.WebCore.Enums;

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

        public override int Count(SxFilter filter)
        {
            var f = (Filter)filter;
            using (var conn = new SqlConnection(base.ConnectionString))
            {
                var query = @"SELECT COUNT(1) FROM D_SEO_INFO";
                var data = conn.Query<int>(query).Single();
                return (int)data;
            }
        }

        /// <summary>
        /// Теги для url
        /// </summary>
        /// <param name="rawUrl"></param>
        /// <returns></returns>
        public SxSeoInfo GetSeoInfo(string rawUrl)
        {
            using (var conn = new SqlConnection(base.ConnectionString))
            {
                var query = @"SELECT*FROM D_SEO_INFO AS dsi";
                query += " WHERE dsi.RawUrl = @raw_url";

                var data = conn.Query<SxSeoInfo>(query, new { raw_url = rawUrl}).SingleOrDefault();
                if(data!=null)
                {
                    data.Keywords = conn.Query<SxSeoKeyword>(@"SELECT*FROM D_SEO_KEYWORD AS dsk WHERE dsk.SeoInfoId=@siid", new { siid = data.Id }).ToArray();
                }

                return data;
            }
        }

        /// <summary>
        /// Теги для материала
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="mct"></param>
        /// <returns></returns>
        public SxSeoInfo GetSeoInfo(int mid, ModelCoreType mct)
        {
            using (var conn = new SqlConnection(base.ConnectionString))
            {
                var query = @"SELECT*FROM D_SEO_INFO AS dsi";
                query += " WHERE (dsi.MaterialId = @mid AND dsi.ModelCoreType = @mct)";

                var data = conn.Query<SxSeoInfo>(query, new { mid = mid, mct = mct }).SingleOrDefault();
                if (data != null)
                {
                    data.Keywords = conn.Query<SxSeoKeyword>(@"SELECT*FROM D_SEO_KEYWORD AS dsk WHERE dsk.SeoInfoId=@siid", new { siid = data.Id }).ToArray();
                }

                return data;
            }
        }
    }
}
