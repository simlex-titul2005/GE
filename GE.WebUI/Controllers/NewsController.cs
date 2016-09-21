using SX.WebCore;
using System.Web.Mvc;
using GE.WebUI.Models;
using GE.WebUI.Infrastructure.Repositories;
using GE.WebUI.ViewModels;
using SX.WebCore.Repositories;

namespace GE.WebUI.Controllers
{
    public sealed class NewsController : MaterialsController<News, VMNews>
    {
        private static RepoNews _repo = new RepoNews();
        public NewsController() : base(Enums.ModelCoreType.News) { }
        public override SxRepoMaterial<News, VMNews> Repo
        {
            get
            {
                return _repo;
            }
        }

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
    }
}