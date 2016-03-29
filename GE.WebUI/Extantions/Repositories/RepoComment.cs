using GE.WebCoreExtantions.Repositories;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using GE.WebUI.Models;
using static SX.WebCore.Enums;

namespace GE.WebUI.Extantions.Repositories
{
    public static partial class RepositoryExtantions
    {
        public static VMComment[] GetComments(this RepoComment repo, int mid, ModelCoreType mct)
        {
            var query = @"SELECT dc.Id,
       CASE 
            WHEN anu.Id IS NULL THEN dc.UserName
            ELSE anu.NikName
       END                    AS UserName,
       dc.DateCreate,
       dc.Html
FROM   D_COMMENT              AS dc
       LEFT JOIN AspNetUsers  AS anu
            ON  anu.Id = dc.UserId
WHERE  dc.MaterialId = @mid
       AND dc.ModelCoreType = @mct
ORDER BY
       dc.DateCreate DESC";
            using (var connection = new SqlConnection(repo.ConnectionString))
            {
                var data = connection.Query<VMComment>(query, new { mid = mid, mct = mct });
                return data.ToArray();
            }
        }
    }
}