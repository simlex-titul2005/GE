using Dapper;
using SX.WebCore.Abstract;
using SX.WebCore.ViewModels;
using System;
using System.Data.SqlClient;
using System.Linq;

namespace SX.WebCore.Repositories
{
    public sealed class RepoSiteTestResult<TDbContext> : SxDbRepository<Guid, SxSiteTestResult, TDbContext> where TDbContext : SxDbContext
    {
        public SxSiteTestResult[] GetByKey(Guid id)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SxSiteTestResult, SxSiteTestQuestion, SxSiteTestBlock, SxSiteTest, SxSiteTestResult>("get_site_test_result @id", (r, q, b, t) =>
                {
                    b.Test = t;
                    q.Block = b;
                    r.Question = q;
                    return r;
                }, new { id = id }).ToArray();

                return data;
            }
        }

        public override SxSiteTestResult Create(SxSiteTestResult model)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Execute("add_site_test_result @id, @questionId, @result, @dateAnswer", new
                {
                    id = model.Id,
                    questionId = model.QuestionId,
                    result = model.Result,
                    dateAnswer = model.DateAnswer
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
