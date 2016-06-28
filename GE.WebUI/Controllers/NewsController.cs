using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using SX.WebCore;
using System.Web.Mvc;
using GE.WebUI.Extantions.Repositories;

namespace GE.WebUI.Controllers
{
    public sealed class NewsController : MaterialsController<int, News>
    {
        public NewsController() : base(Enums.ModelCoreType.News) { }

#if !DEBUG
        [OutputCache(Duration = 900, VaryByParam = "lnc;gc;glnc;gtc")]
#endif
        [ChildActionOnly]
        public PartialViewResult LastGamesNewsBlock(int lnc = 5, int gc = 4, int glnc = 3, int gtc=20)
        {
            var viewModel = (base.Repository as RepoNews).LastGameNewsBlock(lnc, gc, glnc, gtc);
            return PartialView("_LastNewsBlock", viewModel);
        }

#if !DEBUG
        [OutputCache(Duration = 900, VaryByParam = "lnc;clnc;ctc")]
#endif
        [ChildActionOnly]
        public PartialViewResult NewsCategories(int lnc=5, int clnc=3, int ctc=20)
        {
            var data = (Repository as RepoNews).LastCategoryBlock(lnc, clnc, ctc);
            return PartialView("_NewsCategories", data);
        }
    }
}