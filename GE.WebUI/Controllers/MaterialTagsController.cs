using GE.WebUI.Infrastructure;
using SX.WebCore;
using SX.WebCore.MvcControllers;
using System.Web.Mvc;
using static SX.WebCore.Enums;

namespace GE.WebUI.Controllers
{
    public sealed class MaterialTagsController : SxMaterialTagsController
    {
        
#if !DEBUG
        [OutputCache(Duration =900, VaryByParam = "mid;mct")]
#endif
        [HttpGet, AllowAnonymous]
        public PartialViewResult List(int mid, ModelCoreType mct, int maxFs=30, int amount=50)
        {
            var filter = new SxFilter(1, 10) { MaterialId= mid, ModelCoreType=mct };
            var viewModel = Repo.GetCloud(filter);
            string url = "#";
            switch(mct)
            {
                case ModelCoreType.Article:
                    url = Url.Action("List","Articles");
                    break;
                case ModelCoreType.News:
                    url = Url.Action("List", "News");
                    break;
            }
            ViewData["TagsMaxFs"] = maxFs;
            ViewData["TagsUrl"] = url;
            ViewData["TagsShowHeader"] = true;
            return PartialView("~/views/MaterialTags/_TagsCloud.cshtml", viewModel);
        }
    }
}