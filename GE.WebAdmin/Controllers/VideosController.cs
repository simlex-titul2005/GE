using GE.WebAdmin.Models;
using GE.WebCoreExtantions;
using SX.WebCore;
using SX.WebCore.Abstract;
using SX.WebCore.HtmlHelpers;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using static SX.WebCore.Enums;
using System;
using GE.WebCoreExtantions.Repositories;
using GE.WebAdmin.Extantions.Repositories;

namespace GE.WebAdmin.Controllers
{
    [Authorize(Roles ="admin")]
    public partial class VideosController : BaseController
    {
        private SxDbRepository<Guid, SxVideo, DbContext> _repo;
        public VideosController()
        {
            _repo = new RepoVideo();
        }

        private static int _pageSize = 20;
        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ViewResult Index(int page = 1)
        {
            var filter = new WebCoreExtantions.Filter(page, _pageSize);
            var totalItems = (_repo as RepoVideo).FilterCount(filter);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.PagerInfo = filter.PagerInfo;

            var viewModel = (_repo as RepoVideo).QueryForAdmin(filter);
            return View(viewModel);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual PartialViewResult Index(VMVideo filterModel, IDictionary<string, SxExtantions.SortDirection> order, int page = 1)
        {
            ViewBag.Filter = filterModel;
            ViewBag.Order = order;

            var filter = new WebCoreExtantions.Filter(page, _pageSize) { Orders = order, WhereExpressionObject = filterModel };
            var totalItems = (_repo as RepoVideo).FilterCount(filter);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.PagerInfo = filter.PagerInfo;

            var viewModel = (_repo as RepoVideo).QueryForAdmin(filter);

            return PartialView("_GridView", viewModel);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ViewResult Edit(Guid? id)
        {
            var isNew = !id.HasValue;
            var model = isNew ? new SxVideo() : _repo.GetByKey(id);
            var seoInfo = Mapper.Map<SxVideo, VMEditVideo>(model);
            if (isNew)
                seoInfo.PictureId = null;
            return View(seoInfo);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Edit(VMEditVideo model)
        {
            var redactModel = Mapper.Map<VMEditVideo, SxVideo>(model);
            if (!model.PictureId.HasValue)
                ModelState.AddModelError("PictureId", "Поле обязательно для заполнения");
            if (ModelState.IsValid)
            {
                SxVideo newModel = null;
                if (model.Id == Guid.Empty)
                    newModel = _repo.Create(redactModel);
                else
                    newModel = _repo.Update(redactModel, "Title", "Url", "PictureId");

                return RedirectToAction("Index");
            }
            else
                return View(model);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Delete(VMEditSeoInfo model)
        {
            _repo.Delete(model.Id);
            return RedirectToAction("Index");
        }
    }
}