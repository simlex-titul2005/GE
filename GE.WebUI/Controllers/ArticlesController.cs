using System.Linq;
using System.Web.Mvc;
using GE.WebUI.Models;
using GE.WebUI.Infrastructure.Repositories;
using GE.WebUI.ViewModels;
using SX.WebCore.Repositories;
using GE.WebUI.ViewModels.Abstracts;
using SX.WebCore.ViewModels;
using System;

namespace GE.WebUI.Controllers
{
    public sealed class ArticlesController : MaterialsController<Article, VMArticle>
    {
        private static RepoArticle _repo = new RepoArticle();
        public ArticlesController() : base(MvcApplication.ModelCoreTypeProvider[nameof(Article)]) { }
        public override SxRepoMaterial<Article, VMArticle> Repo
        {
            get
            {
                return _repo;
            }
        }

        [ChildActionOnly]
        public PartialViewResult ForGamersBlock()
        {
            var gameTitle = Request.RequestContext.RouteData.Values["GameTitle"];
            var viewModel = (Repo as RepoArticle).ForGamersBlock((string)gameTitle);
            return PartialView("_ForGamersBlock", viewModel);
        }

        [OutputCache(Duration = 900, VaryByParam = "gt;c;lc")]
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
#if !DEBUG
        [OutputCache(Duration = 900)]
#endif
        public override PartialViewResult Last(byte? mct = default(byte?), int amount = 5, int? mid = default(int?))
        {
            var data = (Repo as RepoArticle).Last(mct, amount, mid, new byte[] {
                MvcApplication.ModelCoreTypeProvider[nameof(Article)],
                MvcApplication.ModelCoreTypeProvider[nameof(News)]
            });

            var viewModel = new VMMaterial[data.Length];
            SxVMMaterial item = null;
            for (int i = 0; i < data.Length; i++)
            {
                item = data[i];
                switch(item.ModelCoreType)
                {
                    case 1:
                        viewModel[i] = new VMArticle { Title = item.Title, TitleUrl = item.TitleUrl, DateCreate = item.DateCreate, ModelCoreType=item.ModelCoreType };
                        break;
                    case 2:
                        viewModel[i] = new VMNews { Title = item.Title, TitleUrl = item.TitleUrl, DateCreate = item.DateCreate, ModelCoreType = item.ModelCoreType };
                        break;
                }
            }

            return PartialView("_Last", viewModel);
        }

        public sealed override PartialViewResult Popular(int? mct = default(int?), int? mid = default(int?), int amount = 4)
        {
            throw new NotImplementedException("Популярные статьи реализованы в контроллере Новостей");
        }
    }
}