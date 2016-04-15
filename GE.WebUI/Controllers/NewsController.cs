using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using SX.WebCore;
using System.Web.Mvc;
using GE.WebUI.Extantions.Repositories;

namespace GE.WebUI.Controllers
{
    public partial class NewsController : MaterialsController<int, News>
    {
        public NewsController() : base(Enums.ModelCoreType.News) { }

#if !DEBUG
        [OutputCache(Duration =900)]
#endif
        [ChildActionOnly]
        public virtual PartialViewResult LastGamesNewsBlock(int lnc = 5, int gc = 4, int glnc = 3, int gtc=20)
        {
            var viewModel = (base.Repository as RepoNews).LastGameNewsBlock(lnc, gc, glnc, gtc);
            return PartialView(MVC.News.Views._LastNewsBlock, viewModel);
        }
    }
}