using GE.WebAdmin.Models;
using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using SX.WebCore;
using System.Web.Mvc;
using GE.WebAdmin.Extantions.Repositories;
using Microsoft.AspNet.Identity;
using SX.WebCore.Repositories;
using static SX.WebCore.HtmlHelpers.SxExtantions;
using SX.WebCore.MvcControllers;
using static SX.WebCore.Enums;

namespace GE.WebAdmin.Controllers
{
    public partial class NewsController : SxMaterialsController<News, DbContext>
    {
        public NewsController() : base(ModelCoreType.News)
        {
            if (Repo == null)
                Repo = new RepoNews();
        }

        private static int _pageSize = 20;

        [HttpGet]
        public virtual ViewResult Index(int page = 1)
        {
            var defaultOrder = new SxOrder { FieldName = "DateCreate", Direction = SortDirection.Desc };
            var filter = new SxFilter(page, _pageSize) { Order = defaultOrder };
            var totalItems = (Repo as RepoNews).FilterCount(filter);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.Filter = filter;

            var viewModel = (Repo as RepoNews).QueryForAdmin(filter);
            return View(viewModel);
        }

        [HttpPost]
        public virtual PartialViewResult Index(VMNews filterModel, SxOrder order, int page = 1)
        {
            var defaultOrder = new SxOrder { FieldName = "DateCreate", Direction = SortDirection.Desc };
            var filter = new SxFilter(page, _pageSize) { Order = order == null || order.Direction == SortDirection.Unknown ? defaultOrder : order, WhereExpressionObject = filterModel };
            var totalItems = (Repo as RepoNews).FilterCount(filter);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.Filter = filter;

            var viewModel = (Repo as RepoNews).QueryForAdmin(filter);

            return PartialView("_GridView", viewModel);
        }

        [HttpGet]
        public virtual ViewResult Edit(int? id)
        {
            var model = id.HasValue ? Repo.GetByKey(id, ModelCoreType.News) : new News { ModelCoreType = Enums.ModelCoreType.News };
            var viewModel = Mapper.Map<News, VMEditNews>(model);
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
        public virtual ActionResult Edit(VMEditNews model)
        {
            if (model.Title != null && string.IsNullOrEmpty(model.TitleUrl))
            {
                var titleUrl = Url.SeoFriendlyUrl(model.Title);
                var existModel = (Repo as RepoNews).GetByTitleUrl(titleUrl);
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
                var redactModel = Mapper.Map<VMEditNews, News>(model);
                
                News newModel = null;

                if (isNew)
                {
                    redactModel.UserId = User.Identity.GetUserId();
                    newModel = Repo.Create(redactModel);
                }
                else
                {
                    if (model.TitleUrl != model.OldTitleUrl)
                    {
                        var existModel = (Repo as RepoNews).GetByTitleUrl(model.TitleUrl);
                        if (existModel != null)
                        {
                            ViewBag.HasError = true;
                            model = isNew ? model : getPreparedNews(model);
                            ModelState.AddModelError("TitleUrl", "Строковый ключ должен быть уникальным");
                            return View(model);
                        }
                    }

                    newModel = Repo.Update(redactModel, true, "Title", "TitleUrl", "Show", "GameId", "FrontPictureId", "Html", "DateOfPublication", "UserId", "Foreword",  "ShowFrontPictureOnDetailPage", "CategoryId", "IsTop");
                }

                return RedirectToAction("index");
            }
            else
            {
                ViewBag.HasError = true;
                ViewBag.ModelCoreType = Enums.ModelCoreType.News;
                model = isNew ? model : getPreparedNews(model);
                return View(model);
            }
        }

        private VMEditNews getPreparedNews(VMEditNews newModel)
        {
            var old = Repo.GetByKey(newModel.Id, newModel.ModelCoreType);
            newModel.Category = old.Category;
            newModel.Game = old.Game;
            return newModel;
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult Delete(VMEditNews model)
        {
            var repoSeoInfo = new SxRepoSeoTags<DbContext>();
            repoSeoInfo.DeleteMaterialSeoInfo(model.Id, model.ModelCoreType);

            Repo.Delete(model.Id, model.ModelCoreType);
            return RedirectToAction("index");
        }
    }
}