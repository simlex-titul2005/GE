using GE.WebAdmin.Models;
using GE.WebCoreExtantions;
using SX.WebCore;
using SX.WebCore.Abstract;
using SX.WebCore.HtmlHelpers;
using SX.WebCore.Repositories;
using System.Collections.Generic;
using System.Linq;
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
                if(string.IsNullOrEmpty(model.TitleUrl))
                {
                    var url=Url.SeoFriendlyUrl(model.Title);
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
                        newModel = _repo.Update(redactModel, true, "Title", "Description", "TestType", "TitleUrl");
                    else
                        newModel = _repo.Update(redactModel, true, "Title", "Description", "TestType");
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
    }
}