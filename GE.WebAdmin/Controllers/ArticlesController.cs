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
using GE.WebAdmin.Extantions.Repositories;

namespace GE.WebAdmin.Controllers
{
    public partial class ArticlesController : BaseController
    {
        SxDbRepository<int, Article, DbContext> _repo;
        SxDbRepository<int, ArticleType, DbContext> _repoArticleTypes;
        public ArticlesController()
        {
            _repo = new RepoArticle();
            _repoArticleTypes = new RepoArticleType();
        }

        private static int _pageSize = 20;

        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ViewResult Index(int page = 1)
        {
            var list = _repo.All
                .Skip((page - 1) * _pageSize)
                .Take(_pageSize).Select(x => Mapper.Map<Article, VMArticle>(x)).ToArray();

            ViewData["Page"] = page;
            ViewData["PageSize"] = _pageSize;
            ViewData["RowsCount"] = _repo.All.Count();

            return View(list);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual PartialViewResult Index(VMArticle filter, IDictionary<string, SxExtantions.SortDirection> order, int page = 1)
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
                .Take(_pageSize).ToArray().Select(x => Mapper.Map<Article, VMArticle>(x)).ToArray();

            ViewData["Page"] = page;
            ViewData["PageSize"] = _pageSize;
            ViewData["RowsCount"] = temp.Count();

            return PartialView("_GridView", list);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ViewResult Edit(int? id)
        {
            var model = id.HasValue ? _repo.GetByKey(id, Enums.ModelCoreType.Article) : new Article { ModelCoreType = Enums.ModelCoreType.Article };
            var viewModel = Mapper.Map<Article, VMEditArticle>(model);
            viewModel.OldTitleUrl = viewModel.TitleUrl;
            return View(viewModel);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Edit(VMEditArticle model)
        {
            if(string.IsNullOrEmpty(model.TitleUrl))
            {
                var titleUrl = SX.WebCore.StringHelper.SeoFriendlyUrl(model.Title);
                var existModel = (_repo as RepoArticle).GetByTitleUrl(titleUrl);
                if (existModel != null && existModel.Id != model.Id)
                    ModelState.AddModelError("Title", "Строковый ключ должен быть уникальным");
                else
                {
                    model.TitleUrl = titleUrl;
                    ModelState.Remove("TitleUrl");
                }
            }
            
            if (ModelState.IsValid)
            {
                var isNew = model.Id == 0;
                var redactModel = Mapper.Map<VMEditArticle, Article>(model);
                redactModel.ArticleTypeGameId = model.GameId;
                Article newModel = null;

                if (isNew)
                    newModel = _repo.Create(redactModel);
                else
                {
                    if (model.TitleUrl != model.OldTitleUrl)
                    {
                        var existModel = (_repo as RepoArticle).GetByTitleUrl(model.TitleUrl);
                        if (existModel != null)
                        {
                            ModelState.AddModelError("TitleUrl", "Строковый ключ должен быть уникальным");
                            return View(model);
                        }
                    }

                    newModel = _repo.Update(redactModel, "Title", "TitleUrl", "Show", "GameId", "FrontPictureId", "Html", "ArticleTypeName", "ArticleTypeGameId");
                }

                return RedirectToAction(MVC.Articles.Index());
            }
            else
                return View(model);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Delete(VMEditNews model)
        {
            _repo.Delete(model.Id, model.ModelCoreType);
            return RedirectToAction(MVC.Articles.Index());
        }
    }
}