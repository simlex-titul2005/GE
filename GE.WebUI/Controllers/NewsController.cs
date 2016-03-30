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
        public virtual PartialViewResult LastNewsBlock(int amount = 5)
        {
            var viewModel = (base.Repository as RepoNews).LastNewsBlock(amount);
            return viewModel.HasNews ? PartialView(MVC.News.Views._LastNewsBlock, viewModel) : null;
        }
    }
}