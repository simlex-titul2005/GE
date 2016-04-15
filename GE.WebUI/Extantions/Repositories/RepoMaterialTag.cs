using GE.WebCoreExtantions.Repositories;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using GE.WebCoreExtantions;
using SX.WebCore.ViewModels;

namespace GE.WebUI.Extantions.Repositories
{
    public static partial class RepositoryExtantions
    {
        public static SxVMMaterialTag[] GetCloud(this RepoMaterialTag repo, Filter filter, int amount=50)
        {
            var query = @"SELECT TOP(@amount) x.Title,
       SUM(x.[Count])  AS [Count],
       (CASE WHEN SUM(x.IsCurrent) >= 1 THEN 1 ELSE 0 END) AS IsCurrent
FROM   (
           SELECT dmt.Id          AS Title,
                  COUNT(1)        AS [Count],
                  (
                      CASE 
                           WHEN (dmt.MaterialId = @mid OR @mid IS NULL)
                      AND (dmt.ModelCoreType = @mct OR @mct IS NULL) THEN 1 ELSE 
                          0 END
                  )               AS IsCurrent
           FROM   D_MATERIAL_TAG  AS dmt  WHERE dmt.ModelCoreType=@mct
           GROUP BY
                  dmt.Id,
                  dmt.MaterialId,
                  dmt.ModelCoreType
       )                  x
GROUP BY
       x.Title
ORDER BY
       x.Title";

            using (var connection = new SqlConnection(repo.ConnectionString))
            {
                var data = connection.Query<SxVMMaterialTag>(query, new {
                    mid = filter.MaterialId,
                    mct = filter.ModelCoreType,
                    amount = amount
                });
                return data.ToArray();
            }
        }
    }
}