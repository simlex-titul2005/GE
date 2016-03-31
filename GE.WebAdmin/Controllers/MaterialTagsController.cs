using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using SX.WebCore;
using SX.WebCore.Abstract;
using System.Web.Mvc;
using static SX.WebCore.Enums;
using GE.WebAdmin.Extantions.Repositories;
using GE.WebAdmin.Models;
using System;
using System.Collections.Generic;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace GE.WebAdmin.Controllers
{
    public partial class MaterialTagsController : BaseController
    {
        private SxDbRepository<string, SxMaterialTag, DbContext> _repo;
        public MaterialTagsController()
        {
            _repo = new RepoMaterialTag();
        }

        private int _pageSize = 10;
        [AcceptVerbs(HttpVerbs.Get)]
        public virtual PartialViewResult Index(int mid, ModelCoreType mct, int page = 1)
        {
            var filter = new WebCoreExtantions.Filter { PageSize = _pageSize, SkipCount = (page - 1) * _pageSize };
            var viewModel = (_repo as RepoMaterialTag).QueryForAdmin(mid, mct, filter);

            ViewData["Page"] = page;
            ViewData["PageSize"] = _pageSize;
            ViewData["RowsCount"] = (_repo as RepoMaterialTag).FilterCount(mid, mct, filter);
            ViewBag.MaterialId = mid;
            ViewBag.ModelCoreType = mct;

            return PartialView(MVC.MaterialTags.Views._GridView, viewModel);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual PartialViewResult Index(int mid, ModelCoreType mct, VMMaterialTag filter, IDictionary<string, SortDirection> order, int page = 1)
        {
            ViewBag.Filter = filter;
            ViewBag.Order = order;

            var f = new WebCoreExtantions.Filter { PageSize = _pageSize, SkipCount = (page - 1) * _pageSize, Orders = order, WhereExpressionObject = filter };
            var list = (_repo as RepoMaterialTag).QueryForAdmin(mid, mct, f);

            ViewData["Page"] = page;
            ViewData["PageSize"] = _pageSize;
            ViewData["RowsCount"] = (_repo as RepoMaterialTag).FilterCount(mid, mct, f);
            ViewBag.MaterialId = mid;
            ViewBag.ModelCoreType = mct;

            return PartialView(MVC.MaterialTags.Views._GridView, list);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ViewResult Edit(int mid, ModelCoreType mct, string id = null)
        {
            var newModel = id != null ? _repo.GetByKey(id, mid, mct) : new SxMaterialTag { MaterialId = mid, ModelCoreType = mct };
            var viewModel = Mapper.Map<SxMaterialTag, VMEditMaterialTag>(newModel);
            return View(viewModel);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Edit(VMEditMaterialTag model)
        {
            if (_repo.GetByKey(model.Id, model.MaterialId, model.ModelCoreType) != null)
                ModelState.AddModelError("Id", "Такой тег уже добавлен для материала");
            if(ModelState.IsValid)
            {
                
                var redactModel = Mapper.Map<VMEditMaterialTag, SxMaterialTag>(model);
                _repo.Create(redactModel);

                string controller = null;
                switch(model.ModelCoreType)
                {
                    case ModelCoreType.Article:
                        controller = "articles";
                        break;
                    case ModelCoreType.News:
                        controller = "news";
                        break;
                }

                TempData["TabId"] = "tags-cloud";
                return RedirectToAction("edit", new { controller = controller, id = model.MaterialId });
            }
            return View(model);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public virtual RedirectToRouteResult Delete(VMMaterialTag model)
        {
            var mid = model.MaterialId;
            var mct = model.ModelCoreType;

            _repo.Delete(model.Id, mid, mct);

            return RedirectToAction(MVC.MaterialTags.Index(mid, mct));
        }
    }
}