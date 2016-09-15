using System.Web.Mvc;
using SX.WebCore;
using System.Linq;
using System.Configuration;
using SX.WebCore.ViewModels;
using GE.WebUI.Models;
using GE.WebUI.Infrastructure;
using SX.WebCore.MvcControllers;
using GE.WebUI.ViewModels;
using GE.WebUI.Infrastructure.Repositories;

namespace GE.WebUI.Controllers
{
    public sealed class HumorController : MaterialsController<Humor, VMHumor>
    {
        public HumorController() : base(Enums.ModelCoreType.Humor) {
            if (Repo == null)
                Repo = new RepoHumor();
        }

        public override ViewResult Add()
        {
            ViewBag.SiteSettingsGoogleRecaptchaSiteKey = ConfigurationManager.AppSettings["SiteSettingsGoogleRecaptchaSiteKey"];
            ViewBag.Categories = SxMaterialCategoriesController<SxVMMaterialCategory, DbContext>.Repo.GetByModelCoreType(Enums.ModelCoreType.Humor).Select(x=>new SelectListItem {
                Value=x.Id,
                Text=x.Title
            }).ToArray();
            var breadcrumbs = (SxVMBreadcrumb[])ViewBag.Breadcrumbs;
            if (breadcrumbs != null)
            {
                var bc = breadcrumbs.ToList();
                bc.Add(new SxVMBreadcrumb { Title = "Добавить юмор" });
                ViewBag.Breadcrumbs = bc.ToArray();
            }

            return base.Add();
        }
    }
}