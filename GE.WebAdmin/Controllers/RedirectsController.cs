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
    public partial class RedirectsController : BaseController
    {
        SxDbRepository<Guid, SxRedirect, DbContext> _repo;
        public RedirectsController()
        {
            _repo = new RepoRedirect();
        }

        private static int _pageSize = 20;

        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ViewResult Index(int page = 1)
        {
            var list = (_repo as RepoRedirect).QueryForAdmin(new GE.WebCoreExtantions.Filter { PageSize = _pageSize, SkipCount = (page - 1) * _pageSize })
                .Select(x => Mapper.Map<SxRedirect, VMRedirect>(x)).ToArray();

            ViewData["Page"] = page;
            ViewData["PageSize"] = _pageSize;
            ViewData["RowsCount"] = _repo.All.Count();

            return View(list);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual PartialViewResult Index(VMRedirect filter, IDictionary<string, SxExtantions.SortDirection> order, int page = 1)
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

            var list = (_repo as RepoRedirect).QueryForAdmin(new GE.WebCoreExtantions.Filter { PageSize = _pageSize, SkipCount = (page - 1) * _pageSize })
                .Select(x => Mapper.Map<SxRedirect, VMRedirect>(x)).ToArray();

            ViewData["Page"] = page;
            ViewData["PageSize"] = _pageSize;
            ViewData["RowsCount"] = _repo.Count(null);

            return PartialView("_GridView", list);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ViewResult Edit(Guid? id)
        {
            var model = id.HasValue ? _repo.GetByKey(id) : new SxRedirect();
            var viewModel = Mapper.Map<SxRedirect, VMEditRedirect>(model);
            return View(viewModel);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Edit(VMEditRedirect model)
        {
            var redactModel = Mapper.Map<VMEditRedirect, SxRedirect>(model);
            if (ModelState.IsValid)
            {
                SxRedirect newModel = null;
                if (model.Id==Guid.Empty)
                    newModel = _repo.Create(redactModel);
                else
                    newModel = _repo.Update(redactModel, "OldUrl", "NewUrl");
                return RedirectToAction(MVC.Redirects.Index());
            }
            else
                return View(redactModel);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Delete(VMEditRedirect model)
        {
            _repo.Delete(model.Id);
            return RedirectToAction(MVC.Redirects.Index());
        }
    }
}