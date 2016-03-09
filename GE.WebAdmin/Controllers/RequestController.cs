using GE.WebAdmin.Models;
using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using SX.WebCore;
using SX.WebCore.Abstract;
using SX.WebCore.HtmlHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GE.WebAdmin.Controllers
{
    public partial class RequestController : BaseController
    {
        SxDbRepository<Guid, SxRequest, DbContext> _repo;
        public RequestController()
        {
            _repo = new RepoRequest();
        }

        private static int _pageSize = 50;

        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ViewResult Index(int page = 1)
        {
            var list = (_repo as RepoRequest).QueryForAdmin(new GE.WebCoreExtantions.Filter { PageSize = _pageSize, SkipCount = (page - 1) * _pageSize })
                .Select(x => Mapper.Map<SxRequest, VMRequest>(x)).ToArray();

            ViewData["Page"] = page;
            ViewData["PageSize"] = _pageSize;
            ViewData["RowsCount"] = _repo.All.Count();

            return View(list);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual PartialViewResult Index(VMRequest filter, IDictionary<string, SxExtantions.SortDirection> order, int page = 1)
        {
            //string sessionId = filter != null ? filter.SessionId : null;
            //string urlRef = filter != null ? filter.UrlRef : null;
            //string browser = filter != null ? filter.Browser : null;
            //string clientIP = filter != null ? filter.ClientIP : null;
            //string userAgent = filter != null ? filter.UserAgent : null;
            //string requestType = filter != null ? filter.RequestType : null;
            //string rawUrl = filter != null ? filter.RawUrl : null;
            ViewBag.Filter = filter;
            ViewBag.Order = order;

            //select
            //var temp = _repo.All;
            //if (id != 0)
            //    temp = temp.Where(x => x.Id == id);
            //if (title != null)
            //    temp = temp.Where(x => x.Title.Contains(title));
            //if (html != null)
            //    temp = temp.Where(x => x.Html.Contains(html));

            //order
            //var orders = order.Where(x => x.Value != SxExtantions.SortDirection.Unknown);
            //if (orders.Count() != 0)
            //{
            //    foreach (var o in orders)
            //    {
            //        if (o.Key == "Title")
            //        {
            //            if (o.Value == SxExtantions.SortDirection.Desc)
            //                temp = temp.OrderByDescending(x => x.Title);
            //            else if (o.Value == SxExtantions.SortDirection.Asc)
            //                temp = temp.OrderBy(x => x.Title);
            //        }
            //        if (o.Key == "Html")
            //        {
            //            if (o.Value == SxExtantions.SortDirection.Desc)
            //                temp = temp.OrderByDescending(x => x.Html);
            //            else if (o.Value == SxExtantions.SortDirection.Asc)
            //                temp = temp.OrderBy(x => x.Html);
            //        }
            //        if (o.Key == "DateCreate")
            //        {
            //            if (o.Value == SxExtantions.SortDirection.Desc)
            //                temp = temp.OrderByDescending(x => x.DateCreate);
            //            else if (o.Value == SxExtantions.SortDirection.Asc)
            //                temp = temp.OrderBy(x => x.DateCreate);
            //        }
            //    }
            //}

            var list = (_repo as RepoRequest).QueryForAdmin(new GE.WebCoreExtantions.Filter { PageSize = _pageSize, SkipCount = (page - 1) * _pageSize })
                .Select(x => Mapper.Map<SxRequest, VMRequest>(x)).ToArray();

            ViewData["Page"] = page;
            ViewData["PageSize"] = _pageSize;
            ViewData["RowsCount"] = _repo.Count(null);

            return PartialView("_GridView", list);
        }
    }
}