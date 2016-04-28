using GE.WebAdmin.Models;
using GE.WebCoreExtantions;
using SX.WebCore;
using SX.WebCore.Abstract;
using SX.WebCore.HtmlHelpers;
using SX.WebCore.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace GE.WebAdmin.Controllers
{
    public partial class BannersController : BaseController
    {
        private static int _pageSize = 20;
        private SxDbRepository<Guid, SxBanner, DbContext> _repo;
        public BannersController()
        {
            _repo = new RepoBanner<DbContext>();
        }

        [HttpGet]
        public virtual ViewResult Index(int page = 1)
        {
            var filter = new WebCoreExtantions.Filter(page, _pageSize);
            var totalItems = _repo.Count(filter);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.PagerInfo = filter.PagerInfo;

            var viewModel = _repo.Query(filter).ToArray().Select(x => Mapper.Map<SxBanner, VMBanner>(x)).ToArray();
            return View(viewModel);
        }

        [HttpPost]
        public virtual PartialViewResult Index(VMBanner filterModel, IDictionary<string, SxExtantions.SortDirection> order, int page = 1)
        {
            ViewBag.Filter = filterModel;
            ViewBag.Order = order;

            var filter = new WebCoreExtantions.Filter(page, _pageSize) { Orders = order, WhereExpressionObject = filterModel };
            var totalItems = _repo.Count(filter);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.PagerInfo = filter.PagerInfo;

            var viewModel = _repo.Query(filter).ToArray().Select(x => Mapper.Map<SxBanner, VMBanner>(x)).ToArray();

            return PartialView("_GridView", viewModel);
        }

        [HttpGet]
        public virtual ViewResult Edit(Guid? id = null)
        {
            var model = id.HasValue ? _repo.GetByKey((Guid)id) : new SxBanner();
            var viewModel = Mapper.Map<SxBanner, VMEditBanner>(model);
            if (!id.HasValue)
                viewModel.PictureId = null;
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
                    newModel = _repo.Update(redactModel, "Title", "PictureId", "Url");

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

        [HttpGet]
        public virtual PartialViewResult FindGridView(Guid? bgid = null, int page = 1)
        {
            var filter = new WebCoreExtantions.Filter(page, _pageSize) { WhereExpressionObject = new VMBanner { BannerGroupId = bgid } };
            var totalItems = _repo.Count(filter);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.PagerInfo = filter.PagerInfo;

            ViewBag.Banners = new RepoBannerGroup<DbContext>().GetByKey(bgid).BannerLinks.Select(x => new VMBanner {
                Id=x.BannerId,
                PictureId=x.Banner.PictureId,
                Title=x.Banner.Title,
                Url=x.Banner.Url
            }).ToArray();

            var viewModel = _repo.Query(filter).ToArray().Select(x => Mapper.Map<SxBanner, VMBanner>(x)).ToArray();
            return PartialView(MVC.Banners.Views._FindGridView, viewModel);
        }

        [HttpPost]
        public virtual PartialViewResult FindGridView(VMBanner filterModel, Guid? bgid = null, int page = 1, int pageSize = 10)
        {
            ViewBag.Filter = filterModel;
            var filter = new WebCoreExtantions.Filter(page, pageSize) { WhereExpressionObject = filterModel };
            var totalItems = _repo.Count(filter);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.PagerInfo = filter.PagerInfo;

            var viewModel = _repo.Query(filter).Select(x => Mapper.Map<SxBanner, VMBanner>(x)).ToArray();

            return PartialView(MVC.Banners.Views._FindGridView, viewModel);
        }
    }
}