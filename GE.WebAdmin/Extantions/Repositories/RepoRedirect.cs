using GE.WebCoreExtantions;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using SX.WebCore;
using GE.WebCoreExtantions.Repositories;
using System.Collections.Generic;
using SX.WebCore.HtmlHelpers;

namespace GE.WebAdmin.Extantions.Repositories
{
    public static partial class RepositoryExtantions
    {
        public static IQueryable<SxRedirect> QueryForAdmin(this RepoRedirect repo, Filter filter, IDictionary<string, SxExtantions.SortDirection> order = null)
        {
            var f = (Filter)filter;
            string oldUrl = null;
            string newUrl = null;
            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                var query = @"SELECT dr.Id,
       dr.OldUrl,
       dr.NewUrl,
       dr.DateCreate
FROM   D_REDIRECT dr
WHERE  (dr.OldUrl LIKE '%' + @old_url + '%' OR @old_url IS NULL)
       AND (dr.NewUrl = '%' + @new_url + '%' OR @new_url IS NULL)
ORDER BY ";

                //where
                if (filter != null && filter.Additional != null && filter.Additional[0] != null)
                    oldUrl = filter.Additional[0].ToString();
                if (filter != null && filter.Additional != null && filter.Additional[1] != null)
                    newUrl = filter.Additional[1].ToString();

                if (f != null && f.SkipCount.HasValue && f.PageSize.HasValue)
                    query += " OFFSET " + filter.SkipCount + " ROWS FETCH NEXT " + filter.PageSize + " ROWS ONLY";

                var data = conn.Query<SxRedirect>(query, new { old_url = oldUrl, new_url = newUrl });

                return data.AsQueryable();
            }
        }

        public static int FilterCount(this RepoRedirect repo, Filter filter)
        {
            string oldUrl = null;
            string newUrl = null;
            using (var conn = new SqlConnection(repo.ConnectionString))
            {
                var query = @"SELECT COUNT(1)
FROM   D_REDIRECT dr
WHERE  (dr.OldUrl LIKE '%' + @old_url + '%' OR @old_url IS NULL)
       AND (dr.NewUrl = '%' + @new_url + '%' OR @new_url IS NULL)";

                //where
                if (filter != null && filter.Additional != null && filter.Additional[0] != null)
                    oldUrl = filter.Additional[0].ToString();
                if (filter != null && filter.Additional != null && filter.Additional[1] != null)
                    newUrl = filter.Additional[1].ToString();

                var data = conn.Query<int>(query, new { old_url = oldUrl, new_url = newUrl }).Single();
                return (int)data;
            }
        }
    }
}