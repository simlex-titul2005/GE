using GE.WebUI.ViewModels;
using SX.WebCore.MvcControllers;
using GE.WebUI.Infrastructure.Repositories;
using SX.WebCore.ViewModels;
using System;
using static SX.WebCore.Enums;

namespace GE.WebUI.Areas.Admin.Controllers
{
    public sealed class MaterialCategoriesController : SxMaterialCategoriesController<VMMaterialCategory>
    {
        public MaterialCategoriesController()
        {
            if (Repo == null || !(Repo is RepoMaterialCategory))
                Repo = new RepoMaterialCategory();
        }

        protected override Func<SxVMMaterialCategory, string> TreeViewMenuFuncContent(byte mct)
        {
            switch (mct)
            {
                case 6://aphorism
                    return (x) => string.Format("<a href=\"{0}\">{1}</a>", Url.Action("Index", "Aphorisms", new { curCat = x.Id }), x.Title);
                case (byte)ModelCoreType.Manual:
                    return (x) => string.Format("<a href=\"{0}\">{1}</a>", Url.Action("Index", "FAQ", new { curCat = x.Id }), x.Title);
                default:
                    return null;
            }
        }
    }
}