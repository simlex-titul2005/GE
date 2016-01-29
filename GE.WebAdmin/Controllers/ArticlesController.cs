using AutoMapper;
using GE.WebAdmin.Models;
using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using SX.WebCore.HtmlHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GE.WebAdmin.Controllers
{
    public partial class ArticlesController : Controller
    {
        private static int _pageSize = 20;

        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ViewResult Index(int page = 1)
        {
            var dbRepo = new RepoArticle();

            var list = dbRepo.All
                .Skip((page - 1) * _pageSize)
                .Take(_pageSize).ToArray().Select(x => Mapper.Map<Article, VMArticle>(x)).ToArray();

            ViewData["Page"] = page;
            ViewData["PageSize"] = _pageSize;
            ViewData["RowsCount"] = dbRepo.All.Count();

            return View(list);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual PartialViewResult Index(VMArticle filterArticle, IDictionary<string, SxExtantions.SortDirection> order, int page = 1)
        {
            int id = filterArticle != null ? filterArticle.Id : 0;
            string title = filterArticle != null ? filterArticle.Title : null;
            string html = filterArticle != null ? filterArticle.Html : null;
            ViewBag.Filter = filterArticle;
            ViewBag.Order = order;

            //select
            var dbRepo = new RepoArticle();
            var temp = dbRepo.All;
            if (id != 0)
                temp = temp.Where(x => x.Id == id);
            if (title != null)
                temp = temp.Where(x => x.Title.Contains(title));
            if (html != null)
                temp = temp.Where(x => x.Html.Contains(html));

            //order
            var orders = order.Where(x => x.Value != SxExtantions.SortDirection.Unknown);
            if (orders.Count() != 0)
            {
                foreach (var o in orders)
                {
                    if (o.Key == "Title")
                    {
                        if (o.Value == SxExtantions.SortDirection.Desc)
                            temp = temp.OrderByDescending(x => x.Title);
                        else if (o.Value == SxExtantions.SortDirection.Asc)
                            temp = temp.OrderBy(x => x.Title);
                    }
                    if (o.Key == "Html")
                    {
                        if (o.Value == SxExtantions.SortDirection.Desc)
                            temp = temp.OrderByDescending(x => x.Html);
                        else if (o.Value == SxExtantions.SortDirection.Asc)
                            temp = temp.OrderBy(x => x.Html);
                    }
                    if (o.Key == "DateCreate")
                    {
                        if (o.Value == SxExtantions.SortDirection.Desc)
                            temp = temp.OrderByDescending(x => x.DateCreate);
                        else if (o.Value == SxExtantions.SortDirection.Asc)
                            temp = temp.OrderBy(x => x.DateCreate);
                    }
                }
            }

            var list = temp.Skip((page - 1) * _pageSize)
                .Take(_pageSize).ToArray().Select(x => Mapper.Map<Article, VMArticle>(x)).ToArray();

            ViewData["Page"] = page;
            ViewData["PageSize"] = _pageSize;
            ViewData["RowsCount"] = temp.Count();

            return PartialView(MVC.Shared.Views._GridView, list);
        }
    }
}