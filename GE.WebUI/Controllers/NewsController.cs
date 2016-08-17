using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using SX.WebCore;
using System.Web.Mvc;
using GE.WebUI.Extantions.Repositories;
using GE.WebUI.Models;

namespace GE.WebUI.Controllers
{
    public sealed class NewsController : MaterialsController<News>
    {
        public NewsController() : base(Enums.ModelCoreType.News)
        {
            if(Repo==null)
                Repo = new RepoNews<VMMaterial>();
        }

#if !DEBUG
        [OutputCache(Duration = 900, VaryByParam = "lnc;gc;glnc;gtc;vc")]
#endif
        [ChildActionOnly]
        public PartialViewResult LastGamesNewsBlock(int lnc = 5, int gc = 4, int glnc = 3, int gtc = 20, int vc=6)
        {
            ViewBag.VideosAmount = vc;
            var viewModel = (Repo as RepoNews<VMMaterial>).LastGameNewsBlock(lnc, gc, glnc, gtc, vc);
            return PartialView("_LastNewsBlock", viewModel);
        }

#if !DEBUG
        [OutputCache(Duration = 900, VaryByParam = "lnc;clnc;ctc")]
#endif
        [ChildActionOnly]
        public PartialViewResult NewsCategories(int lnc = 5, int clnc = 3, int ctc = 20)
        {
            var data = (Repo as RepoNews<VMMaterial>).LastCategoryBlock(lnc, clnc, ctc);
            return PartialView("_NewsCategories", data);
        }
    }
}