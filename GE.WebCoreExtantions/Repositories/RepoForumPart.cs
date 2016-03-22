using System.Linq;
using SX.WebCore;
using SX.WebCore.Abstract;
using System.Data.SqlClient;
using Dapper;

namespace GE.WebCoreExtantions.Repositories
{
    public sealed class RepoForumPart : SxDbRepository<int, SxForumPart, DbContext>
    {
        public override IQueryable<SxForumPart> Query(SxFilter filter)
        {
            var query = @"SELECT dfp.Id,
       dfp.Title,
       dfp.Html
FROM   D_FORUM_PART dfp
ORDER BY
       dfp.Title";
                using (var conn = new SqlConnection(base.ConnectionString))
                {
                    var data = conn.Query<SxForumPart>(query);
                    return data.AsQueryable();
                }
        }
    }
}
