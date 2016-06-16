using GE.WebAdmin.Models;
using GE.WebCoreExtantions;
using SX.WebCore;
using SX.WebCore.Abstract;
using SX.WebCore.HtmlHelpers;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using GE.WebAdmin.Extantions.Repositories;
using SX.WebCore.Repositories;
using System.Linq;

namespace GE.WebAdmin.Controllers
{
    public partial class BannersController : BaseController
    {
        private static int _pageSize = 20;
        private SxDbRepository<Guid, SxBanner, DbContext> _repo;
        public BannersController()
        {
            _repo = new SxRepoBanner<DbContext>();
        }

        [HttpGet]
        public virtual ViewResult Index(int page = 1)
        {
            var filter = new WebCoreExtantions.Filter(page, _pageSize);
            var totalItems = (_repo as SxRepoBanner<DbContext>).FilterCount(filter);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.PagerInfo = filter.PagerInfo;

            var viewModel = (_repo as SxRepoBanner<DbContext>).QueryForAdmin(filter);
            return View(viewModel);
        }

        [HttpPost]
        public virtual PartialViewResult Index(VMBanner filterModel, IDictionary<string, SxExtantions.SortDirection> order, int page = 1)
        {
            ViewBag.Filter = filterModel;
            ViewBag.Order = order;

            var filter = new WebCoreExtantions.Filter(page, _pageSize) { Orders = order, WhereExpressionObject = filterModel };
            var totalItems = (_repo as SxRepoBanner<DbContext>).FilterCount(filter);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.PagerInfo = filter.PagerInfo;

            var viewModel = (_repo as SxRepoBanner<DbContext>).QueryForAdmin(filter);

            return PartialView("_GridView", viewModel);
        }

        [HttpGet]
        public virtual ViewResult Edit(Guid? id = null)
        {
            var model = id.HasValue ? _repo.GetByKey((Guid)id) : new SxBanner();
            var viewModel = Mapper.Map<SxBanner, VMEditBanner>(model);
            viewModel.Place = viewModel.Place == SxBanner.BannerPlace.Unknown ? null : viewModel.Place;
            if (!id.HasValue)
                viewModel.PictureId = null;
            else
                ViewBag.PictureCaption = model.Picture.Caption;

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Edit(VMEditBanner model)
        {
            var redactModel = Mapper.Map<VMEditBanner, SxBanner>(model);

            if (ModelState.IsValid)
            {
                SxBanner newModel = null;
                if (model.Id == Guid.Empty)
                    newModel = _repo.Create(redactModel);
                else
                    newModel = _repo.Update(redactModel, true, "Title", "PictureId", "Url", "Place", "ControllerName", "ActionName");

                return RedirectToAction(MVC.Banners.Index());
            }
            else
            {
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Delete(VMEditBanner model)
        {
            _repo.Delete(model.Id);
            return RedirectToAction("index");
        }

        [HttpPost]
        public virtual PartialViewResult FindGridView(Guid bgid, VMBanner filterModel = null, int page = 1, int pageSize = 10)
        {
            filterModel.BannerGroupId = bgid;
            ViewBag.Filter = filterModel;
            var filter = new WebCoreExtantions.Filter(page, pageSize) { WhereExpressionObject = filterModel };
            var totalItems = (_repo as SxRepoBanner<DbContext>).FilterCount(filter, false);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.PagerInfo = filter.PagerInfo;
            ViewBag.BannerGroupId = bgid;

            var viewModel = (_repo as SxRepoBanner<DbContext>).QueryForAdmin(filter, false);

            return PartialView(MVC.Banners.Views._FindGridView, viewModel);
        }

        [HttpGet]
        public virtual PartialViewResult GroupBanners(Guid bgid, int page = 1, int pageSize = 10)
        {
            var filter = new WebCoreExtantions.Filter(page, pageSize) { WhereExpressionObject = new VMBanner { BannerGroupId = bgid } };
            var totalItems = (_repo as SxRepoBanner<DbContext>).FilterCount(filter, true);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.PagerInfo = filter.PagerInfo;
            ViewBag.BannerGroupId = bgid;

            var viewModel = (_repo as SxRepoBanner<DbContext>).QueryForAdmin(filter, true);
            return PartialView(MVC.Banners.Views._GroupBanners, viewModel);
        }

        [HttpPost]
        public virtual PartialViewResult GroupBanners(Guid bgid, VMBanner filterModel, IDictionary<string, SxExtantions.SortDirection> order, int page = 1, int pageSize = 10)
        {
            ViewBag.Filter = filterModel;
            ViewBag.Order = order;

            filterModel.BannerGroupId = bgid;
            var filter = new WebCoreExtantions.Filter(page, _pageSize) { Orders = order, WhereExpressionObject = filterModel };
            var totalItems = (_repo as SxRepoBanner<DbContext>).FilterCount(filter, true);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.PagerInfo = filter.PagerInfo;
            ViewBag.BannerGroupId = bgid;

            var viewModel = (_repo as SxRepoBanner<DbContext>).QueryForAdmin(filter, true);

            return PartialView(MVC.Banners.Views._GroupBanners, viewModel);
        }
    }
}