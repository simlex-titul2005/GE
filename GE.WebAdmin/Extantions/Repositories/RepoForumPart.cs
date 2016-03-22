using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using SX.WebCore.Abstract;
using GE.WebAdmin.Models;
using SX.WebCore.HtmlHelpers;
using GE.WebCoreExtantions.Repositories;
using GE.WebCoreExtantions;

namespace GE.WebAdmin.Extantions.Repositories
{
    public static partial class RepositoryExtantions
    {
        public static VMForumPart[] QueryForAdmin(this RepoForumPart repo, SxFilter filter, IDictionary<string, SxExtantions.SortDirection> order=null)
        {
            string title = null;
            string html = null;
            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                var query = @"SELECT dfp.Id,
       dfp.Title,
       dfp.Html
FROM   D_FORUM_PART dfp
WHERE (@TITLE IS NULL OR dfp.Title LIKE '%'+@TITLE+'%') AND (@HTML IS NULL OR dfp.Html LIKE '%'+@HTML+'%')
ORDER BY";

                //order
                if (order != null && order.ContainsKey("Title") && order["Title"] != SxExtantions.SortDirection.Unknown)
                    query += " Title " + order["Title"];
                else if (order != null && order.ContainsKey("Html") && order["Html"] != SxExtantions.SortDirection.Unknown)
                    query += " Html " + order["Html"];
                else
                    query += " Title";

                //filter
                if (filter != null && filter.Additional != null && filter.Additional[0] != null)
                    title = filter.Additional[0].ToString();
                if (filter != null && filter.Additional != null && filter.Additional[1] != null)
                    html = filter.Additional[1].ToString();

                if (filter != null && filter.SkipCount.HasValue && filter.PageSize.HasValue)
                    query += " OFFSET " + filter.SkipCount + " ROWS FETCH NEXT " + filter.PageSize + " ROWS ONLY";

                var data = conn.Query<VMForumPart>(query, new { @TITLE= title , @HTML=html });

                return data.ToArray();
            }
        }

        public static int FilterCount(this RepoForumPart repo, Filter filter)
        {
            string title = null;
            string html = null;
            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                var query = @"SELECT COUNT(1) FROM D_FORUM_PART AS dfp WHERE (@TITLE IS NULL OR dfp.Title LIKE '%'+@TITLE+'%') AND (@HTML IS NULL OR dfp.Html LIKE '%'+@HTML+'%')";
                
                //filter
                if (filter != null && filter.Additional != null && filter.Additional[0] != null)
                    title = filter.Additional[0].ToString();
                if (filter != null && filter.Additional != null && filter.Additional[1] != null)
                    html = filter.Additional[1].ToString();

                var data = conn.Query<int>(query, new { @TITLE = title, @HTML = html }).Single();
                return (int)data;
            }
        }
    }
}