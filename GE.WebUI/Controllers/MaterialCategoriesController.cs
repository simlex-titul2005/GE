using GE.WebUI.Infrastructure.Repositories;
using GE.WebUI.Models;
using GE.WebUI.ViewModels;
using SX.WebCore.MvcControllers;
using System.Web.Mvc;

namespace GE.WebUI.Controllers
{
    public sealed class MaterialCategoriesController : SxMaterialCategoriesController<MaterialCategory, VMMaterialCategory>
    {
        public MaterialCategoriesController()
        {
            if (Repo == null || (Repo as RepoMaterialCategory) == null)
                Repo = new RepoMaterialCategory();
        }

#if !DEBUG
        [OutputCache(Duration = 3600, VaryByParam ="amount")]
#endif
        [AllowAnonymous]
        public PartialViewResult Featured(int amount)
        {
            var viewModel = (Repo as RepoMaterialCategory).GetFeatured(amount);
            return PartialView("_Featured", viewModel);
        }
    }
}