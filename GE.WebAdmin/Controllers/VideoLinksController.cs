using GE.WebAdmin.Models;
using GE.WebCoreExtantions;
using SX.WebCore;
using SX.WebCore.Repositories;
using SX.WebCore.ViewModels;
using System;
using System.Linq;
using System.Web.Mvc;
using static SX.WebCore.Enums;

namespace GE.WebAdmin.Controllers
{
    public partial class VideoLinksController : BaseController
    {
        private static SxRepoVideo<DbContext> _repo;
        public VideoLinksController()
        {
            if(_repo==null)
                _repo = new SxRepoVideo<DbContext>();
        }

        private static readonly int _pageSize = 20;
        [HttpGet]
        public virtual PartialViewResult Index(int mid, ModelCoreType mct, bool fm = true, int page = 1)
        {
            var filter = new SxFilter(page, _pageSize);
            filter.MaterialId = mid;
            filter.ModelCoreType = mct;
            var totalItems = _repo.LinkedVideosCount(filter, fm);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.PagerInfo = filter.PagerInfo;

            ViewBag.MaterialId = mid;
            ViewBag.ModelCoreType = mct;

            var viewModel = _repo.LinkedVideos(filter, fm);
            return PartialView("~/views/VideoLinks/_GridView.cshtml", viewModel);
        }

        [HttpPost]
        public virtual PartialViewResult Index(int mid, ModelCoreType mct, SxVMVideo filterModel, SxOrder order, bool fm = true, int page = 1)
        {
            ViewBag.Filter = filterModel;
            ViewBag.Order = order;

            ViewBag.MaterialId = mid;
            ViewBag.ModelCoreType = mct;

            var filter = new SxFilter(page, _pageSize) { Order = order, WhereExpressionObject = filterModel };
            filter.MaterialId = mid;
            filter.ModelCoreType = mct;
            var totalItems = _repo.LinkedVideosCount(filter, fm);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.PagerInfo = filter.PagerInfo;

            var viewModel = _repo.LinkedVideos(filter, fm);

            return PartialView("~/views/VideoLinks/_GridView.cshtml", viewModel);
        }

        [HttpGet]
        public virtual PartialViewResult IndexNotlinked(int mid, ModelCoreType mct, bool fm = true, int page = 1)
        {
            var filter = new SxFilter(page, _pageSize);
            filter.MaterialId = mid;
            filter.ModelCoreType = mct;
            var totalItems = _repo.LinkedVideosCount(filter, fm);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.PagerInfo = filter.PagerInfo;

            ViewBag.MaterialId = mid;
            ViewBag.ModelCoreType = mct;

            var viewModel = _repo.LinkedVideos(filter, fm);
            return PartialView("~/views/VideoLinks/_GridViewNotLinked.cshtml", viewModel);
        }

        [HttpPost]
        public virtual PartialViewResult IndexNotlinked(int mid, ModelCoreType mct, SxVMVideo filterModel, SxOrder order, bool fm = true, int page = 1)
        {
            ViewBag.Filter = filterModel;
            ViewBag.Order = order;

            var filter =new SxFilter(page, _pageSize) { Order = order, WhereExpressionObject = filterModel };
            filter.MaterialId = mid;
            filter.ModelCoreType = mct;
            var totalItems = _repo.LinkedVideosCount(filter, fm);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.PagerInfo = filter.PagerInfo;

            ViewBag.MaterialId = mid;
            ViewBag.ModelCoreType = mct;

            var viewModel = _repo.LinkedVideos(filter, fm);
            return PartialView("~/views/VideoLinks/_GridViewNotLinked.cshtml", viewModel);
        }

        [HttpPost]
        public virtual RedirectResult AddMaterialVideo(int mid, ModelCoreType mct)
        {
            var videos = Request.Form.GetValues("video");
            if (videos!=null && videos.Any())
            {
                for (int i = 0; i < videos.Length; i++)
                {
                    var videoId = Guid.Parse(videos[i]);
                    _repo.AddMaterialVideo(mid, mct, videoId);
                }
            }
            return Redirect(getReturnUrl(mid, mct));
        }

        [HttpPost]
        public virtual RedirectToRouteResult DeleteMaterialVideo(int mid, ModelCoreType mct, Guid vid)
        {
            _repo.DeleteMaterialVideo(mid, mct, vid);
            return RedirectToAction("index", new { mid= mid , mct=mct, fm =true});
        }

        private static string getReturnUrl(int mid, ModelCoreType mct)
        {
            var url = string.Empty;
            switch(mct)
            {
                case ModelCoreType.Article:
                    url= "/articles/edit?id=" + mid + "&fragment=videos";
                    break;
                case ModelCoreType.News:
                    url= "/news/edit?id=" + mid + "&fragment=videos";
                    break;
            }
            return url;
        }
    }
}