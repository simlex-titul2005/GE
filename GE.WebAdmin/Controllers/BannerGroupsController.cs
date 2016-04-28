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
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Edit(VMEditBannerGroup model)
        {
            var redactModel = Mapper.Map<VMEditBannerGroup, SxBannerGroup>(model);
            var bannersId = getBannersId(Request.Form.GetValues("banner"));
            if (bannersId == null)
                ModelState.AddModelError("Banners", "Группа должна содержать баннеры");

            if (ModelState.IsValid)
            {
                SxBannerGroup newModel = null;
                if (model.Id == Guid.Empty)
                {
                    newModel = _repo.Create(redactModel);
                }
                else
                {
                    newModel = _repo.Update(redactModel, "Title");
                }

                addBanners(newModel.Id, bannersId);

                return RedirectToAction(MVC.BannerGroups.Index());
            }
            else
            {
                return View(model);
            }
        }

        public void addBanners(Guid bannerGroupId, Guid[] bannersId)
        {
            (_repo as RepoBannerGroup<DbContext>).AddBanners(bannerGroupId, bannersId);
        }

        private static Guid[] getBannersId(string[] bannersId)
        {
            if (bannersId==null || !bannersId.Any()) return null;

            var list = new List<Guid>();
            for (int i = 0; i < bannersId.Length; i++)
            {
                list.Add(Guid.Parse(bannersId[i]));
            }

            return list.ToArray();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Delete(VMEditBannerGroup model)
        {
            _repo.Delete(model.Id);
            return RedirectToAction("index");
        }
    }
}