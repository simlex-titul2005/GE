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
    public partial class RequestController : BaseController
    {
        private SxDbRepository<Guid, SxRequest, DbContext> _repo;
        public RequestController()
        {
            _repo = new RepoRequest();
        }

        private static int _pageSize = 50;

        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ViewResult Index(int page = 1)
        {
            var filter = new WebCoreExtantions.Filter { PageSize = _pageSize, SkipCount = (page - 1) * _pageSize };
            var list = (_repo as RepoRequest).QueryForAdmin(filter);

            ViewData["Page"] = page;
            ViewData["PageSize"] = _pageSize;
            ViewData["RowsCount"] = (_repo as RepoRequest).FilterCount(filter);

            return View(list);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual PartialViewResult Index(VMRequest filter, IDictionary<string, SxExtantions.SortDirection> order, int page = 1)
        {
            ViewBag.Filter = filter;
            ViewBag.Order = order;

            var f = new WebCoreExtantions.Filter { PageSize = _pageSize, SkipCount = (page - 1) * _pageSize, Orders=order, WhereExpressionObject = filter };
            var list = (_repo as RepoRequest).QueryForAdmin(f);

            ViewData["Page"] = page;
            ViewData["PageSize"] = _pageSize;
            ViewData["RowsCount"] = (_repo as RepoRequest).FilterCount(f);

            return PartialView("_GridView", list);
        }
    }
}