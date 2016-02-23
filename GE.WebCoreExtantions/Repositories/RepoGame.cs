using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;

namespace GE.WebCoreExtantions.Repositories
{
    public sealed class RepoGame : SX.WebCore.Abstract.SxDbRepository<int, Game, DbContext>
    {
        public override IQueryable<Game> All
        {
            get
            {
                using (var conn = new SqlConnection(base.ConnectionString))
                {
                    var result = conn.Query<Game>("select dg.ID as Id, dg.TITLE as Title, dg.SHOW as Show, dg.[DESCRIPTION] as [Description], dg.FRONT_PICTURE_ID as FrontPictureId, dg.TITLE_ABBR as TitleAbbr, dg.GOOD_PICTURE_ID as GoodPictureId, dg.BAD_PICTURE_ID as BadPictureId from D_GAME dg order by dg.[TITLE]");
                    return result.AsQueryable();
                }
            }
        }
    }
}
