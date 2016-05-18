using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using SX.WebCore;
using SX.WebCore.Abstract;
using System.Web.Mvc;
using static SX.WebCore.Enums;
using GE.WebAdmin.Extantions.Repositories;
using GE.WebAdmin.Models;
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
        [HttpGet]
        public virtual PartialViewResult Index(int mid, ModelCoreType mct, int page = 1)
        {
            ViewBag.MaterialId = mid;
            ViewBag.ModelCoreType = mct;

            var filter = new WebCoreExtantions.Filter(page, _pageSize) { MaterialId = mid, ModelCoreType = mct };
            var totalItems = (_repo as RepoMaterialTag).FilterCount(filter);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.PagerInfo = filter.PagerInfo;

            var viewModel = (_repo as RepoMaterialTag).QueryForAdmin(filter);

            return PartialView(MVC.MaterialTags.Views._GridView, viewModel);
        }

        [HttpPost]
        public virtual PartialViewResult Index(int mid, ModelCoreType mct, VMMaterialTag filterModel, IDictionary<string, SortDirection> order, int page = 1)
        {
            ViewBag.MaterialId = mid;
            ViewBag.ModelCoreType = mct;

            ViewBag.Filter = filterModel;
            ViewBag.Order = order;

            var filter = new WebCoreExtantions.Filter(page, _pageSize) { Orders = order, WhereExpressionObject = filterModel, MaterialId = mid, ModelCoreType = mct };
            var totalItems = (_repo as RepoMaterialTag).FilterCount(filter);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.PagerInfo = filter.PagerInfo;

            var viewModel = (_repo as RepoMaterialTag).QueryForAdmin(filter);

            return PartialView(MVC.MaterialTags.Views._GridView, viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual RedirectToRouteResult Edit(VMEditMaterialTag model)
        {
            if (ModelState.IsValid)
            {
                var id = model.Id.Trim();
                model.Id = id;
                if (_repo.GetByKey(model.Id, model.MaterialId, model.ModelCoreType) != null)
                {
                    ModelState.AddModelError("Id", "Такой тег уже добавлен для материала");
                    return RedirectToAction(MVC.MaterialTags.Index(model.MaterialId, model.ModelCoreType));
                }

                var redactModel = Mapper.Map<VMEditMaterialTag, SxMaterialTag>(model);
                _repo.Create(redactModel);
            }

            return RedirectToAction(MVC.MaterialTags.Index(model.MaterialId, model.ModelCoreType));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual RedirectToRouteResult Delete(VMMaterialTag model)
        {
            _repo.Delete(model.Id, model.MaterialId, model.ModelCoreType);

            return RedirectToAction(MVC.MaterialTags.Index(model.MaterialId, model.ModelCoreType));
        }
    }
}