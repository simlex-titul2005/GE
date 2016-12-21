using SX.WebCore;
using System.Web.Mvc;
using GE.WebUI.Models;
using GE.WebUI.Infrastructure.Repositories;
using GE.WebUI.ViewModels;
using SX.WebCore.SxRepositories;

namespace GE.WebUI.Controllers
{
    public sealed class NewsController : MaterialsController<News, VMNews>
    {
        private static RepoNews _repo = new RepoNews();
        public NewsController() : base((byte)Enums.ModelCoreType.News) { }
        public override SxRepoMaterial<News, VMNews> Repo => _repo;

#if !DEBUG
        [OutputCache(Duration = 900, VaryByParam = "lnc;gc;glnc;gtc;vc")]
#endif
        [ChildActionOnly]
        public PartialViewResult LastGamesNewsBlock(int lnc = 5, int gc = 4, int glnc = 3, int gtc = 20, int vc=6)
        {
            ViewBag.VideosAmount = vc;
            var viewModel = (Repo as RepoNews).LastGameNewsBlock(lnc, gc, glnc, gtc, vc);
            return PartialView("_LastNewsBlock", viewModel);
        }

#if !DEBUG
        [OutputCache(Duration = 900, VaryByParam = "lnc;clnc;ctc")]
#endif
        [ChildActionOnly]
        public PartialViewResult NewsCategories(int lnc = 5, int clnc = 3, int ctc = 20)
        {
            var data = (Repo as RepoNews).LastCategoryBlock(lnc, clnc, ctc);
            return PartialView("_NewsCategories", data);
        }

        public override PartialViewResult Popular(int? mct = null, int? mid = null, int amount = 4)
        {
            var title = "Популяные ";
            switch(mct)
            {
                case 1:
                    title += "статьи";
                    break;
                case 2:
                    title += "новости";
                    break;
                default:
                    title += "материалы";
                    break;
            }

            ViewBag.PopularMaterialTitle = title;
            ViewBag.InNewTab = mct == null;
            return base.Popular(mct, mid, amount);
        }

#if !DEBUG
        [OutputCache(Duration = 3600)]
#endif
        [ChildActionOnly]
        public override PartialViewResult SimilarMaterials(SxFilter filter, int amount = 10)
        {
            ViewBag.SimilarMaterialHeader = "Похожие новости";
            return base.SimilarMaterials(filter, amount);
        }
    }
}