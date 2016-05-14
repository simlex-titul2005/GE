using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using System.Linq;
using System.Web.Mvc;
using GE.WebUI.Extantions.Repositories;
using SX.WebCore;
using System.Web.SessionState;

namespace GE.WebUI.Controllers
{
    public partial class ArticlesController : MaterialsController<int, Article>
    {
        public ArticlesController() : base(Enums.ModelCoreType.Article) { }

        [ChildActionOnly]
        public virtual PartialViewResult ForGamersBlock()
        {
            var gameTitle = Request.RequestContext.RouteData.Values["GameTitle"];
            var viewModel = (base.Repository as RepoArticle).ForGamersBlock((string)gameTitle);
            return PartialView(MVC.Articles.Views._ForGamersBlock, viewModel);
        }

        [OutputCache(Duration =900, VaryByParam = "gt;c;lc")]
        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ActionResult Preview(string gt, string c, int lc)
        {
            if (!Request.IsAjaxRequest()) return null;

            var viewModel = (base.Repository as RepoArticle).PreviewMaterials(gt, c, lc);
            if (!viewModel.Any())
                return Content("<div class=\"empty-result\">Данные отсутствуют</div>");
            return PartialView(MVC.Articles.Views._Preview, viewModel);
        }

        [ChildActionOnly]
        [OutputCache(Duration = 900, VaryByParam = "amount")]
        public virtual PartialViewResult Last(int amount=3)
        {
            var viewModel = (base.Repository as RepoArticle).Last(amount);
            return PartialView(MVC.Articles.Views._Last, viewModel);
        }
    }
}