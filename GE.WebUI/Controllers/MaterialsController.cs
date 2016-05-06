using GE.WebCoreExtantions;
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

namespace GE.WebUI.Controllers
{
    public abstract partial class MaterialsController<TKey, TModel> : BaseController where TModel : SxDbModel<TKey>, IHasGame
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
        public virtual ViewResult List(WebCoreExtantions.Filter filter)
        {
            ViewBag.GameTitle = filter.GameTitle;
            var page = Request.RequestContext.RouteData.Values["page"]!=null? Convert.ToInt32(Request.RequestContext.RouteData.Values["page"]):1;
            filter.PagerInfo.Page = page;

            if (filter.GameTitle!=null)
            {
                var existGame = (_repoGame as RepoGame).ExistGame(filter.GameTitle);
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
        public virtual PartialViewResult LikeMaterials(WebCoreExtantions.Filter filter, int amount=10)
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

            return PartialView(MVC.Shared.Views._LikeMaterial, viewModel);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ViewResult Details(int year, string month, string day, string titleUrl)
        {
            VMDetailMaterial model = null;
            switch (_modelCoreType)
            {
                case Enums.ModelCoreType.Article:
                    model = (_repo as RepoArticle).GetByTitleUrl(titleUrl);
                    break;
                case Enums.ModelCoreType.News:
                    model = (_repo as RepoNews).GetByTitleUrl(titleUrl);
                    break;
            }
            if (model == null)
            {
                Response.StatusCode = 404;
                Response.Clear();
                return null;
            }
            else
            {
                var html=SxBBCodeParser.GetHtml(model.Html);
                html = SxBBCodeParser.ReplaceBanners(html, SiteBanners.Collection, (b)=>Url.Action(MVC.Pictures.Picture(b.PictureId)));
                html = SxBBCodeParser.ReplaceVideo(html, model.Videos, (v)=>Url.Action(MVC.Pictures.Picture((Guid)v.PictureId)));
                model.Html = html;

                if (!Request.IsLocal)
                {
                    /*ViewBag.VKScript = "<script type=\"text/javascript\" src=\"//vk.com/js/api/openapi.js?121\"></script><script type=\"text/javascript\"> VK.init({ apiId: 5387252, onlyWidgets: true}); </script> ";

                    ViewBag.FBScript = "<div id=\"fb-root\"></div><script>(function(d, s, id) { var js, fjs = d.getElementsByTagName(s)[0]; if (d.getElementById(id)) return; js = d.createElement(s); js.id = id; js.src = \"//connect.facebook.net/ru_RU/sdk.js#xfbml=1&version=v2.5\"; fjs.parentNode.insertBefore(js, fjs); } (document, 'script', 'facebook-jssdk'));</script>";*/
                }

                var seoInfoRepo = new RepoSeoInfo();
                var matSeoInfo = Mapper.Map<SxSeoInfo, VMSeoInfo>(seoInfoRepo.GetSeoInfo(model.Id, model.ModelCoreType));

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
                            case Enums.ModelCoreType.Article:
                                (_repo as RepoArticle).Update(new Article { Id = model.Id, ModelCoreType = model.ModelCoreType, ViewsCount = viewsCount + 1 }, false, "ViewsCount");
                                break;
                            case Enums.ModelCoreType.News:
                                (_repo as RepoNews).Update(new News { Id = model.Id, ModelCoreType = model.ModelCoreType, ViewsCount = viewsCount + 1 }, false, "ViewsCount");
                                break;
                        }
                    });
                }

                model.ViewsCount = viewsCount + 1;

                return View(model);
            }
        }

#if !DEBUG
        [OutputCache(Duration =900, VaryByParam ="mct;date")]
#endif
        [HttpGet]
        public virtual PartialViewResult ByDateMaterial(ModelCoreType mct, DateTime date)
        {
            VMLastMaterial[] data = null;
            switch(mct)
            {
                case ModelCoreType.Article:
                    data = new RepoArticle().GetByDateMaterial(mct, date);
                    break;
                case ModelCoreType.News:
                    data = new RepoNews().GetByDateMaterial(mct, date);
                    break;
            }

            return PartialView(MVC.Shared.Views._ByDateMaterial, data);
        }

#if !DEBUG
        [OutputCache(Duration =900, VaryByParam ="mct;mid;amount")]
#endif
        [HttpGet]
        [ChildActionOnly]
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

            return PartialView(MVC.Shared.Views._PopularMaterials, data);
        }
    }
}