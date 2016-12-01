using Dapper;
using GE.WebUI.Models;
using GE.WebUI.ViewModels;
using SX.WebCore.SxRepositories.Abstract;
using System.Data.SqlClient;
using System.Linq;
using System;
using System.Threading.Tasks;

namespace GE.WebUI.Infrastructure.Repositories
{
    public sealed class RepoSiteTestSetting : SxDbRepository<int, SiteTestSetting, VMSiteTestSetting>
    {
        public sealed override SiteTestSetting Create(SiteTestSetting model)
        {
            throw new NotSupportedException("Добавление настроек теста не предусмотрено");
        }

        public sealed override SiteTestSetting Update(SiteTestSetting model)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var data = connection.Query<SiteTestSetting>("dbo.save_site_test_setting @testId, @lc, @oetbc, @bfos, @dqs, @dsab", new {
                    testId = model.TestId,
                    lc = model.LettersInSecond,
                    oetbc =model.OnEndTimeBalsCount,
                    bfos=model.BalsForOneSecond,
                    dqs=model.DefQuestionSeconds,
                    dsab=model.DefCorrectAnswerBals
                });
                return data.SingleOrDefault();
            }
        }
        public sealed override async Task<SiteTestSetting> UpdateAsync(SiteTestSetting model)
        {
            return await Task.Run(() => {
                return Update(model);
            });
        }

        public sealed override void Delete(SiteTestSetting model)
        {
            throw new NotSupportedException("Удаление настроек теста не предусмотрено");
        }

        public sealed override SiteTestSetting GetByKey(params object[] id)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var data = connection.Query<SiteTestSetting>("dbo.get_site_test_setting @testId", new { testId = id[0] });
                return data.SingleOrDefault();
            }
        }
    }
}