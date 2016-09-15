using System.Linq;
using System.Web.Mvc;
using SX.WebCore;
using GE.WebUI.Models;
using GE.WebUI.Infrastructure.Repositories;
using GE.WebUI.ViewModels;
using GE.WebUI.Infrastructure;

namespace GE.WebUI.Controllers
{
    public sealed class ArticlesController : MaterialsController<Article, VMArticle>
    {
        public ArticlesController() : base(Enums.ModelCoreType.Article) {
            if (Repo == null || !(Repo is RepoArticle))
                Repo = new RepoArticle();

            WriteBreadcrumbs = BreadcrumbsManager.WriteBreadcrumbs;
        }

        [ChildActionOnly]
        public PartialViewResult ForGamersBlock()
        {
            var gameTitle = Request.RequestContext.RouteData.Values["GameTitle"];
            var viewModel = (Repo as RepoArticle).ForGamersBlock((string)gameTitle);
            return PartialView("_ForGamersBlock", viewModel);
        }

        [OutputCache(Duration =900, VaryByParam = "gt;c;lc")]
        [HttpGet]
        public ActionResult Preview(string gt, string c, int lc)
        {
            if (!Request.IsAjaxRequest()) return null;

            var viewModel = (Repo as RepoArticle).PreviewMaterials(gt, c, lc);
            if (!viewModel.Any())
                return Content("<div class=\"empty-result\">Данные отсутствуют</div>");
            return PartialView("_Preview", viewModel);
        }

        [ChildActionOnly]
        [OutputCache(Duration = 900, VaryByParam = "amount")]
        public PartialViewResult Last(int amount=3)
        {
            var viewModel = (Repo as RepoArticle).Last(amount);
            return PartialView("_Last", viewModel);
        }
    }
}