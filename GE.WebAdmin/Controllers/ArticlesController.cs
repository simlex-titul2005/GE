using GE.WebAdmin.Models;
using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using SX.WebCore;
using System.Web.Mvc;
using GE.WebAdmin.Extantions.Repositories;
using Microsoft.AspNet.Identity;
using static SX.WebCore.HtmlHelpers.SxExtantions;
using static SX.WebCore.Enums;
using SX.WebCore.MvcControllers;
using SX.WebCore.ViewModels;

namespace GE.WebAdmin.Controllers
{
    [Authorize(Roles = "author-article")]
    public sealed class ArticlesController : SxMaterialsController<Article, SxVMMaterial, DbContext>
    {
        public ArticlesController():base(ModelCoreType.Article)
        {
            if (Repo == null)
                Repo = new RepoArticle<SxVMMaterial>();
        }

        private static int _pageSize = 20;

        [HttpGet]
        public ViewResult Index(int page = 1)
        {
            var defaultOrder = new SxOrder { FieldName = "DateCreate", Direction = SortDirection.Desc };
            var filter = new SxFilter(page, _pageSize) { Order = defaultOrder };
            var totalItems = (Repo as RepoArticle<SxVMMaterial>).FilterCount(filter);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.Filter = filter;

            var viewModel = (Repo as RepoArticle<SxVMMaterial>).QueryForAdmin(filter);
            return View(viewModel);
        }

        [HttpPost]
        public PartialViewResult Index(VMArticle filterModel, SxOrder order, int page = 1)
        {
            var defaultOrder = new SxOrder { FieldName = "DateCreate", Direction = SortDirection.Desc };
            var filter = new SxFilter(page, _pageSize) { Order = order == null || order.Direction == SortDirection.Unknown ? defaultOrder : order, WhereExpressionObject = filterModel };
            var totalItems = (Repo as RepoArticle<SxVMMaterial>).FilterCount(filter);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.Filter = filter;

            var viewModel = (Repo as RepoArticle<SxVMMaterial>).QueryForAdmin(filter);

            return PartialView("_GridView", viewModel);
        }

        [HttpGet]
        public ViewResult Edit(int? id)
        {
            var model = id.HasValue ? Repo.GetByKey(id, ModelCoreType.Article) : new Article { ModelCoreType = ModelCoreType.Article };
            var viewModel = Mapper.Map<Article, VMEditArticle>(model);
            viewModel.OldTitleUrl = viewModel.TitleUrl;
            if (model.FrontPictureId.HasValue)
                ViewData["FrontPictureIdCaption"] = model.FrontPicture.Caption;
            if (model.Game != null)
                ViewBag.GameTitle = model.Game.Title;
            if (model.Category != null)
                ViewBag.MaterialCategoryTitle = model.Category.Title;

            ViewBag.MaterialId = id;
            ViewBag.ModelCoreType = model.ModelCoreType;

            return View(viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(VMEditArticle model)
        {
            if (model.Title != null && string.IsNullOrEmpty(model.TitleUrl))
            {
                var titleUrl = Url.SeoFriendlyUrl(model.Title);
                var existModel = (Repo as RepoArticle<SxVMMaterial>).GetByTitleUrl(titleUrl);
                if (existModel != null && existModel.Id != model.Id)
                    ModelState.AddModelError("Title", "Строковый ключ должен быть уникальным");
                else
                {
                    model.TitleUrl = titleUrl;
                    ModelState.Remove("TitleUrl");
                }
            }

            var isNew = model.Id == 0;

            if (ModelState.IsValid)
            {
                var redactModel = Mapper.Map<VMEditArticle, Article>(model);

                Article newModel = null;

                if (isNew)
                {
                    redactModel.UserId = User.Identity.GetUserId();
                    newModel = Repo.Create(redactModel);
                }
                else
                {
                    if (model.TitleUrl != model.OldTitleUrl)
                    {
                        var existModel = (Repo as RepoArticle<SxVMMaterial>).GetByTitleUrl(model.TitleUrl);
                        if (existModel != null)
                        {
                            ViewBag.HasError = true;
                            model = isNew ? model : getPreparedArticle(model);
                            ModelState.AddModelError("TitleUrl", "Строковый ключ должен быть уникальным");
                            return View(model);
                        }
                    }

                    newModel = Repo.Update(redactModel, true, "Title", "TitleUrl", "Show", "GameId", "FrontPictureId", "Html", "DateOfPublication", "UserId", "Foreword", "ShowFrontPictureOnDetailPage", "CategoryId", "IsTop", "GameVersion");
                }

                return RedirectToAction("index");
            }
            else
            {
                ViewBag.HasError = true;
                ViewBag.ModelCoreType = ModelCoreType.Article;
                model = isNew ? model : getPreparedArticle(model);
                return View(model);
            }

        }

        private VMEditArticle getPreparedArticle(VMEditArticle newModel)
        {
            var old = Repo.GetByKey(newModel.Id, newModel.ModelCoreType);
            newModel.Category = old.Category;
            newModel.Game = old.Game;
            return newModel;
        }
    }
}