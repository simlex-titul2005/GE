using System.Web.Mvc;
using System.Linq;
using System.Configuration;
using SX.WebCore.ViewModels;
using GE.WebUI.Models;
using GE.WebUI.ViewModels;
using GE.WebUI.Infrastructure.Repositories;
using SX.WebCore.Repositories;

namespace GE.WebUI.Controllers
{
    public sealed class HumorController : MaterialsController<Humor, VMHumor>
    {
        private static RepoHumor _repo = new RepoHumor();
        public HumorController() : base(MvcApplication.ModelCoreTypeProvider[nameof(Humor)]) { }

        public override SxRepoMaterial<Humor, VMHumor> Repo
        {
            get
            {
                return _repo;
            }
        }

        public override ViewResult Add()
        {
            ViewBag.SiteSettingsGoogleRecaptchaSiteKey = ConfigurationManager.AppSettings["SiteSettingsGoogleRecaptchaSiteKey"];
            ViewBag.Categories = MaterialCategoriesController.Repo.GetByModelCoreType(MvcApplication.ModelCoreTypeProvider[nameof(Humor)]).Select(x=>new SelectListItem {
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