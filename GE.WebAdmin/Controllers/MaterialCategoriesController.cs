using GE.WebAdmin.Models;
using GE.WebCoreExtantions;
using SX.WebCore;
using System.Web.Mvc;
using static SX.WebCore.Enums;
using GE.WebCoreExtantions.Repositories;
using SX.WebCore.MvcControllers;

namespace GE.WebAdmin.Controllers
{
    public sealed class MaterialCategoriesController : SxMaterialCategoriesController<DbContext, VMMaterialCategory>
    {
        private static RepoMaterialCategory _repo;
        public MaterialCategoriesController() : base()
        {
            if (_repo == null)
                _repo = new RepoMaterialCategory();
        }

        [HttpGet]
        public sealed override ActionResult Edit(ModelCoreType mct, string pcid = null, string id = null)
        {
            var data = string.IsNullOrEmpty(id) ? new MaterialCategory { ModelCoreType = mct, ParentCategoryId = pcid } : _repo.GetByKey(id);
            var viewModel = Mapper.Map<SxMaterialCategory, VMEditMaterialCategory>(data);

            if (data.FrontPictureId.HasValue)
                ViewData["FrontPictureIdCaption"] = data.FrontPicture.Caption;

            if (viewModel.GameId.HasValue)
                ViewBag.GameTitle = viewModel.Game.Title;

            return View(viewModel);
        }
    }
}