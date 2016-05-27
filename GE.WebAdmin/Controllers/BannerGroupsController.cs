using GE.WebAdmin.Models;
using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using SX.WebCore;
using SX.WebCore.Abstract;
using SX.WebCore.HtmlHelpers;
using SX.WebCore.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GE.WebAdmin.Extantions.Repositories;

namespace GE.WebAdmin.Controllers
{
    public partial class BannerGroupsController : BaseController
    {
        private static int _pageSize = 20;
        private SxDbRepository<Guid, SxBannerGroup, DbContext> _repo;
        public BannerGroupsController()
        {
            _repo = new RepoBannerGroup<DbContext>();
        }

        [HttpGet]
        public virtual ViewResult Index(int page = 1)
        {
            var filter = new WebCoreExtantions.Filter(page, _pageSize);
            var totalItems = _repo.Count(filter);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.PagerInfo = filter.PagerInfo;

            var viewModel = _repo.Query(filter).ToArray().Select(x => Mapper.Map<SxBannerGroup, VMBannerGroup>(x)).ToArray();
            return View(viewModel);
        }

        [HttpPost]
        public virtual PartialViewResult Index(VMBannerGroup filterModel, IDictionary<string, SxExtantions.SortDirection> order, int page = 1)
        {
            ViewBag.Filter = filterModel;
            ViewBag.Order = order;

            var filter = new WebCoreExtantions.Filter(page, _pageSize) { Orders = order, WhereExpressionObject = filterModel };
            var totalItems = _repo.Count(filter);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.PagerInfo = filter.PagerInfo;

            var viewModel = _repo.Query(filter).ToArray().Select(x => Mapper.Map<SxBannerGroup, VMBannerGroup>(x)).ToArray();

            return PartialView("_GridView", viewModel);
        }

        [HttpGet]
        public virtual ViewResult Edit(Guid? id = null)
        {
            var model = id.HasValue ? _repo.GetByKey((Guid)id) : new SxBannerGroup();
            var viewModel = Mapper.Map<SxBannerGroup, VMEditBannerGroup>(model);
            if (id.HasValue)
                ViewBag.BannerGroupId = model.Id;
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Edit(VMEditBannerGroup model)
        {
            var redactModel = Mapper.Map<VMEditBannerGroup, SxBannerGroup>(model);

            if (ModelState.IsValid)
            {
                SxBannerGroup newModel = null;
                if (model.Id == Guid.Empty)
                {
                    newModel = _repo.Create(redactModel);
                }
                else
                {
                    newModel = _repo.Update(redactModel, true, "Title");
                }

                return RedirectToAction(MVC.BannerGroups.Index());
            }
            else
            {
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Delete(VMEditBannerGroup model)
        {
            _repo.Delete(model.Id);
            return RedirectToAction("index");
        }

        [HttpPost]
        public virtual RedirectToRouteResult AddBanner(Guid bgid, Guid bid)
        {
            (_repo as RepoBannerGroup<DbContext>).AddBanner(bgid, bid);
            return RedirectToAction(MVC.BannerGroups.Edit(bgid));
        }

        [HttpPost]
        public virtual PartialViewResult DeleteBanner(Guid bgid, Guid bid)
        {
            (_repo as RepoBannerGroup<DbContext>).DeleteBanner(bgid, bid);

            var repoBanner = new RepoBanner<DbContext>();
            var filter = new WebCoreExtantions.Filter(1, 20) { WhereExpressionObject = new VMBanner { BannerGroupId = bgid } };
            var totalItems = repoBanner.FilterCount(filter, true);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.PagerInfo = filter.PagerInfo;
            ViewBag.BannerGroupId = bgid;

            var viewModel = repoBanner.QueryForAdmin(filter, true);
            return PartialView(MVC.Banners.Views._GroupBanners, viewModel);
        }
    }
}