using GE.WebAdmin.Models;
using GE.WebCoreExtantions;
using SX.WebCore;
using SX.WebCore.Abstract;
using SX.WebCore.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace GE.WebAdmin.Controllers
{
    [Authorize(Roles = "admin")]
    public partial class SiteTestQuestionsController : BaseController
    {
        private SxDbRepository<int, SxSiteTestQuestion, DbContext> _repo;
        public SiteTestQuestionsController()
        {
            _repo = new RepoSiteTestQuestion<DbContext>();
        }

        private static int _pageSize = 20;

        [HttpGet]
        public virtual ViewResult Index(int page = 1)
        {
            var filter = new WebCoreExtantions.Filter(page, _pageSize);
            var totalItems = (_repo as RepoSiteTestQuestion<DbContext>).Count(filter);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.PagerInfo = filter.PagerInfo;

            var viewModel = (_repo as RepoSiteTestQuestion<DbContext>).Query(filter).ToArray()
                .Select(x => Mapper.Map<SxSiteTestQuestion, VMSiteTestQuestion>(x)).ToArray();
            return View(viewModel);
        }

        [HttpPost]
        public virtual PartialViewResult Index(VMSiteTestQuestion filterModel, IDictionary<string, SortDirection> order, int page = 1)
        {
            var blockTitle = Request.Form["BlockTitle"];
            var testTitle = Request.Form["TestTitle"];
            if (!string.IsNullOrEmpty(blockTitle) || !string.IsNullOrEmpty(testTitle))
            {
                filterModel.Block = new VMSiteTestBlock
                {
                    Title = blockTitle,
                    Test = string.IsNullOrEmpty(testTitle) ? null : new VMSiteTest { Title = testTitle }
                };
            }

            ViewBag.Filter = filterModel;
            ViewBag.Order = order;
            var whereObject = filterModel;

            var filter = new WebCoreExtantions.Filter(page, _pageSize) { Orders = order, WhereExpressionObject = filterModel };
            var totalItems = (_repo as RepoSiteTestQuestion<DbContext>).Count(filter);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.PagerInfo = filter.PagerInfo;

            var viewModel = (_repo as RepoSiteTestQuestion<DbContext>).Query(filter).ToArray()
                .Select(x => Mapper.Map<SxSiteTestQuestion, VMSiteTestQuestion>(x)).ToArray(); ;

            return PartialView("_GridView", viewModel);
        }

        [HttpGet]
        public virtual ViewResult Edit(int? id)
        {
            var model = id.HasValue ? _repo.GetByKey(id) : new SxSiteTestQuestion();
            if (id.HasValue)
            {
                ViewBag.SiteTestName = model.Block.Test.Title;
                ViewBag.SiteTestBlockName = model.Block.Title;
            }
            return View(Mapper.Map<SxSiteTestQuestion, VMEditSiteTestQuestion>(model));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult Edit(VMEditSiteTestQuestion model)
        {
            if (model.BlockId == 0)
                ModelState.AddModelError("BlockId", "Выберите блок теста");

            var redactModel = Mapper.Map<VMEditSiteTestQuestion, SxSiteTestQuestion>(model);
            if (ModelState.IsValid)
            {
                SxSiteTestQuestion newModel = null;
                if (model.Id == 0)
                    newModel = _repo.Create(redactModel);
                else
                    newModel = _repo.Update(redactModel, true, "Text", "BlockId", "IsCorrect");
                return RedirectToAction("index");
            }
            else
            {
                if (model.Id != 0)
                {
                    var old = _repo.GetByKey(model.Id);
                    ViewBag.SiteTestName = old.Block.Test.Title;
                    ViewBag.SiteTestBlockName = old.Block.Title;
                }
                return View(model);
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual RedirectToRouteResult Delete(VMEditSiteTestQuestion model)
        {
            _repo.Delete(model.Id);
            return RedirectToAction("index");
        }
    }
}