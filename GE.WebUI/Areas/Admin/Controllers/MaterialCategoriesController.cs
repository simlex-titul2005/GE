using GE.WebUI.ViewModels;
using SX.WebCore.MvcControllers;
using GE.WebUI.Infrastructure.Repositories;

namespace GE.WebUI.Areas.Admin.Controllers
{
    public sealed class MaterialCategoriesController : SxMaterialCategoriesController<VMMaterialCategory>
    {
        public MaterialCategoriesController()
        {
            if (Repo == null || !(Repo is RepoMaterialCategory))
                Repo = new RepoMaterialCategory();
        }
    }
}