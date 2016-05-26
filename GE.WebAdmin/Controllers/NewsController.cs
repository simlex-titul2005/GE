using GE.WebAdmin.Models;
using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using SX.WebCore;
using SX.WebCore.Abstract;
using SX.WebCore.HtmlHelpers;
using System.Collections.Generic;
using System.Web.Mvc;
using GE.WebAdmin.Extantions.Repositories;
using Microsoft.AspNet.Identity;

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

        [HttpGet]
        public virtual ViewResult Index(int page = 1)
        {
            var filter = new WebCoreExtantions.Filter(page, _pageSize);
            var totalItems = (_repo as RepoNews).FilterCount(filter);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.PagerInfo = filter.PagerInfo;

            var viewModel = (_repo as RepoNews).QueryForAdmin(filter);
            return View(viewModel);
        }

        [HttpPost]
        public virtual PartialViewResult Index(VMNews filterModel, IDictionary<string, SxExtantions.SortDirection> order, int page = 1)
        {
            ViewBag.Filter = filterModel;
            ViewBag.Order = order;

            var filter = new WebCoreExtantions.Filter(page, _pageSize) { Orders = order, WhereExpressionObject = filterModel };
            var totalItems = (_repo as RepoNews).FilterCount(filter);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.PagerInfo = filter.PagerInfo;

            var viewModel = (_repo as RepoNews).QueryForAdmin(filter);

            return PartialView("_GridView", viewModel);
        }

        [HttpGet]
        public virtual ViewResult Edit(int? id)
        {
            var model = id.HasValue ? _repo.GetByKey(id, Enums.ModelCoreType.News) : new News { ModelCoreType = Enums.ModelCoreType.News };
            var viewModel = Mapper.Map<News, VMEditNews>(model);
            viewModel.OldTitleUrl = viewModel.TitleUrl;
            if (model.FrontPictureId.HasValue)
                ViewBag.PictureCaption = model.FrontPicture.Caption;
            return View(viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult Edit(VMEditNews model)
        {
            if (string.IsNullOrEmpty(model.TitleUrl))
            {
                var titleUrl = Url.SeoFriendlyUrl(model.Title);
                var existModel = (_repo as RepoNews).GetByTitleUrl(titleUrl);
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
                    newModel = _repo.Create(redactModel);
                }
                else
                {
                    if (model.TitleUrl != model.OldTitleUrl)
                    {
                        var existModel = (_repo as RepoNews).GetByTitleUrl(model.TitleUrl);
                        if (existModel != null)
                        {
                            ViewBag.HasError = true;
                            model = isNew ? model : getPreparedNews(model);
                            ModelState.AddModelError("TitleUrl", "Строковый ключ должен быть уникальным");
                            return View(model);
                        }
                    }

                    newModel = _repo.Update(redactModel, true, "Title", "TitleUrl", "Show", "GameId", "FrontPictureId", "Html", "DateOfPublication", "UserId", "Foreword",  "ShowFrontPictureOnDetailPage", "CategoryId", "IsTop");
                }

                return RedirectToAction(MVC.News.Index());
            }
            else
            {
                ViewBag.HasError = true;
                model = isNew ? model : getPreparedNews(model);
                return View(model);
            }
        }

        private VMEditNews getPreparedNews(VMEditNews newModel)
        {
            var old = _repo.GetByKey(newModel.Id, newModel.ModelCoreType);
            newModel.Category = old.Category;
            newModel.Game = old.Game;
            return newModel;
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult Delete(VMEditNews model)
        {
            var repoSeoInfo = new RepoSeoInfo();
            repoSeoInfo.DeleteMaterialSeoInfo(model.Id, model.ModelCoreType);

            _repo.Delete(model.Id, model.ModelCoreType);
            return RedirectToAction(MVC.News.Index());
        }
    }
}