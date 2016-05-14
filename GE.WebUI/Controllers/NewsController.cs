using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using SX.WebCore;
using System.Web.Mvc;
using GE.WebUI.Extantions.Repositories;
using System.Web.SessionState;

namespace GE.WebUI.Controllers
{
    public partial class NewsController : MaterialsController<int, News>
    {
        public NewsController() : base(Enums.ModelCoreType.News) { }

#if !DEBUG
        [OutputCache(Duration = 900, VaryByParam = "lnc;gc;glnc;gtc")]
#endif
        [ChildActionOnly]
        public virtual PartialViewResult LastGamesNewsBlock(int lnc = 5, int gc = 4, int glnc = 3, int gtc=20)
        {
            var viewModel = (base.Repository as RepoNews).LastGameNewsBlock(lnc, gc, glnc, gtc);
            return PartialView(MVC.News.Views._LastNewsBlock, viewModel);
        }

#if !DEBUG
        [OutputCache(Duration = 900, VaryByParam = "lnc;clnc;ctc")]
#endif
        [ChildActionOnly]
        public virtual PartialViewResult NewsCategories(int lnc=5, int clnc=3, int ctc=20)
        {
            var data = (Repository as RepoNews).LastCategoryBlock(lnc, clnc, ctc);
            return PartialView(MVC.News.Views._NewsCategories, data);
        }
    }
}