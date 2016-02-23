using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace GE.WebCoreExtantions.Repositories
{
    public sealed class RepoNews : SX.WebCore.Abstract.SxDbRepository<int, News, DbContext>
    {
        public override IQueryable<News> All
        {
            get
            {
                using (var conn = new SqlConnection(base.ConnectionString))
                {
                    var result = conn.Query<News>("select dvm.ID as Id, dvm.DATE_CREATE as DateCreate, dvm.TITLE as Title, dvm.HTML as Html, dvm.CORE_TYPE as CoreType, dvm.SHOW as Show, dvm.FRONT_PICTURE_ID as FrontPictureId, dn.GAME_ID as GameId from DV_MATERIAL dvm join D_NEWS dn on dn.ID=dvm.ID and dn.CORE_TYPE=dvm.CORE_TYPE order by dvm.DATE_CREATE desc");
                    return result.AsQueryable();
                }
            }
        }
    }
}
