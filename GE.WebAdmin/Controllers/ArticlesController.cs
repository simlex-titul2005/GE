using GE.WebAdmin.Models;
using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using SX.WebCore;
using System.Web.Mvc;
using GE.WebAdmin.Extantions.Repositories;
using Microsoft.AspNet.Identity;
using SX.WebCore.Repositories;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace GE.WebAdmin.Controllers
{
    public partial class ArticlesController : BaseController
    {
        private static RepoArticle _repo;
        public ArticlesController()
        {
            if(_repo==null)
                _repo = new RepoArticle();
        }

        private static int _pageSize = 20;

        [HttpGet]
        public virtual ViewResult Index(int page = 1)
        {
            var defaultOrder = new SxOrder { FieldName = "DateCreate", Direction = SortDirection.Desc };
            var filter = new SxFilter(page, _pageSize) { Order= defaultOrder };
            var totalItems = (_repo as RepoArticle).FilterCount(filter);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.Filter = filter;

            var viewModel = _repo.QueryForAdmin(filter);
            return View(viewModel);
        }

        [HttpPost]
        public virtual PartialViewResult Index(VMArticle filterModel, SxOrder order, int page = 1)
        {
            var defaultOrder = new SxOrder { FieldName = "DateCreate", Direction = SortDirection.Desc };
            var filter = new SxFilter(page, _pageSize) { Order = order==null || order.Direction==SortDirection.Unknown? defaultOrder:order, WhereExpressionObject = filterModel };
            var totalItems = (_repo as RepoArticle).FilterCount(filter);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.Filter = filter;

            var viewModel = (_repo as RepoArticle).QueryForAdmin(filter);

            return PartialView("_GridView", viewModel);
        }

        [HttpGet]
        public virtual ViewResult Edit(int? id)
        {
            var model = id.HasValue ? _repo.GetByKey(id, Enums.ModelCoreType.Article) : new Article { ModelCoreType = Enums.ModelCoreType.Article };
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
        public virtual ActionResult Edit(VMEditArticle model)
        {
            if(model.Title!=null && string.IsNullOrEmpty(model.TitleUrl))
            {
                var titleUrl = Url.SeoFriendlyUrl(model.Title);
                var existModel = (_repo as RepoArticle).GetByTitleUrl(titleUrl);
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
                    newModel = _repo.Create(redactModel);
                }
                else
                {
                    if (model.TitleUrl != model.OldTitleUrl)
                    {
                        var existModel = (_repo as RepoArticle).GetByTitleUrl(model.TitleUrl);
                        if (existModel != null)
                        {
                            ViewBag.HasError = true;
                            model = isNew? model: getPreparedArticle(model);
                            ModelState.AddModelError("TitleUrl", "Строковый ключ должен быть уникальным");
                            return View(model);
                        }
                    }

                    newModel = _repo.Update(redactModel, true, "Title", "TitleUrl", "Show", "GameId", "FrontPictureId", "Html", "DateOfPublication", "UserId", "Foreword", "ShowFrontPictureOnDetailPage", "CategoryId", "IsTop");
                }

                return RedirectToAction("index");
            }
            else
            {
                ViewBag.HasError = true;
                model = isNew?model: getPreparedArticle(model);
                return View(model);
            }
                
        }

        private VMEditArticle getPreparedArticle(VMEditArticle newModel)
        {
            var old=_repo.GetByKey(newModel.Id, newModel.ModelCoreType);
            newModel.Category = old.Category;
            newModel.Game = old.Game;
            return newModel;
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult Delete(VMEditNews model)
        {
            var repoSeoInfo = new SxRepoSeoTags<DbContext>();
            repoSeoInfo.DeleteMaterialSeoInfo(model.Id, model.ModelCoreType);

            _repo.Delete(model.Id, model.ModelCoreType);
            return RedirectToAction("index");
        }
    }
}