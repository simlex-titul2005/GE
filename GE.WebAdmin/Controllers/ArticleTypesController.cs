using GE.WebAdmin.Models;
using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using SX.WebCore.Abstract;
using SX.WebCore.HtmlHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GE.WebAdmin.Extantions.Repositories;

namespace GE.WebAdmin.Controllers
{
    public partial class ArticleTypesController : BaseController
    {
        SxDbRepository<int, ArticleType, DbContext> _repo;
        public ArticleTypesController()
        {
            _repo = new RepoArticleType();
        }

        private static int _pageSize = 20;

        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ViewResult Index(int page = 1)
        {
            var f = new GE.WebCoreExtantions.Filter { PageSize = _pageSize, SkipCount = (page - 1) * _pageSize };
            var list = (_repo as RepoArticleType).QueryForAdmin(f).ToArray();

            ViewData["Page"] = page;
            ViewData["PageSize"] = _pageSize;
            ViewData["RowsCount"] = _repo.Count(f);

            return View(list);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual PartialViewResult Index(VMArticleType filter, IDictionary<string, SxExtantions.SortDirection> order, int page = 1)
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

            var f = new GE.WebCoreExtantions.Filter { PageSize = _pageSize, SkipCount = (page - 1) * _pageSize };
            var list = (_repo as RepoArticleType).QueryForAdmin(f).ToArray();

            ViewData["Page"] = page;
            ViewData["PageSize"] = _pageSize;
            ViewData["RowsCount"] = _repo.Count(f);

            return PartialView("_GridView", list);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ViewResult Edit(string name=null, int? gameId=null )
        {
            var model = gameId.HasValue ? _repo.GetByKey(name, gameId) : new ArticleType();
            return View(Mapper.Map<ArticleType, VMEditArticleType>(model));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Edit(VMEditArticleType model)
        {
            var redactModel = Mapper.Map<VMEditArticleType, ArticleType>(model);
            if (ModelState.IsValid)
            {
                ArticleType newModel = null;
                if (model.Id == 0)
                    newModel = _repo.Create(redactModel);
                else
                    newModel = _repo.Update(redactModel, "Description");
                return RedirectToAction(MVC.ArticleTypes.Index());
            }
            else
                return View(redactModel);
        }

        public virtual ActionResult ArticleTypesByGameId(int? gameId, string curName)
        {
            if (gameId.HasValue)
            {
                var viewModel = (_repo as RepoArticleType).GetArticleTypesByGameId((int)gameId)
                    .Select(x => new SelectListItem
                    {
                        Text = x.Description,
                        Value = x.Name,
                        Selected = x.Name == curName
                    });
                return PartialView(MVC.ArticleTypes.Views._ArticleTypesByGameId, viewModel);
            }
            else return Content("<span class=\"form-control text-danger\">Не поддерживается без выбора игры</span>");
        }
    }
}