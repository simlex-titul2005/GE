using SX.WebCore;
using SX.WebCore.Abstract;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Dapper;

namespace GE.WebCoreExtantions.Repositories
{
    public sealed class RepoPicture : SxDbRepository<Guid, SxPicture, DbContext>
    {
        public override IQueryable<SxPicture> All
        {
            get
            {
                using (var conn = new SqlConnection(base.ConnectionString))
                {
                    var result = conn.Query<SxPicture>(@"select dp.ID, dp.CAPTION, dp.[DESCRIPTION], dp.WIDTH, dp.HEIGHT, dp.ImgFormat from D_PICTURE dp order by dp.DateCreate desc");
                    return result.AsQueryable();
                }
            }
        }

        public override SxPicture GetByKey(params object[] id)
        {
            using (var conn = new SqlConnection(base.ConnectionString))
            {
                var picture = conn.Query<SxPicture>("select dp.OriginalContent, dp.WIDTH as Width, dp.HEIGHT as Height, dp.ImgFormat from D_PICTURE dp where dp.ID=@PICTURE_ID", new { PICTURE_ID = id }).SingleOrDefault();
                return picture;
            }
        }
    }
}
