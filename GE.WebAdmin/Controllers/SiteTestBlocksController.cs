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
    public partial class SiteTestBlocksController : BaseController
    {
        private SxDbRepository<int, SxSiteTestBlock, DbContext> _repo;
        public SiteTestBlocksController()
        {
            _repo = new RepoSiteTestBlock<DbContext>();
        }

        private static int _pageSize = 20;

        [HttpGet]
        public virtual ViewResult Index(int page = 1)
        {
            var filter = new WebCoreExtantions.Filter(page, _pageSize);
            var totalItems = (_repo as RepoSiteTestBlock<DbContext>).Count(filter);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.PagerInfo = filter.PagerInfo;

            var viewModel = (_repo as RepoSiteTestBlock<DbContext>).Query(filter).ToArray()
                .Select(x => Mapper.Map<SxSiteTestBlock, VMSiteTestBlock>(x)).ToArray();
            return View(viewModel);
        }

        [HttpPost]
        public virtual PartialViewResult Index(VMSiteTestBlock filterModel, IDictionary<string, SortDirection> order, int page = 1)
        {
            ViewBag.Filter = filterModel;
            ViewBag.Order = order;
            var whereObject = filterModel;

            var testTitle = Request.Form["TestTitle"];
            if (!string.IsNullOrEmpty(testTitle) )
                whereObject.Test = new VMSiteTest { Title = Request.Form["TestTitle"] };

            var filter = new WebCoreExtantions.Filter(page, _pageSize) { Orders = order, WhereExpressionObject = filterModel };
            var totalItems = (_repo as RepoSiteTestBlock<DbContext>).Count(filter);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.PagerInfo = filter.PagerInfo;

            var viewModel = (_repo as RepoSiteTestBlock<DbContext>).Query(filter).ToArray()
                .Select(x => Mapper.Map<SxSiteTestBlock, VMSiteTestBlock>(x)).ToArray(); ;

            return PartialView("_GridView", viewModel);
        }

        [HttpPost]
        public virtual PartialViewResult FindGridView(int testId, VMSiteTestBlock filterModel, int page = 1, int pageSize = 10)
        {
            filterModel.TestId = testId;
            ViewBag.Filter = filterModel;
            var filter = new WebCoreExtantions.Filter(page, pageSize) { WhereExpressionObject= filterModel };
            filter.WhereExpressionObject = filterModel;
            var totalItems = (_repo as RepoSiteTestBlock<DbContext>).Count(filter);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.PagerInfo = filter.PagerInfo;

            var viewModel = (_repo as RepoSiteTestBlock<DbContext>).Query(filter).ToArray()
                .Select(x => Mapper.Map<SxSiteTestBlock, VMSiteTestBlock>(x)).ToArray();

            return PartialView("_FindGridView", viewModel);
        }


        [HttpGet]
        public virtual ViewResult Edit(int? id)
        {
            var model = id.HasValue ? _repo.GetByKey(id) : new SxSiteTestBlock();
            if (id.HasValue)
                ViewBag.SiteTestName = new RepoSiteTest<DbContext>().GetByKey(model.TestId).Title;
            return View(Mapper.Map<SxSiteTestBlock, VMEditSiteTestBlock>(model));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult Edit(VMEditSiteTestBlock model)
        {
            if (model.TestId == 0)
                ModelState.AddModelError("TestId", "Выберите тест");

            var redactModel = Mapper.Map<VMEditSiteTestBlock, SxSiteTestBlock>(model);
            if (ModelState.IsValid)
            {
                SxSiteTestBlock newModel = null;
                if (model.Id == 0)
                    newModel = _repo.Create(redactModel);
                else
                    newModel = _repo.Update(redactModel, true, "Title", "TestId", "Description");
                return RedirectToAction("index");
            }
            else
                return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual RedirectToRouteResult Delete(VMEditSiteTestBlock model)
        {
            _repo.Delete(model.Id);
            return RedirectToAction("index");
        }
    }
}