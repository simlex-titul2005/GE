using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using SX.WebCore;
using SX.WebCore.Abstract;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using GE.WebAdmin.Extantions.Repositories;
using GE.WebAdmin.Models;
using SX.WebCore.HtmlHelpers;

namespace GE.WebAdmin.Controllers
{
    public partial class ClicksController : BaseController
    {
        private SxDbRepository<Guid, SxClick, DbContext> _repo;
        public ClicksController()
        {
            _repo = new RepoClick();
        }

        private static int _pageSize = 20;

        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ViewResult Index(int page = 1)
        {
            var filter = new WebCoreExtantions.Filter(page, _pageSize);
            var totalItems = (_repo as RepoClick).FilterCount(filter);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.PagerInfo = filter.PagerInfo;

            var viewModel = (_repo as RepoClick).QueryForAdmin(filter);
            return View(viewModel);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual PartialViewResult Index(VMClick filterModel, IDictionary<string, SxExtantions.SortDirection> order, int page = 1)
        {
            ViewBag.Filter = filterModel;
            ViewBag.Order = order;

            var filter = new WebCoreExtantions.Filter(page, _pageSize) { Orders = order, WhereExpressionObject = filterModel };
            var totalItems = (_repo as RepoClick).FilterCount(filter);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.PagerInfo = filter.PagerInfo;

            var viewModel = (_repo as RepoClick).QueryForAdmin(filter);

            return PartialView("_GridView", viewModel);
        }
    }
}