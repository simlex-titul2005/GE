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
        public override SxPicture GetByKey(params object[] id)
        {
            using (var conn = new SqlConnection(base.ConnectionString))
            {
                var picture = conn.Query<SxPicture>("select dp.Id, dp.Caption, dp.Description, dp.OriginalContent, dp.Width, dp.Height, dp.ImgFormat, dp.DateCreate from D_PICTURE dp where dp.ID=@PICTURE_ID", new { PICTURE_ID = id }).SingleOrDefault();
                return picture;
            }
        }
    }
}
