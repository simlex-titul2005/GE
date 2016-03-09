using AutoMapper;
using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using GE.WebUI.Models;
using SX.WebCore;
using SX.WebCore.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace GE.WebUI.Controllers
{
    public abstract partial class BaseController : Controller
    {
        private static CacheItemPolicy _defaultPolicy
        {
            get
            {
                return new CacheItemPolicy
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(15)
                };
            }
        }

        private IMapper _mapper;
        private SxDbRepository<Guid, SxRequest, DbContext> _repo;
        private SxDbRepository<int, SxSeoInfo, DbContext> _repoSeoInfo;
        private SxDbRepository<Guid, SxRedirect, DbContext> _repoRedirect;
        private static MemoryCache _seoInfoCache;
        public BaseController()
        {
            _mapper = MvcApplication.MapperConfiguration.CreateMapper();
            _repo = new RepoRequest();
            _repoSeoInfo = new RepoSeoInfo();
            _repoRedirect=new RepoRedirect();
            if (_seoInfoCache == null)
                _seoInfoCache = new MemoryCache("SEO_INFO_CACHE");
        }

        protected IMapper Mapper
        {
            get
            {
                return _mapper;
            }
        }
        
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (filterContext.IsChildAction) return;

            var newUrl = getNewUrl(Request.RawUrl);
            if (newUrl != null)
            {
                filterContext.Result = new RedirectResult(newUrl);
                return;
            }

            if (!Request.IsLocal)
                writeRequestInfo(_repo, Request);

            writeSeoInfo(filterContext);

            writeBreadcrumbs(filterContext);
        }

        private string getNewUrl(string oldUrl)
        {
            var newUrl = (_repoRedirect as RepoRedirect).GetNewUrl(oldUrl);
            return newUrl;
        }
        private static void writeRequestInfo(SxDbRepository<Guid, SxRequest, DbContext> repo, HttpRequestBase request)
        {
            var sessionId = request.RequestContext.HttpContext.Session.SessionID;
            var urlRef = request.UrlReferrer != null ? request.UrlReferrer.AbsolutePath.ToLower() : null;
            var rawUrl = request.RawUrl.ToLower();
            if (Equals(urlRef, rawUrl)) return;

            Task.Run(() =>
            {
                repo.Create(new SxRequest(request, sessionId));
            });
        }
        private void writeSeoInfo(ActionExecutingContext filterContext)
        {
            var rawUrl = filterContext.RequestContext.HttpContext.Request.RawUrl;

            var seoInfo = (SiteSeoInfo)_seoInfoCache.Get(rawUrl);
            if (seoInfo == null)
            {
                var seo = (_repoSeoInfo as RepoSeoInfo).GetByRawUrl(rawUrl);
                if (seo != null)
                {
                    seoInfo = _mapper.Map<SxSeoInfo, SiteSeoInfo>(seo);
                    seoInfo.IsEmpty = false;
                }
                else
                {
                    seoInfo = new SiteSeoInfo { IsEmpty = true };
                }
                _seoInfoCache.Add(rawUrl, seoInfo, _defaultPolicy);
            }

            if (seoInfo != null && !seoInfo.IsEmpty)
            {
                ViewBag.Title = seoInfo.SeoTitle;
                ViewBag.Description = seoInfo.SeoDescription;
                ViewBag.Keywords = seoInfo.KeywordsString;
            }
        }
        private void writeBreadcrumbs(ActionExecutingContext filterContext)
        {
            var routes = filterContext.RouteData.Values;
            var controllerName = routes["controller"].ToString();
            var actionName = routes["action"].ToString();
            var gameName = routes["game"] != null && !string.IsNullOrEmpty(routes["game"].ToString()) ? routes["game"].ToString() : null;

            var breadcrumbs = new List<VMBreadcrumb>();
            breadcrumbs.Add(new VMBreadcrumb { Title = "Главная", Url = "/" });
            if (controllerName == "articles")
            {
                if (actionName == "list")
                {
                    breadcrumbs.Add(new VMBreadcrumb { Title = "Статьи", Url = Url.Action(MVC.Articles.List(game: "")) });
                    if (gameName!=null)
                        breadcrumbs.Add(new VMBreadcrumb { Title = gameName });
                }
                else if (actionName == "details")
                {
                    breadcrumbs.Add(new VMBreadcrumb { Title = "Статьи", Url = Url.Action(MVC.Articles.List(game: "")) });
                }
            }
            else if (controllerName == "news")
            {
                if (actionName == "list")
                {
                    breadcrumbs.Add(new VMBreadcrumb { Title = "Новости", Url = Url.Action(MVC.News.List(game: "")) });
                    if (gameName != null)
                        breadcrumbs.Add(new VMBreadcrumb { Title = gameName });
                }
                else if (actionName == "details")
                {
                    breadcrumbs.Add(new VMBreadcrumb { Title = "Новости", Url = Url.Action(MVC.News.List(game: "")) });
                }
            }

            ViewBag.Breadcrumbs = breadcrumbs.ToArray();
        }
    }
}