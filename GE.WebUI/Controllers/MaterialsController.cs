﻿using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Abstract;
using GE.WebCoreExtantions.Repositories;
using GE.WebUI.Models;
using GE.WebUI.Models.Abstract;
using SX.WebCore;
using SX.WebCore.Abstract;
using SX.WebCore.HtmlHelpers;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using GE.WebUI.Extantions.Repositories;
using static SX.WebCore.Enums;
using System.Globalization;
using SX.WebCore.Attrubutes;
using SX.WebCore.Repositories;
using SX.WebCore.MvcApplication;
using SX.WebCore.ViewModels;

namespace GE.WebUI.Controllers
{
    public abstract class MaterialsController<TKey, TModel> : BaseController where TModel : SxDbModel<TKey>, IHasGame
    {
        private SxDbRepository<TKey, TModel, DbContext> _repo;
        private SxDbRepository<int, Game, DbContext> _repoGame;
        protected MaterialsController() { }
        private ModelCoreType _modelCoreType;
        protected MaterialsController(ModelCoreType modelCoreType)
        {
            _repoGame = new RepoGame();
            _modelCoreType = modelCoreType;
            switch (_modelCoreType)
            {
                case Enums.ModelCoreType.Article:
                    _repo = new RepoArticle() as SxDbRepository<TKey, TModel, DbContext>;
                    break;
                case Enums.ModelCoreType.News:
                    _repo = new RepoNews() as SxDbRepository<TKey, TModel, DbContext>;
                    break;
                default:
                    throw new NotSupportedException("Не определен репозиторий");
            }
        }
        protected SxDbRepository<TKey, TModel, DbContext> Repository
        {
            get
            {
                return _repo;
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ViewResult List(SxFilter filter)
        {
            var routeDataValues = Request.RequestContext.RouteData.Values;
            var gameTitle = (string)routeDataValues["gameTitle"];
            ViewBag.GameTitle = gameTitle;
            var page = routeDataValues["page"]!=null? Convert.ToInt32(routeDataValues["page"]):1;
            filter.PagerInfo.Page = page;
            filter.AddintionalInfo = new object[] { gameTitle };

            if (gameTitle != null)
            {
                var existGame = (_repoGame as RepoGame).ExistGame(gameTitle);
                if(!existGame)
                {
                    Response.StatusCode = 404;
                    Response.Clear();
                    return null;
                }
            }

            var pageSize = 10;
            switch (_modelCoreType)
            {
                case ModelCoreType.Article:
                    pageSize = 9;
                    break;
                case ModelCoreType.News:
                    pageSize = 10;
                    break;
            }

            var viewModel = new SxExtantions.SxPagedCollection<TModel>();
            filter.PagerInfo.PageSize = pageSize;
            var tag = Request.QueryString.Get("tag");
            if (!string.IsNullOrEmpty(tag))
            {
                filter.Tag = tag;
                ViewBag.Tag = tag;
            }

            viewModel.Collection = _repo.Query(filter).ToArray();
            viewModel.PagerInfo = new SxExtantions.SxPagerInfo(filter.PagerInfo.Page, pageSize)
            {
                PagerSize = 3,
                TotalItems = _repo.Count(filter)
            };

            return View(viewModel);
        }

#if !DEBUG
        [OutputCache(Duration =900, VaryByParam ="MaterialId;ModelCoreType")]
#endif
        [AcceptVerbs(HttpVerbs.Get)]
        [ChildActionOnly]
        public virtual PartialViewResult LikeMaterials(SxFilter filter, int amount=10)
        {
            dynamic[] viewModel = null;
            switch(filter.ModelCoreType)
            {
                case ModelCoreType.Article:
                    viewModel = (_repo as RepoArticle).GetLikeMaterial(filter, amount);
                    break;
                case ModelCoreType.News:
                    viewModel = (_repo as RepoNews).GetLikeMaterial(filter, amount);
                    break;
            }
            ViewBag.LikeMatTitle = getLikeMatTitle(filter.ModelCoreType);

            return PartialView("~/views/Shared/_LikeMaterial.cshtml", viewModel);
        }

        private static string getLikeMatTitle(ModelCoreType mct)
        {
            switch (mct)
            {
                case ModelCoreType.News:
                    return "Эту новость хорошо дополняют";
                case ModelCoreType.Article:
                    return "Эту статью хорошо дополняют";
                default:
                    return "Похожие материалы";
            }
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ActionResult Details(int year, string month, string day, string titleUrl)
        {
            VMDetailMaterial model = null;
            switch (_modelCoreType)
            {
                case ModelCoreType.Article:
                    model = (_repo as RepoArticle).GetByTitleUrl(year, month, day,titleUrl);
                    break;
                case ModelCoreType.News:
                    model = (_repo as RepoNews).GetByTitleUrl(year, month, day, titleUrl);
                    break;
            }
            if (model == null)
            {
                return new HttpStatusCodeResult(404);
            }
            else
            {
                var html=SxBBCodeParser.GetHtml(model.Html);
                html = SxBBCodeParser.ReplaceBanners(html, SxApplication<DbContext>.GetBanners(), (b)=>Url.Action("Picture", "Pictures", new { id=b.PictureId}));
                html = SxBBCodeParser.ReplaceVideo(html, model.Videos);
                model.Html = html;

                var seoInfoRepo = new SxRepoSeoTags<DbContext>();
                var matSeoInfo = Mapper.Map<SxSeoTags, SxVMSeoTags>(seoInfoRepo.GetSeoTags(model.Id, model.ModelCoreType));

                ViewBag.Title = ViewBag.Title ?? (matSeoInfo!=null? matSeoInfo.SeoTitle:null) ?? model.Title;
                ViewBag.Description = ViewBag.Description ?? (matSeoInfo != null ? matSeoInfo.SeoDescription : null) ?? model.Foreword;
                ViewBag.Keywords = ViewBag.Keywords ?? (matSeoInfo != null ? matSeoInfo.KeywordsString : null);
                ViewBag.H1= ViewBag.H1 ?? (matSeoInfo != null ? matSeoInfo.H1 : null) ?? model.Title;

                CultureInfo ci = new CultureInfo("en-US");
                ViewBag.LastModified = model.DateUpdate.ToString("ddd, dd MMM yyyy HH:mm:ss 'GMT'", ci);

                if (model.GameTitleUrl != null)
                    ViewBag.GameName = model.GameTitleUrl.ToLowerInvariant();

                var viewsCount = model.ViewsCount;

                var breadcrumbs = (VMBreadcrumb[])ViewBag.Breadcrumbs;
                if(breadcrumbs!=null)
                {
                    var bc=breadcrumbs.ToList();
                    bc.Add(new VMBreadcrumb{Title=ViewBag.Title});
                    ViewBag.Breadcrumbs = bc.ToArray();
                }

                //update views count
                if (!Request.IsLocal)
                {
                    Task.Run(() =>
                    {
                        switch (_modelCoreType)
                        {
                            case ModelCoreType.Article:
                                (_repo as RepoArticle).AddUserView(model.Id, model.ModelCoreType);
                                break;
                            case ModelCoreType.News:
                                (_repo as RepoNews).AddUserView(model.Id, model.ModelCoreType);
                                break;
                        }
                    });
                }

                model.ViewsCount = viewsCount + 1;

                return View(model);
            }
        }

#if !DEBUG
                [OutputCache(Duration =900, VaryByParam ="mid;mct;dir;amount")]
#endif
        [HttpGet, NotLogRequest]
        public virtual PartialViewResult ByDateMaterial(int mid, ModelCoreType mct, bool dir=false, int amount=3)
        {
            ViewBag.ModelCoreType = mct;
            VMLastMaterial[] data = null;
            switch(mct)
            {
                case ModelCoreType.Article:
                    data = new RepoArticle().GetByDateMaterial(mid, mct, dir, amount);
                    break;
                case ModelCoreType.News:
                    data = new RepoNews().GetByDateMaterial(mid, mct, dir, amount);
                    break;
            }

            return PartialView("~/views/shared/_bydatematerial.cshtml", data);
        }


#if !DEBUG
        [OutputCache(Duration =900, VaryByParam ="mct;mid;amount")]
#endif
        [HttpGet, ChildActionOnly]
        public virtual PartialViewResult Popular(ModelCoreType mct, int mid, int amount=4)
        {
            VMLastMaterial[] data = null;
            switch (mct)
            {
                case ModelCoreType.Article:
                    data = new RepoArticle().GetPopular(mct, mid, amount); ;
                    break;
                case ModelCoreType.News:
                    data = new RepoNews().GetPopular(mct, mid, amount);
                    break;
            }
            ViewData["ModelCoreType"] = mct;

            return PartialView("~/views/shared/_popularmaterials.cshtml", data);
        }
    }
}