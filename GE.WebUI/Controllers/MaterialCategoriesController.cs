using GE.WebUI.Infrastructure.Repositories;
using GE.WebUI.Models;
using GE.WebUI.ViewModels;
using SX.WebCore.MvcControllers;

namespace GE.WebUI.Controllers
{
    public sealed class MaterialCategoriesController : SxMaterialCategoriesController<MaterialCategory, VMMaterialCategory>
    {
        public MaterialCategoriesController()
        {
            if (Repo == null || (Repo as RepoMaterialCategory) == null)
                Repo = new RepoMaterialCategory();
        }
    }
}