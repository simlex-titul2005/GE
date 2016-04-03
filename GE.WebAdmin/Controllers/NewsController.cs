using GE.WebAdmin.Models;
using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using SX.WebCore;
using SX.WebCore.Abstract;
using SX.WebCore.HtmlHelpers;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GE.WebAdmin.Extantions.Repositories;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

namespace GE.WebAdmin.Controllers
{
    public partial class NewsController : BaseController
    {
        SxDbRepository<int, News, DbContext> _repo;
        SxDbRepository<int, SxSeoInfo, DbContext> _repoSeoInfo;
        public NewsController()
        {
            _repo = new RepoNews();
            _repoSeoInfo = new RepoSeoInfo();
        }

        private static int _pageSize = 20;

        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ViewResult Index(int page = 1)
        {
            var filter = new WebCoreExtantions.Filter { PageSize = _pageSize, SkipCount = (page - 1) * _pageSize };
            var list = (_repo as RepoNews).QueryForAdmin(filter);

            ViewData["Page"] = page;
            ViewData["PageSize"] = _pageSize;
            ViewData["RowsCount"] = (_repo as RepoNews).FilterCount(filter);

            return View(list);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual PartialViewResult Index(VMNews filterModel, IDictionary<string, SxExtantions.SortDirection> order, int page = 1)
        {
            ViewBag.Filter = filterModel;
            ViewBag.Order = order;

            var filter = new WebCoreExtantions.Filter { PageSize = _pageSize, SkipCount = (page - 1) * _pageSize, Orders = order, WhereExpressionObject = filterModel };
            var viewModel = (_repo as RepoNews).QueryForAdmin(filter);

            ViewData["Page"] = page;
            ViewData["PageSize"] = _pageSize;
            ViewData["RowsCount"] = (_repo as RepoNews).FilterCount(filter);

            return PartialView("_GridView", viewModel);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ViewResult Edit(int? id)
        {
            var model = id.HasValue ? _repo.GetByKey(id, Enums.ModelCoreType.News) : new News { ModelCoreType = Enums.ModelCoreType.News };
            var viewModel = Mapper.Map<News, VMEditNews>(model);
            viewModel.OldTitleUrl = viewModel.TitleUrl;
            return View(viewModel);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Edit(VMEditNews model)
        {
            if (string.IsNullOrEmpty(model.TitleUrl))
            {
                var titleUrl = SX.WebCore.StringHelper.SeoFriendlyUrl(model.Title);
                var existModel = (_repo as RepoNews).GetByTitleUrl(titleUrl);
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
                            ModelState.AddModelError("TitleUrl", "Строковый ключ должен быть уникальным");
                            return View(model);
                        }
                    }

                    newModel = _repo.Update(redactModel, "Title", "TitleUrl", "Show", "GameId", "FrontPictureId", "Html", "DateOfPublication", "UserId", "Foreword",  "ShowFrontPictureOnDetailPage");
                }

                return RedirectToAction(MVC.News.Index());
            }
            else
                return View(model);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Delete(VMEditNews model)
        {
            Task.Run(() =>
            {
                (_repoSeoInfo as RepoSeoInfo).DeleteMaterialSeoInfo(model.Id, model.ModelCoreType);
            });

            _repo.Delete(model.Id, model.ModelCoreType);
            return RedirectToAction(MVC.News.Index());
        }
    }
}