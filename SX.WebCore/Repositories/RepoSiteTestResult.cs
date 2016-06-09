using Dapper;
using SX.WebCore.Abstract;
using System;
using System.Data.SqlClient;

namespace SX.WebCore.Repositories
{
    public sealed class RepoSiteTestResult<TDbContext> : SxDbRepository<Guid, SxSiteTestResult, TDbContext> where TDbContext : SxDbContext
    {
        public override SxSiteTestResult Create(SxSiteTestResult model)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Execute("dbo.add_site_test_result @id, @testId, @blockId, @questionId, @result", new {
                    id= model.Id,
                    testId=model.TestId,
                    blockId=model.BlockId,
                    questionId=model.QuestionId,
                    result=model.Result
                });
            }

            return null;
        }

        public Guid Create(SxSiteTestResult[] models)
        {
            var id = Guid.NewGuid();
            SxSiteTestResult model;
            for (int i = 0; i < models.Length; i++)
            {
                model = models[i];
                model.Id = id;
                Create(models[i]);
            }
            return id;
        }
    }
}
