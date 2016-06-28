using GE.WebCoreExtantions;
using SX.WebCore;
using SX.WebCore.Repositories;
using System.Web.Mvc;
using static SX.WebCore.Enums;

namespace GE.WebUI.Controllers
{
    public sealed class MaterialTagsController : BaseController
    {
        private static SxRepoMaterialTag<DbContext> _repo;
        public MaterialTagsController()
        {
            if(_repo==null)
                _repo = new SxRepoMaterialTag<DbContext>();
        }

#if !DEBUG
        [OutputCache(Duration =900, VaryByParam = "mid;mct")]
#endif
        [AcceptVerbs(HttpVerbs.Get)]
        public PartialViewResult List(int mid, ModelCoreType mct, int maxFs=30, int amount=50)
        {
            var filter = new SxFilter(1, 10) { MaterialId= mid, ModelCoreType=mct };
            var viewModel = _repo.GetCloud(filter);
            string url = "#";
            switch(mct)
            {
                case ModelCoreType.Article:
                    url = Url.Action("~/views/articles/list.cshtml");
                    break;
                case ModelCoreType.News:
                    url = Url.Action("~/views/news/list.cshtml");
                    break;
            }
            ViewData["TagsMaxFs"] = maxFs;
            ViewData["TagsUrl"] = url;
            ViewData["TagsShowHeader"] = true;
            return PartialView("~/views/MaterialTags/_TagsCloud.cshtml", viewModel);
        }
    }
}