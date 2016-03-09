using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using SX.WebCore;
using SX.WebCore.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GE.WebAdmin.Extantions.Repositories;
using GE.WebAdmin.Models;
using SX.WebCore.HtmlHelpers;

namespace GE.WebAdmin.Controllers
{
    public partial class ClicksController : BaseController
    {
        SxDbRepository<Guid, SxClick, DbContext> _repo;
        public ClicksController()
        {
            _repo = new RepoClick();
        }

        private static int _pageSize = 20;

        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ViewResult Index(int page = 1)
        {
            var filter = new GE.WebCoreExtantions.Filter { SkipCount = (page - 1) * _pageSize, PageSize = _pageSize };
            var list = (_repo as RepoClick).QueryForAdmin(filter).ToArray();

            ViewData["Page"] = page;
            ViewData["PageSize"] = _pageSize;
            ViewData["RowsCount"] = _repo.Count(filter);

            return View(list);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual PartialViewResult Index(VMClick filter, IDictionary<string, SxExtantions.SortDirection> order, int page = 1)
        {
            //string name = filter != null ? filter.Name : null;
            ViewBag.Filter = filter;
            ViewBag.Order = order;

            //select
            //var temp = _repo.All;
            //if (name != null)
            //    temp = temp.Where(x => x.Name.Contains(name));

            //order
            //var orders = order.Where(x => x.Value != SxExtantions.SortDirection.Unknown);
            //if (orders.Count() != 0)
            //{
            //    foreach (var o in orders)
            //    {
            //        if (o.Key == "Id")
            //        {
            //            if (o.Value == SxExtantions.SortDirection.Desc)
            //                temp = temp.OrderByDescending(x => x.Name);
            //            else if (o.Value == SxExtantions.SortDirection.Asc)
            //                temp = temp.OrderBy(x => x.Name);
            //        }
            //    }
            //}

            var f = new GE.WebCoreExtantions.Filter { SkipCount = (page - 1) * _pageSize, PageSize = _pageSize };
            var list = (_repo as RepoClick).QueryForAdmin(f).ToArray();

            ViewData["Page"] = page;
            ViewData["PageSize"] = _pageSize;
            ViewData["RowsCount"] = _repo.Count(f);

            return PartialView("_GridView", list);
        }
    }
}