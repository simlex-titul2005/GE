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
    public partial class NewsController : BaseController
    {
        SxDbRepository<int, News, DbContext> _repo;
        public NewsController()
        {
            _repo = new RepoNews();
        }

        private static int _pageSize = 20;

        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ViewResult Index(int page = 1)
        {
            var list = _repo.All
                .OrderByDescending(x=>x.DateCreate)
                .Skip((page - 1) * _pageSize)
                .Take(_pageSize).Select(x => Mapper.Map<News, VMNews>(x)).ToArray();

            ViewData["Page"] = page;
            ViewData["PageSize"] = _pageSize;
            ViewData["RowsCount"] = _repo.All.Count();

            return View(list);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual PartialViewResult Index(VMNews filter, IDictionary<string, SxExtantions.SortDirection> order, int page = 1)
        {
            int id = filter != null ? filter.Id : 0;
            string title = filter != null ? filter.Title : null;
            string html = filter != null ? filter.Html : null;
            ViewBag.Filter = filter;
            ViewBag.Order = order;

            //select
            var temp = _repo.All;
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
                .Take(_pageSize).ToArray().Select(x => Mapper.Map<News, VMNews>(x)).ToArray();

            ViewData["Page"] = page;
            ViewData["PageSize"] = _pageSize;
            ViewData["RowsCount"] = temp.Count();

            return PartialView("_GridView", list);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ViewResult Edit(int? id)
        {
            var model = id.HasValue ? _repo.GetByKey(id, Enums.ModelCoreType.News) : new News { ModelCoreType = Enums.ModelCoreType.News };
            return View(Mapper.Map<News, VMEditNews>(model));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Edit(VMEditNews model)
        {
            var redactModel = Mapper.Map<VMEditNews, News>(model);
            if (ModelState.IsValid)
            {
                News newModel = null;
                if (model.Id == 0)
                    newModel = _repo.Create(redactModel);
                else
                    newModel = _repo.Update(redactModel, "Title", "Show", "GameId", "FrontPictureId", "Html");

                return RedirectToAction(MVC.News.Index());
            }
            else
                return View(model);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Delete(VMEditNews model)
        {
            _repo.Delete(model.Id, model.ModelCoreType);
            return RedirectToAction(MVC.News.Index());
        }
    }
}