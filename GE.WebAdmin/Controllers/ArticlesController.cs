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
    public partial class ArticlesController : BaseController
    {
        SxDbRepository<int, Article, DbContext> _repo;
        SxDbRepository<int, ArticleType, DbContext> _repoArticleTypes;
        SxDbRepository<int, SxSeoInfo, DbContext> _repoSeoInfo;
        public ArticlesController()
        {
            _repo = new RepoArticle();
            _repoArticleTypes = new RepoArticleType();
            _repoSeoInfo = new RepoSeoInfo();
        }

        private static int _pageSize = 20;

        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ViewResult Index(int page = 1)
        {
            var filter=new WebCoreExtantions.Filter { PageSize = _pageSize, SkipCount = (page - 1) * _pageSize };
            var list = (_repo as RepoArticle).QueryForAdmin(filter);

            ViewData["Page"] = page;
            ViewData["PageSize"] = _pageSize;
            ViewData["RowsCount"] = (_repo as RepoArticle).FilterCount(filter);

            return View(list);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual PartialViewResult Index(VMArticle filterModel, IDictionary<string, SxExtantions.SortDirection> order, int page = 1)
        {
            ViewBag.Filter = filterModel;
            ViewBag.Order = order;

            var filter = new WebCoreExtantions.Filter { PageSize = _pageSize, SkipCount = (page - 1) * _pageSize, Orders = order, WhereExpressionObject = filterModel };
            var viewModel = (_repo as RepoArticle).QueryForAdmin(filter);

            ViewData["Page"] = page;
            ViewData["PageSize"] = _pageSize;
            ViewData["RowsCount"] = (_repo as RepoArticle).FilterCount(filter);

            return PartialView("_GridView", viewModel);
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
                var titleUrl = StringHelper.SeoFriendlyUrl(model.Title);
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
                            ModelState.AddModelError("TitleUrl", "Строковый ключ должен быть уникальным");
                            return View(model);
                        }
                    }

                    newModel = _repo.Update(redactModel, "Title", "TitleUrl", "Show", "GameId", "FrontPictureId", "Html", "ArticleTypeName", "ArticleTypeGameId", "DateOfPublication", "UserId", "Foreword", "SeoInfoId", "ShowFrontPictureOnDetailPage");
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
            Task.Run(() =>
            {
                (_repoSeoInfo as RepoSeoInfo).DeleteMaterialSeoInfo(model.Id, model.ModelCoreType);
            });

            _repo.Delete(model.Id, model.ModelCoreType);
            return RedirectToAction(MVC.Articles.Index());
        }
    }
}