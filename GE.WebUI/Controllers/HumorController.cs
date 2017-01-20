using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using GE.WebUI.Infrastructure;
using GE.WebUI.Infrastructure.Repositories;
using GE.WebUI.Models;
using GE.WebUI.ViewModels;
using SX.WebCore.MvcApplication;
using SX.WebCore.MvcControllers.Abstract;
using SX.WebCore.SxRepositories;
using SX.WebCore.ViewModels;

namespace GE.WebUI.Controllers
{
    public sealed class HumorController : MaterialsController<Humor, VMHumor>
    {
        private static readonly RepoHumor _repo = new RepoHumor();
        public HumorController() : base(SxMvcApplication.ModelCoreTypeProvider[nameof(Humor)]) { }
        protected override Action<SxBaseController, HashSet<SxVMBreadcrumb>> FillBreadcrumbs => BreadcrumbsManager.WriteBreadcrumbs;

        public override SxRepoMaterial<Humor, VMHumor> Repo => _repo;

        public override ViewResult Add()
        {
            ViewBag.SiteSettingsGoogleRecaptchaSiteKey = ConfigurationManager.AppSettings["SiteSettingsGoogleRecaptchaSiteKey"];
            ViewBag.Categories = MaterialCategoriesController.Repo.GetByModelCoreType(SxMvcApplication.ModelCoreTypeProvider[nameof(Humor)]).Select(x=>new SelectListItem {
                Value=x.Id,
                Text=x.Title
            }).ToArray();

            return base.Add();
        }

        protected override ActionResult AddErrorResult(VMHumor model)
        {
            ViewBag.SiteSettingsGoogleRecaptchaSiteKey = ConfigurationManager.AppSettings["SiteSettingsGoogleRecaptchaSiteKey"];
            ViewBag.Categories = MaterialCategoriesController.Repo.GetByModelCoreType(SxMvcApplication.ModelCoreTypeProvider[nameof(Humor)]).Select(x => new SelectListItem
            {
                Value = x.Id,
                Text = x.Title
            }).ToArray();

            return base.AddErrorResult(model);
        }
    }
}