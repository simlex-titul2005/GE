using Dapper;
using SX.WebCore.Abstract;
using System.Data.SqlClient;
using System.Linq;

namespace SX.WebCore.Repositories
{
    public sealed class RepoProjectStep<TDbContext> : SxDbRepository<int, SxProjectStep, TDbContext> where TDbContext : SxDbContext
    {
        public override IQueryable<SxProjectStep> Query(SxFilter filter)
        {
            var query = @"WITH j(Id, [Level]) AS (
         SELECT dps.Id,
                1
         FROM   D_PROJECT_STEP AS dps
         WHERE  dps.ParentStepId IS NULL
         UNION ALL
         SELECT dps1.Id,
                j.[Level] + 1
         FROM   D_PROJECT_STEP  AS dps1
                JOIN j          AS j
                     ON  j.Id = dps1.ParentStepId
     )

SELECT dps.*,
       j.[Level]
FROM   j                    AS j
       JOIN D_PROJECT_STEP  AS dps
            ON  dps.Id = j.Id
ORDER BY
       dps.ParentStepId,
       dps.DateCreate DESC";

            using (var connection = new SqlConnection(ConnectionString))
            {
                var data = connection.Query<SxProjectStep>(query);
                return data.AsQueryable();
            }
        }

        public override int Count(SxFilter filter)
        {
            var query = @"SELECT COUNT(dps.Id)
FROM   D_PROJECT_STEP AS dps";

            using (var connection = new SqlConnection(ConnectionString))
            {
                var data = connection.Query<int>(query).Single();
                return data;
            }
        }

        public override void Delete(params object[] id)
        {
            var query = @"WITH j(Id) AS (
         SELECT dps.Id
         FROM   D_PROJECT_STEP AS dps
         WHERE  dps.Id = @id
         UNION ALL
         SELECT dps1.Id
         FROM   D_PROJECT_STEP  AS dps1
                JOIN j          AS j
                     ON  j.Id = dps1.ParentStepId
     )

DELETE 
FROM   D_PROJECT_STEP
WHERE  Id IN (SELECT j.Id
              FROM   j AS j)";

            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Execute(query, new { id = id[0] });
            }
        }
    }
}
