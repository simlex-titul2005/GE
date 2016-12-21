using System.Web.Mvc;
using System.Linq;
using System.Configuration;
using SX.WebCore.ViewModels;
using GE.WebUI.Models;
using GE.WebUI.ViewModels;
using GE.WebUI.Infrastructure.Repositories;
using SX.WebCore.SxRepositories;
using GE.WebUI.Infrastructure;
using SX.WebCore.MvcApplication;

namespace GE.WebUI.Controllers
{
    public sealed class HumorController : MaterialsController<Humor, VMHumor>
    {
        private static readonly RepoHumor _repo = new RepoHumor();
        public HumorController() : base(SxMvcApplication.ModelCoreTypeProvider[nameof(Humor)]) {
            FillBreadcrumbs = BreadcrumbsManager.WriteBreadcrumbs;
        }

        public override SxRepoMaterial<Humor, VMHumor> Repo => _repo;

        public override ViewResult Add()
        {
            ViewBag.SiteSettingsGoogleRecaptchaSiteKey = ConfigurationManager.AppSettings["SiteSettingsGoogleRecaptchaSiteKey"];
            ViewBag.Categories = MaterialCategoriesController.Repo.GetByModelCoreType(SxMvcApplication.ModelCoreTypeProvider[nameof(Humor)]).Select(x=>new SelectListItem {
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