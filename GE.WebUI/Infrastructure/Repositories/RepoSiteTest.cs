using Dapper;
using GE.WebUI.Models;
using GE.WebUI.ViewModels;
using SX.WebCore;
using SX.WebCore.Providers;
using SX.WebCore.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace GE.WebUI.Infrastructure.Repositories
{
    public sealed class RepoSiteTest : SxDbRepository<int, SiteTest, VMSiteTest>
    {
        public override VMSiteTest[] Read(SxFilter filter)
        {
            var sb = new StringBuilder();
            sb.Append(SxQueryProvider.GetSelectString());
            sb.Append(" FROM D_SITE_TEST AS dst ");

            object param = null;
            var gws = getSiteTestWhereString(filter, out param);
            sb.Append(gws);

            var defaultOrder = new SxOrder { FieldName = "dst.DateCreate", Direction = SortDirection.Desc };
            sb.Append(SxQueryProvider.GetOrderString(defaultOrder, filter.Order));

            sb.AppendFormat(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", filter.PagerInfo.SkipCount, filter.PagerInfo.PageSize);

            //count
            var sbCount = new StringBuilder();
            sbCount.Append("SELECT COUNT(1) FROM D_SITE_TEST AS dst ");
            sbCount.Append(gws);

            using (var connection = new SqlConnection(ConnectionString))
            {
                var data = connection.Query<VMSiteTest>(sb.ToString(), param: param);
                filter.PagerInfo.TotalItems = connection.Query<int>(sbCount.ToString(), param: param).SingleOrDefault();
                return data.ToArray();
            }
        }

        private static string getSiteTestWhereString(SxFilter filter, out object param)
        {
            param = null;
            var query = new StringBuilder();
            query.Append(" WHERE (dst.Title LIKE '%'+@title+'%' OR @title IS NULL) ");
            query.Append(" AND (dst.Description LIKE '%'+@desc+'%' OR @desc IS NULL) ");
            query.Append(" AND (dst.Show=@show OR @show IS NULL) ");

            var title = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Title != null ? (string)filter.WhereExpressionObject.Title : null;
            var desc = filter.WhereExpressionObject != null && filter.WhereExpressionObject.Description != null ? (string)filter.WhereExpressionObject.Description : null;

            param = new
            {
                title = title,
                desc = desc,
                show = filter.OnlyShow
            };

            return query.ToString();
        }

        public dynamic[] RandomList(int amount = 3)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {

                var data = conn.Query("dbo.get_random_site_tests @amount", new { amount = amount });
                return data.ToArray();
            }
        }

        public SiteTestAnswer GetSiteTestPage(string titleUrl)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SiteTestAnswer, SiteTestQuestion, SiteTestSubject, SiteTest, SiteTestAnswer>("dbo.get_site_test_page @titleUrl", (a, q, s, t) =>
                {
                    a.Question = q;
                    q.Test = t;
                    a.Subject = s;
                    return a;
                }, new { titleUrl = titleUrl }, splitOn: "Id").SingleOrDefault();

                if (data == null)
                    return null;

                if (Equals(data.Question.Test.Type, SiteTest.SiteTestType.Normal) || Equals(data.Question.Test.Type, SiteTest.SiteTestType.NormalImage))
                {
                    data.Question.Test.Questions = conn.Query<SiteTestQuestion>("dbo.get_site_test_normal_questions @testId, @subjectId, @amount", new { testId = data.Question.TestId, subjectId = data.SubjectId, amount = 3 }).ToArray();
                }

                return data;
            }
        }

        public override void Delete(SiteTest model)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Execute("dbo.del_site_test @testId", new { testId = model.Id });
            }
        }

        public DataTable GetMatrix(int testId, out int count, int page = 1, int pageSize = 10)
        {
            var result = new DataTable();
            using (var conn = new SqlConnection(ConnectionString))
            {
                using (SqlDataAdapter adp = new SqlDataAdapter("dbo.get_site_test_matrix @testId, @page, @pageSize, @count OUTPUT", conn))
                {
                    adp.SelectCommand.Parameters.AddWithValue("testId", testId);
                    adp.SelectCommand.Parameters.AddWithValue("page", page);
                    adp.SelectCommand.Parameters.AddWithValue("pageSize", pageSize);

                    var par = new SqlParameter { ParameterName = "count", DbType = DbType.Int32, Direction = ParameterDirection.Output };
                    adp.SelectCommand.Parameters.Add(par);

                    adp.Fill(result);

                    count = Convert.ToInt32(par.Value);
                }
            }
            return result;
        }

        public void RevertMatrixValue(string subjectTitle, string questionText, int value)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Execute("dbo.revert_site_test_matrix_value @subjectTitle, @questionText, @value", new
                {
                    subjectTitle = subjectTitle,
                    questionText = questionText,
                    value = value == 0 ? 1 : 0
                });
            }
        }

        public override SiteTest Create(SiteTest model)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SiteTest>("dbo.add_site_test @title, @desc, @rules, @titleUrl, @type, @show", new
                {
                    title = model.Title,
                    desc = model.Description,
                    rules = model.Rules,
                    titleUrl = UrlHelperExtensions.SeoFriendlyUrl(model.Title),
                    type = model.Type,
                    show = model.Show
                }).SingleOrDefault();

                return data;
            }
        }

        public SiteTestAnswer GetGuessStep(List<VMSiteTestStepGuess> steps, out int subjectsCount)
        {
            var table = new DataTable();
            table.Columns.Add(new DataColumn { ColumnName = "QuestionId" });
            table.Columns.Add(new DataColumn { ColumnName = "IsCorrect" });
            table.Columns.Add(new DataColumn { ColumnName = "Order" });
            steps.ForEach(x =>
            {
                table.Rows.Add(x.QuestionId, x.IsCorrect, x.Order);
            });

            var p = new DynamicParameters();
            p.Add("oldSteps", table.AsTableValuedParameter("dbo.OldSiteTestStepGuess"));
            p.Add("subjectsCount", dbType: DbType.Int32, direction: ParameterDirection.Output);

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SiteTestAnswer, SiteTestQuestion, SiteTestSubject, SiteTest, SiteTestAnswer>("dbo.get_site_test_next_guess_step", (a, q, s, t) =>
                {
                    a.Question = q;
                    q.Test = t;
                    a.Subject = s;
                    return a;
                }, p, commandType: CommandType.StoredProcedure).SingleOrDefault();

                subjectsCount = p.Get<int>("subjectsCount");

                return data;
            }
        }

        public SiteTestAnswer GetNormalStep(List<VMSiteTestStepNormal> steps, out int subjectsCount, out int allSubjectsCount)
        {
            var table = new DataTable();
            table.Columns.Add(new DataColumn { ColumnName = "QuestionId" });
            table.Columns.Add(new DataColumn { ColumnName = "SubjectId" });
            steps.ForEach(x =>
            {
                table.Rows.Add(x.QuestionId, x.SubjectId);
            });

            var p = new DynamicParameters();
            p.Add("oldSteps", table.AsTableValuedParameter("dbo.OldSiteTestStepNormal"));
            p.Add("subjectsCount", dbType: DbType.Int32, direction: ParameterDirection.Output);
            p.Add("allSubjectsCount", dbType: DbType.Int32, direction: ParameterDirection.Output);

            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SiteTestAnswer, SiteTestQuestion, SiteTestSubject, SiteTest, SiteTestAnswer>("dbo.get_site_test_next_normal_step", (a, q, s, t) =>
                {
                    a.Question = q;
                    q.Test = t;
                    a.Subject = s;
                    return a;
                }, p, commandType: CommandType.StoredProcedure).SingleOrDefault();

                data.Question.Test.Questions = conn.Query<SiteTestQuestion>("dbo.get_site_test_normal_questions @testId, @subjectId, @amount", new { testId = data.Question.TestId, subjectId = data.SubjectId, amount = 3 }).ToArray();

                subjectsCount = p.Get<int>("subjectsCount");
                allSubjectsCount = p.Get<int>("allSubjectsCount");

                return data;
            }
        }
        public SiteTestAnswer[] GetNormalResults(int subjectId)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                var data = conn.Query<SiteTestAnswer, SiteTestQuestion, SiteTestSubject, SiteTest, SiteTestAnswer>("dbo.get_site_test_normal_results @subjectId", (a, q, s, t) =>
                {
                    a.Question = q;
                    q.Test = t;
                    a.Subject = s;
                    return a;
                }, new { subjectId = subjectId });

                return data.ToArray();
            }
        }

        public async Task<SiteTest> GetSiteTestRulesAsync(int siteTestId)
        {
            return await Task.Run(() =>
            {
                using (var conn = new SqlConnection(ConnectionString))
                {
                    var data = conn.Query<SiteTest>("dbo.get_site_test_rules @testId", new { testId = siteTestId }).SingleOrDefault();
                    return data;
                }
            });
        }

        public async Task<int> AddShow(int testId)
        {
            return await Task.Run(() =>
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    var data = connection.Query<int>("dbo.add_site_test_show @testId", new { testId = testId });
                    return data.SingleOrDefault();
                }
            });
        }
    }
}