using SX.WebCore.Abstract;
using System.Data.SqlClient;
using Dapper;

namespace GE.WebCoreExtantions.Repositories
{
    public sealed class RepoNewsRubric : SxDbRepository<string, NewsRubric, DbContext>
    {
        public override void Delete(params object[] id)
        {
            var query = @"BEGIN TRANSACTION
UPDATE D_NEWS
SET    RubricId = NULL
WHERE  RubricId = @id
DELETE 
FROM   D_NEWS_RUBRIC
WHERE  Id = @id
COMMIT TRANSACTION";

            using (var connectuon = new SqlConnection(base.ConnectionString))
            {
                connectuon.Execute(query, new { id = id[0] });
            }
        }
    }
}
