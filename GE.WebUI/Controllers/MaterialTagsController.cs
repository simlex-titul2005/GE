using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using GE.WebUI.Extantions.Repositories;
using SX.WebCore;
using SX.WebCore.Abstract;
using System.Web.Mvc;
using static SX.WebCore.Enums;

namespace GE.WebUI.Controllers
{
    public partial class MaterialTagsController : BaseController
    {
        private SxDbRepository<string, SxMaterialTag, DbContext> _repo;
        public MaterialTagsController()
        {
            _repo = new RepoMaterialTag();
        }

#if !DEBUG
        [OutputCache(Duration =900, VaryByParam = "mid;mct")]
#endif
        [AcceptVerbs(HttpVerbs.Get)]
        public virtual PartialViewResult List(int mid, ModelCoreType mct, int maxFs=30, int amount=50)
        {
            var filter = new WebCoreExtantions.Filter { MaterialId= mid, ModelCoreType=mct };
            var viewModel = (_repo as RepoMaterialTag).GetCloud(filter);
            string url = "#";
            switch(mct)
            {
                case ModelCoreType.Article:
                    url = Url.Action(MVC.Articles.List());
                    break;
                case ModelCoreType.News:
                    url = Url.Action(MVC.News.List());
                    break;
            }
            ViewBag.Url = url;
            ViewBag.CloudMaxFs = maxFs;
            return PartialView(MVC.MaterialTags.Views._TagsCloud, viewModel);
        }
    }
}