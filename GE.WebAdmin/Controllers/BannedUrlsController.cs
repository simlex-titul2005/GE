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


    public partial class BannedUrlsController : BaseController
    {
        private static int _pageSize = 20;
        private SxDbRepository<int, SxBannedUrl, DbContext> _repo;
        public BannedUrlsController()
        {
            _repo = new SxRepoBannedUrl<DbContext>();
        }

        [HttpGet]
        public virtual ViewResult Index(int page = 1)
        {
            var filter = new WebCoreExtantions.Filter(page, _pageSize);
            var totalItems = _repo.Count(filter);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.PagerInfo = filter.PagerInfo;

            var viewModel = _repo.Query(filter).ToArray().Select(x => Mapper.Map<SxBannedUrl, VMBannedUrl>(x)).ToArray();
            return View(viewModel);
        }

        [HttpPost]
        public virtual PartialViewResult Index(VMBannedUrl filterModel, IDictionary<string, SxExtantions.SortDirection> order, int page = 1)
        {
            ViewBag.Filter = filterModel;
            ViewBag.Order = order;

            var filter = new WebCoreExtantions.Filter(page, _pageSize) { Orders = order, WhereExpressionObject = filterModel };
            var totalItems = _repo.Count(filter);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.PagerInfo = filter.PagerInfo;

            var viewModel = _repo.Query(filter).ToArray().Select(x => Mapper.Map<SxBannedUrl, VMBannedUrl>(x)).ToArray();

            return PartialView("_GridView", viewModel);
        }

        [HttpGet]
        public virtual ViewResult Edit(int? id = null)
        {
            var model = id.HasValue ? _repo.GetByKey(id) : new SxBannedUrl();
            var seoInfo = Mapper.Map<SxBannedUrl, VMEditBannedUrl>(model);
            return View(seoInfo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Edit(VMEditBannedUrl model)
        {
            if (_repo.All.SingleOrDefault(x=>x.Url== model.Url) != null)
                ModelState.AddModelError("Url", "Такая запись уже содержится в БД");

            var redactModel = Mapper.Map<VMEditBannedUrl, SxBannedUrl>(model);

            if (ModelState.IsValid)
            {
                SxBannedUrl newModel = null;
                if (model.Id == 0)
                    newModel = _repo.Create(redactModel);
                else
                    newModel = _repo.Update(redactModel, true, "Url", "Couse");

                return RedirectToAction(MVC.BannedUrls.Index());
            }
            else
            {
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Delete(VMEditBannedUrl model)
        {
            if (_repo.GetByKey(model.Id) != null)
                _repo.Delete(model.Id);
            return RedirectToAction("index");
        }
    }
}