using GE.WebAdmin.Models;
using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using SX.WebCore;
using System.Web.Mvc;
using GE.WebAdmin.Extantions.Repositories;
using Microsoft.AspNet.Identity;
using static SX.WebCore.HtmlHelpers.SxExtantions;
using SX.WebCore.MvcControllers;
using static SX.WebCore.Enums;
using SX.WebCore.ViewModels;

namespace GE.WebAdmin.Controllers
{
    public sealed class HumorController : SxMaterialsController<SxHumor, SxVMMaterial, DbContext>
    {
        public HumorController():base(ModelCoreType.Humor)
        {
            if (Repo == null)
                Repo = new RepoHumor<SxVMMaterial>();
        }

        private static int _pageSize = 20;

        [HttpGet]
        public ViewResult Index(int page = 1)
        {
            var defaultOrder = new SxOrder { FieldName = "DateCreate", Direction = SortDirection.Desc };
            var filter = new SxFilter(page, _pageSize) { Order = defaultOrder };
            var totalItems = (Repo as RepoHumor<SxVMMaterial>).FilterCount(filter);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.Filter = filter;

            var viewModel = (Repo as RepoHumor<SxVMMaterial>).QueryForAdmin(filter);
            return View(viewModel);
        }

        [HttpPost]
        public PartialViewResult Index(VMArticle filterModel, SxOrder order, int page = 1)
        {
            var defaultOrder = new SxOrder { FieldName = "DateCreate", Direction = SortDirection.Desc };
            var filter = new SxFilter(page, _pageSize) { Order = order == null || order.Direction == SortDirection.Unknown ? defaultOrder : order, WhereExpressionObject = filterModel };
            var totalItems = (Repo as RepoHumor<SxVMMaterial>).FilterCount(filter);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.Filter = filter;

            var viewModel = (Repo as RepoHumor<SxVMMaterial>).QueryForAdmin(filter);

            return PartialView("_GridView", viewModel);
        }

        [HttpGet]
        public ViewResult Edit(int? id)
        {
            var model = id.HasValue ? Repo.GetByKey(id, ModelCoreType.Humor) : new SxHumor();
            var viewModel = Mapper.Map<SxHumor, VMEditHumor>(model);
            viewModel.OldTitleUrl = viewModel.TitleUrl;
            if (model.FrontPictureId.HasValue)
                ViewData["FrontPictureIdCaption"] = model.FrontPicture.Caption;
            if (model.Category != null)
                ViewBag.MaterialCategoryTitle = model.Category.Title;

            ViewBag.MaterialId = id;
            ViewBag.ModelCoreType = model.ModelCoreType;

            return View(viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(VMEditHumor model)
        {
            if (model.Title != null && string.IsNullOrEmpty(model.TitleUrl))
            {
                var titleUrl = Url.SeoFriendlyUrl(model.Title);
                var existModel = (Repo as RepoHumor<SxVMMaterial>).GetByTitleUrl(titleUrl);
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
                var redactModel = Mapper.Map<VMEditHumor, SxHumor>(model);

                SxHumor newModel = null;

                if (isNew)
                {
                    redactModel.UserId = User.Identity.GetUserId();
                    newModel = Repo.Create(redactModel);
                }
                else
                {
                    if (model.TitleUrl != model.OldTitleUrl)
                    {
                        var existModel = (Repo as RepoHumor<SxVMMaterial>).GetByTitleUrl(model.TitleUrl);
                        if (existModel != null)
                        {
                            ViewBag.HasError = true;
                            model = isNew ? model : getPreparedArticle(model);
                            ModelState.AddModelError("TitleUrl", "Строковый ключ должен быть уникальным");
                            return View(model);
                        }
                    }

                    newModel = Repo.Update(redactModel, true, "Title", "TitleUrl", "Show", "GameId", "FrontPictureId", "Html", "DateOfPublication", "UserId", "Foreword", "ShowFrontPictureOnDetailPage", "CategoryId", "IsTop", "SourceUrl");
                }

                return RedirectToAction("index");
            }
            else
            {
                ViewBag.HasError = true;
                ViewBag.ModelCoreType = ModelCoreType.Humor;
                model = isNew ? model : getPreparedArticle(model);
                return View(model);
            }

        }

        private VMEditHumor getPreparedArticle(VMEditHumor newModel)
        {
            var old = Repo.GetByKey(newModel.Id, newModel.ModelCoreType);
            newModel.Category = old.Category;
            return newModel;
        }
    }
}