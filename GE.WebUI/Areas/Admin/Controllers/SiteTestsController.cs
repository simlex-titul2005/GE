using GE.WebUI.Controllers;
using GE.WebUI.Infrastructure.Repositories;
using GE.WebUI.Models;
using GE.WebUI.ViewModels;
using OfficeOpenXml;
using SX.WebCore;
using SX.WebCore.Attrubutes;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace GE.WebUI.Areas.Admin.Controllers
{
    public sealed class SiteTestsController : BaseController
    {
        //admin
        private static RepoSiteTest _repo = new RepoSiteTest();
        public static RepoSiteTest Repo
        {
            get { return _repo; }
            set { _repo = value; }
        }

        private static int _pageSize = 20;

        [HttpGet]
        public ActionResult Index(int page = 1)
        {
            var order = new SxOrderItem { FieldName = "Title", Direction = SortDirection.Asc };
            var filter = new SxFilter(page, _pageSize) { Order = order };

            var viewModel = _repo.Read(filter);
            if (page > 1 && !viewModel.Any())
                return new HttpNotFoundResult();

            ViewBag.Filter = filter;

            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Index(VMSiteTest filterModel, SxOrderItem order, int page = 1)
        {
            var filter = new SxFilter(page, _pageSize) { Order = order != null && order.Direction != SortDirection.Unknown ? order : null, WhereExpressionObject = filterModel };

            var viewModel = await _repo.ReadAsync(filter);
            if (page > 1 && !viewModel.Any())
                return new HttpNotFoundResult();

            ViewBag.Filter = filter;

            return PartialView("_GridView", viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> FindGridView(VMSiteTest filterModel, SxOrderItem order, int page = 1, int pageSize = 10)
        {
            var filter = new SxFilter(page, pageSize) { Order = order != null && order.Direction != SortDirection.Unknown ? order : null, WhereExpressionObject = filterModel };

            var viewModel = await _repo.ReadAsync(filter);
            if (page > 1 && !viewModel.Any())
                return new HttpNotFoundResult();

            ViewBag.Filter = filter;

            return PartialView("_FindGridView", viewModel);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int? id)
        {
            var data = id.HasValue ? await Repo.GetByKeyAsync(id) : new SiteTest();
            if (id.HasValue && data == null) return new HttpNotFoundResult();
            var viewModel = new VMSiteTest() {
                DateCreate=data.DateCreate,
                Description=data.Description,
                Id=data.Id,
                Rules=data.Rules,
                Show=data.Show,
                ShowSubjectDesc=data.ShowSubjectDesc,
                Title=data.Title,
                TitleUrl=data.TitleUrl,
                Type=data.Type,
                ViewsCount=data.ViewsCount,
                ViewOnMainPage=data.ViewOnMainPage
            };
            return View(viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(VMSiteTest model)
        {
            var isArchitect = User.IsInRole("architect");
            var isNew = model.Id == 0;

            if (isNew || (!isNew && string.IsNullOrEmpty(model.TitleUrl)))
                model.TitleUrl = Url.SeoFriendlyUrl(model.Title);

            var redactModel = Mapper.Map<VMSiteTest, SiteTest>(model);
            if (ModelState.IsValid)
            {
                SiteTest newModel = null;
                if (isNew)
                    newModel = _repo.Create(redactModel);
                else
                {
                    if (isArchitect)
                        newModel = _repo.Update(redactModel, true, "Title", "Description", "Rules", "Type", "TitleUrl", "Show", "ShowSubjectDesc", "ViewOnMainPage");
                    else
                        newModel = _repo.Update(redactModel, true, "Title", "Description", "Rules", "Show", "ShowSubjectDesc", "ViewOnMainPage");
                }
                return RedirectToAction("Index");
            }
            else
                return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(SiteTest model)
        {
            if (await _repo.GetByKeyAsync(model.Id) == null)
                return new HttpNotFoundResult();

            await _repo.DeleteAsync(model);
            return RedirectToAction("Index");
        }

        private readonly int _matrixPageSize = 7;
        [HttpGet]
        public async Task<PartialViewResult> TestMatrix(int testId, int page = 1)
        {
            return await Task.Run(() =>
            {
                var count = 0;
                var data = _repo.GetMatrix(testId, out count, page, _matrixPageSize);
                ViewBag.Count = count;
                ViewBag.PageSize = _matrixPageSize;
                ViewBag.TestId = testId;
                ViewBag.Page = page;
                return PartialView("_Matrix", data);
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult RevertMatrixValue(string subjectTitle, string questionText, int value)
        {
            Task.Run(() =>
            {
                _repo.RevertMatrixValue(subjectTitle, questionText, value);
            });
            return Json(value == 0 ? 1 : 0);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<RedirectToRouteResult> LoadFromFile(HttpPostedFileBase file)
        {
            var test = new SiteTestModel();
            var questions = new List<string>();
            var subjects = new List<string>();

            using (var excel = new ExcelPackage(file.InputStream))
            {
                var ws = excel.Workbook.Worksheets.First();

                var range = ws.Cells["A1"];
                test.Title = range.Value.ToString().Trim();

                range = ws.Cells["B1"];
                test.Desc = range.Value?.ToString().Trim();

                //questions
                range = ws.Cells["C1"];
                var startRow = range.Start.Row;
                var startColumn = range.Start.Column;
                var count = 0;
                while (range.Value != null)
                {
                    count++;
                    questions.Add(range.Value.ToString().Trim());
                    range = ws.Cells[startRow, startColumn + count];
                }
                test.Questions = questions.ToArray();
                count = 0;

                range = ws.Cells["A2"];
                startRow = range.Start.Row;
                startColumn = range.Start.Column;
                while (range.Value != null)
                {
                    count++;
                    subjects.Add(range.Value.ToString().Trim());
                    range = ws.Cells[startRow + count, startColumn];
                }
                test.Subjects = subjects.ToArray();

                range = ws.Cells["A2"];
                TestAnswer answer;
                string subjectTitle;
                string subjectDesc;
                for (int i = 0; i < test.Subjects.Length; i++)
                {
                    subjectTitle = range.Value.ToString().Trim();

                    startColumn++;
                    range = ws.Cells[startRow, startColumn];
                    subjectDesc = range.Value?.ToString().Trim();

                    for (int y = 0; y < test.Questions.Length; y++)
                    {
                        startColumn++;
                        range = ws.Cells[startRow, startColumn];
                        answer = new TestAnswer
                        {
                            SubjectTitle = subjectTitle,
                            SubjectDesc = subjectDesc,
                            Question = test.Questions[y],
                            IsCorrect = range.Value != null && range.Value.ToString() == "1" ? true : false
                        };
                        test.Answers.Add(answer);
                    }
                    startRow++;
                    startColumn = 1;
                    range = ws.Cells[startRow, startColumn];
                }

                var id = await writeSiteTestToDb(test);
                return RedirectToAction("edit", new { id = id });
            }
        }
        private class SiteTestModel
        {
            public SiteTestModel()
            {
                Questions = new string[0];
                Subjects = new string[0];
                Answers = new List<TestAnswer>();
            }
            public string Title { get; set; }
            public string Desc { get; set; }
            public string[] Questions { get; set; }
            public string[] Subjects { get; set; }
            public List<TestAnswer> Answers { get; set; }
        }
        private struct TestAnswer
        {
            public string SubjectTitle { get; set; }
            public string SubjectDesc { get; set; }
            public string Question { get; set; }
            public bool IsCorrect { get; set; }
        }
        private async Task<int> writeSiteTestToDb(SiteTestModel test)
        {
            return await Task.Run(() =>
            {
                var testId = Repo.Create(new SiteTest { Title = test.Title, Description = test.Desc }).Id;
                TestAnswer answer;
                string sTitle;
                string q;
                SiteTestSubject subject;
                SiteTestQuestion question;
                for (int i = 0; i < test.Subjects.Length; i++)
                {
                    sTitle = test.Subjects[i];
                    subject = SiteTestSubjectsController.Repo.Create(new SiteTestSubject { Title = sTitle, TestId = testId });
                    for (int y = 0; y < test.Questions.Length; y++)
                    {
                        q = test.Questions[y];
                        answer = test.Answers.Where(x => x.SubjectTitle == sTitle && x.Question == q).SingleOrDefault();
                        question = SiteTestQuestionsController.Repo.Create(new SiteTestQuestion { Text = q, TestId = testId });
                        if (answer.IsCorrect)
                            Repo.RevertMatrixValue(sTitle, q, 0);
                    }
                }

                return testId;
            });
        }

        [HttpPost]
        public async Task<JsonResult> Rules(int testId)
        {
            var data = await _repo.GetSiteTestRulesAsync(testId);
            return Json(new
            {
                Title = data.Title,
                Rules = data.Rules
            });
        }
    }
}