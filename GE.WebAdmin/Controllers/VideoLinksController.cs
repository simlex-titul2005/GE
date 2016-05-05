using GE.WebAdmin.Extantions.Repositories;
using GE.WebAdmin.Models;
using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using SX.WebCore;
using SX.WebCore.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using static SX.WebCore.Enums;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace GE.WebAdmin.Controllers
{
    public partial class VideoLinksController : BaseController
    {
        private SxDbRepository<Guid, SxVideo, DbContext> _repo;
        public VideoLinksController()
        {
            _repo = new RepoVideo();
        }

        private static readonly int _pageSize = 20;
        [HttpGet]
        public virtual PartialViewResult Index(int mid, ModelCoreType mct, bool fm = true, int page = 1)
        {
            var filter = new WebCoreExtantions.Filter(page, _pageSize);
            filter.MaterialId = mid;
            filter.ModelCoreType = mct;
            var totalItems = (_repo as RepoVideo).LinkedVideosCount(filter, fm);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.PagerInfo = filter.PagerInfo;

            ViewBag.MaterialId = mid;
            ViewBag.ModelCoreType = mct;

            var viewModel = (_repo as RepoVideo).LinkedVideos(filter, fm);
            return PartialView(MVC.VideoLinks.Views._GridView, viewModel);
        }

        [HttpPost]
        public virtual PartialViewResult Index(int mid, ModelCoreType mct, VMVideo filterModel, IDictionary<string, SortDirection> order, bool fm = true, int page = 1)
        {
            ViewBag.Filter = filterModel;
            ViewBag.Order = order;

            ViewBag.MaterialId = mid;
            ViewBag.ModelCoreType = mct;

            var filter = new WebCoreExtantions.Filter(page, _pageSize) { Orders = order, WhereExpressionObject = filterModel };
            filter.MaterialId = mid;
            filter.ModelCoreType = mct;
            var totalItems = (_repo as RepoVideo).LinkedVideosCount(filter, fm);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.PagerInfo = filter.PagerInfo;

            var viewModel = (_repo as RepoVideo).LinkedVideos(filter, fm);

            return PartialView(MVC.VideoLinks.Views._GridView, viewModel);
        }

        [HttpGet]
        public virtual PartialViewResult IndexNotlinked(int mid, ModelCoreType mct, bool fm = true, int page = 1)
        {
            var filter = new WebCoreExtantions.Filter(page, _pageSize);
            filter.MaterialId = mid;
            filter.ModelCoreType = mct;
            var totalItems = (_repo as RepoVideo).LinkedVideosCount(filter, fm);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.PagerInfo = filter.PagerInfo;

            ViewBag.MaterialId = mid;
            ViewBag.ModelCoreType = mct;

            var viewModel = (_repo as RepoVideo).LinkedVideos(filter, fm);
            return PartialView(MVC.VideoLinks.Views._GridViewNotLinked, viewModel);
        }

        [HttpPost]
        public virtual PartialViewResult IndexNotlinked(int mid, ModelCoreType mct, VMVideo filterModel, IDictionary<string, SortDirection> order, bool fm = true, int page = 1)
        {
            ViewBag.Filter = filterModel;
            ViewBag.Order = order;

            var filter = new WebCoreExtantions.Filter(page, _pageSize) { Orders = order, WhereExpressionObject = filterModel };
            filter.MaterialId = mid;
            filter.ModelCoreType = mct;
            var totalItems = (_repo as RepoVideo).LinkedVideosCount(filter, fm);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.PagerInfo = filter.PagerInfo;

            ViewBag.MaterialId = mid;
            ViewBag.ModelCoreType = mct;

            var viewModel = (_repo as RepoVideo).LinkedVideos(filter, fm);
            return PartialView(MVC.VideoLinks.Views._GridViewNotLinked, viewModel);
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
                    (_repo as RepoVideo).AddMaterialVideo(mid, mct, videoId);
                }
            }
            return Redirect(getReturnUrl(mid, mct));
        }

        [HttpPost]
        public virtual RedirectToRouteResult DeleteMaterialVideo(int mid, ModelCoreType mct, Guid vid)
        {
            (_repo as RepoVideo).DeleteMaterialVideo(mid, mct, vid);
            return RedirectToAction(MVC.VideoLinks.Index(mid, mct, true));
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