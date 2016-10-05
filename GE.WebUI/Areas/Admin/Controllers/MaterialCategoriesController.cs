using GE.WebUI.ViewModels;
using SX.WebCore.MvcControllers;
using GE.WebUI.Infrastructure.Repositories;
using System.Web.Mvc;
using SX.WebCore;
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

        protected override Func<SxVMMaterialCategory, string> TreeViewMenuFuncContent(ModelCoreType mct)
        {
            switch (mct)
            {
                case ModelCoreType.Aphorism:
                    return (x) => string.Format("<a href=\"{0}\">{1}</a>", Url.Action("Index", "Aphorisms", new { curCat = x.Id }), x.Title);
                case ModelCoreType.Manual:
                    return (x) => string.Format("<a href=\"{0}\">{1}</a>", Url.Action("Index", "FAQ", new { curCat = x.Id }), x.Title);
                default:
                    return null;
            }
        }
    }
}