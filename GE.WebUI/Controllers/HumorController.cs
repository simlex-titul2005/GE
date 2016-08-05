using System.Web.Mvc;
using GE.WebCoreExtantions.Repositories;
using SX.WebCore;
using System.Linq;
using System.Configuration;
using SX.WebCore.Repositories;
using GE.WebCoreExtantions;
using SX.WebCore.ViewModels;

namespace GE.WebUI.Controllers
{
    public sealed class HumorController : MaterialsController<SxHumor>
    {
        public HumorController() : base(Enums.ModelCoreType.Humor) {
            if (Repo == null)
                Repo = new RepoHumor();
        }

        public override ViewResult Add()
        {
            ViewBag.SiteSettingsGoogleRecaptchaSiteKey = ConfigurationManager.AppSettings["SiteSettingsGoogleRecaptchaSiteKey"];
            ViewBag.Categories = (new SxRepoMaterialCategory<DbContext>()).GetByModelCoreType(Enums.ModelCoreType.Humor).Select(x=>new SelectListItem {
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