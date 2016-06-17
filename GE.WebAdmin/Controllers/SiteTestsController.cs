using GE.WebAdmin.Models;
using GE.WebCoreExtantions;
using SX.WebCore;
using SX.WebCore.Abstract;
using SX.WebCore.HtmlHelpers;
using SX.WebCore.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GE.WebAdmin.Controllers
{
    [Authorize(Roles = "admin")]
    public partial class SiteTestsController : BaseController
    {
        private SxDbRepository<int, SxSiteTest, DbContext> _repo;
        public SiteTestsController()
        {
            _repo = new SxRepoSiteTest<DbContext>();
        }

        private static int _pageSize = 20;

        [HttpGet]
        public virtual ViewResult Index(int page = 1)
        {
            var filter = new WebCoreExtantions.Filter(page, _pageSize);
            var totalItems = (_repo as SxRepoSiteTest<DbContext>).Count(filter);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.PagerInfo = filter.PagerInfo;

            var viewModel = (_repo as SxRepoSiteTest<DbContext>).Query(filter).ToArray()
                .Select(x => Mapper.Map<SxSiteTest, VMSiteTest>(x)).ToArray();
            return View(viewModel);
        }

        [HttpPost]
        public virtual PartialViewResult Index(VMSiteTest filterModel, IDictionary<string, SxExtantions.SortDirection> order, int page = 1)
        {
            ViewBag.Filter = filterModel;
            ViewBag.Order = order;

            var filter = new WebCoreExtantions.Filter(page, _pageSize) { Orders = order, WhereExpressionObject = filterModel };
            var totalItems = (_repo as SxRepoSiteTest<DbContext>).Count(filter);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.PagerInfo = filter.PagerInfo;

            var viewModel = (_repo as SxRepoSiteTest<DbContext>).Query(filter).ToArray()
                .Select(x => Mapper.Map<SxSiteTest, VMSiteTest>(x)).ToArray(); ;

            return PartialView("_GridView", viewModel);
        }

        [HttpPost]
        public virtual PartialViewResult FindGridView(VMSiteTest filterModel, int page = 1, int pageSize = 10)
        {
            ViewBag.Filter = filterModel;
            var filter = new WebCoreExtantions.Filter(page, pageSize);
            filter.WhereExpressionObject = filterModel;
            var totalItems = (_repo as SxRepoSiteTest<DbContext>).Count(filter);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.PagerInfo = filter.PagerInfo;

            var viewModel = (_repo as SxRepoSiteTest<DbContext>).Query(filter).ToArray()
                .Select(x => Mapper.Map<SxSiteTest, VMSiteTest>(x)).ToArray();

            return PartialView("_FindGridView", viewModel);
        }

        [HttpGet]
        public virtual ViewResult Edit(int? id)
        {
            var model = id.HasValue ? _repo.GetByKey(id) : new SxSiteTest();
            return View(Mapper.Map<SxSiteTest, VMEditSiteTest>(model));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult Edit(VMEditSiteTest model)
        {
            var isArchitect = User.IsInRole("architect");
            var isNew = model.Id == 0;
            if (isNew)
            {
                model.TitleUrl = Url.SeoFriendlyUrl(model.Title);
                if (_repo.All.SingleOrDefault(x => x.TitleUrl == model.TitleUrl) != null)
                    ModelState.AddModelError("Title", "Модель с таким текстовым ключем уже существует");
                else
                    ModelState["TitleUrl"].Errors.Clear();
            }
            else
            {
                if (string.IsNullOrEmpty(model.TitleUrl))
                {
                    var url = Url.SeoFriendlyUrl(model.Title);
                    if (_repo.All.SingleOrDefault(x => x.TitleUrl == url && x.Id != model.Id) != null)
                        ModelState.AddModelError(isArchitect ? "TitleUrl" : "Title", "Модель с таким текстовым ключем уже существует");
                    else
                    {
                        model.TitleUrl = url;
                        ModelState["TitleUrl"].Errors.Clear();
                    }
                }
            }

            var redactModel = Mapper.Map<VMEditSiteTest, SxSiteTest>(model);
            if (ModelState.IsValid)
            {
                SxSiteTest newModel = null;
                if (isNew)
                    newModel = _repo.Create(redactModel);
                else
                {
                    var old = _repo.All.SingleOrDefault(x => x.TitleUrl == model.TitleUrl && x.Id != model.Id);
                    if (old != null)
                        ModelState.AddModelError(isArchitect ? "TitleUrl" : "Title", "Модель с таким текстовым ключем уже существует");
                    if (isArchitect)
                        newModel = _repo.Update(redactModel, true, "Title", "Description", "TestType", "TitleUrl", "Show");
                    else
                        newModel = _repo.Update(redactModel, true, "Title", "Description", "TestType", "Show");
                }
                return RedirectToAction("index");
            }
            else
                return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual RedirectToRouteResult Delete(VMEditSiteTest model)
        {
            _repo.Delete(model.Id);
            return RedirectToAction("index");
        }

        [HttpGet]
        public virtual PartialViewResult TestMatrix(int testId)
        {
            var data = (_repo as SxRepoSiteTest<DbContext>).GetMatrix(testId).Select(x => Mapper.Map<SxSiteTestQuestion, VMSiteTestQuestion>(x)).ToArray();
            return PartialView(MVC.SiteTests.Views._Matrix, data);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult LoadTestFromFile(HttpPostedFileBase file)
        {
            var testRepo = (SxRepoSiteTest<DbContext>)_repo;
            var blocksRepo = new SxRepoSiteTestBlock<DbContext>();
            var questionRepo = new SxRepoSiteTestQuestion<DbContext>();
            var data = testRepo.LoadFromFile(file);
            data.TestType = SxSiteTest.SiteTestType.GuessYesNo;

            SxSiteTest test = null;
            SxSiteTestBlock block = null;
            SxSiteTestQuestion question = null;

            test = testRepo.All.SingleOrDefault(x => x.Title == data.Title);
            if (test != null)
                testRepo.Delete(test.Id);
            var testId=createTest(data, testRepo, blocksRepo, questionRepo, ref test, ref block, ref question);

            return RedirectToAction(MVC.SiteTests.Edit(id: testId));
        }
        private static int createTest(SxSiteTest data, SxRepoSiteTest<DbContext> testRepo, SxRepoSiteTestBlock<DbContext> blocksRepo, SxRepoSiteTestQuestion<DbContext> questionRepo, ref SxSiteTest test, ref SxSiteTestBlock block, ref SxSiteTestQuestion question)
        {
            test = new SxSiteTest { Title = data.Title, Description = data.Description, TestType = data.TestType };
            test = testRepo.Create(test);

            if (test != null)
            {
                foreach (var b in data.Blocks)
                {
                    block = new SxSiteTestBlock { TestId = test.Id, Title = b.Title, Description = b.Description };
                    block = blocksRepo.Create(block);
                    if (block != null)
                    {
                        foreach (var q in b.Questions)
                        {
                            question = new SxSiteTestQuestion { BlockId = block.Id, Text = q.Text, IsCorrect = q.IsCorrect };
                            question = questionRepo.Create(question);
                        }
                    }
                }
            }

            return test.Id;
        }
    }
}