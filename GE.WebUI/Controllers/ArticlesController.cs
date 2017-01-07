using System.Linq;
using System.Web.Mvc;
using GE.WebUI.Models;
using GE.WebUI.Infrastructure.Repositories;
using GE.WebUI.ViewModels;
using SX.WebCore.SxRepositories;
using GE.WebUI.ViewModels.Abstracts;
using SX.WebCore.ViewModels;
using System;
using SX.WebCore;
using SX.WebCore.MvcApplication;

namespace GE.WebUI.Controllers
{
    public sealed class ArticlesController : MaterialsController<Article, VMArticle>
    {
        private static readonly RepoArticle _repo = new RepoArticle();
        public ArticlesController() : base(SxMvcApplication.ModelCoreTypeProvider[nameof(Article)]) {
        }
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
        public override PartialViewResult Last(byte? mct = null, int amount = 5, int? mid = null)
        {
            var data = (Repo as RepoArticle).GetLast(mct, amount, mid, new[] {
                SxMvcApplication.ModelCoreTypeProvider[nameof(Article)],
                SxMvcApplication.ModelCoreTypeProvider[nameof(News)]
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

        public override PartialViewResult Popular(int? mct = null, int? mid = null, int amount = 4)
        {
            throw new NotImplementedException("Популярные статьи реализованы в контроллере Новостей");
        }

#if !DEBUG
        [OutputCache(Duration = 3600)]
#endif
        [ChildActionOnly]
        public override PartialViewResult SimilarMaterials(SxFilter filter, int amount = 10)
        {
            ViewBag.SimilarMaterialHeader = "Похожие статьи";
            return base.SimilarMaterials(filter, amount);
        }
    }
}