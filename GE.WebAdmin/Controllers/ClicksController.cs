using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using SX.WebCore;
using SX.WebCore.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var filter = new WebCoreExtantions.Filter { SkipCount = (page - 1) * _pageSize, PageSize = _pageSize };
            var list = (_repo as RepoClick).QueryForAdmin(filter).ToArray();

            ViewData["Page"] = page;
            ViewData["PageSize"] = _pageSize;
            ViewData["RowsCount"] = (_repo as RepoClick).FilterCount(filter);

            return View(list);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual PartialViewResult Index(VMClick filter, IDictionary<string, SxExtantions.SortDirection> order, int page = 1)
        {
            ViewBag.Filter = filter;
            ViewBag.Order = order;

            var f = new GE.WebCoreExtantions.Filter { SkipCount = (page - 1) * _pageSize, PageSize = _pageSize, Orders=order, WhereExpressionObject= filter };
            var list = (_repo as RepoClick).QueryForAdmin(f).ToArray();

            ViewData["Page"] = page;
            ViewData["PageSize"] = _pageSize;
            ViewData["RowsCount"] = (_repo as RepoClick).FilterCount(f);

            return PartialView("_GridView", list);
        }
    }
}