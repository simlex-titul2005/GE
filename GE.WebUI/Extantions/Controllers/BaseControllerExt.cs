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

namespace GE.WebUI.Extantions.Controllers
{
    public static class BaseControllerExt
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

        public static SxRedirect GetRedirectUrl(this Controller controller, MemoryCache redirectsCache)
        {
            var rawUrl=controller.Request.RawUrl.ToLowerInvariant();
            var redirect = (SxRedirect)redirectsCache.Get(rawUrl);
            if (redirect == null)
            {
                var repo=new RepoRedirect();
                redirect = repo.GetRedirectUrl(rawUrl);
                redirect = redirect ?? new SxRedirect { OldUrl = rawUrl, NewUrl = null };
                redirectsCache.Add(rawUrl, redirect, _defaultPolicy);
            }

            return redirect;
        }

        public static void WriteRequestInfo(this Controller controller)
        {
            var request = controller.Request;
            if (request.IsLocal) return;

            var sessionId = request.RequestContext.HttpContext.Session.SessionID;
            Task.Run(() =>
            {
                var requestInstance = new SxRequest {
                    Browser = request.Browser != null ? request.Browser.Browser : null,
                    ClientIP = request.ServerVariables["REMOTE_ADDR"],
                    RawUrl = request.RawUrl.ToLowerInvariant(),
                    RequestType=request.RequestType,
                    UrlRef=request.UrlReferrer!=null?request.UrlReferrer.ToString().ToLowerInvariant():null,
                    SessionId=request.RequestContext.HttpContext.Session.SessionID,
                    UserAgent=request.UserAgent
                };
                var repo = new RepoRequest();
                repo.Create(requestInstance);
            });
        }

        public static void WriteSeoInfo(this Controller controller, MemoryCache seoInfoCache, IMapper mapper)
        {
            var request = controller.Request;
            var rawUrl = request.RawUrl.ToLowerInvariant();
            var seoInfo = (SiteSeoInfo)seoInfoCache.Get(rawUrl);
            if (seoInfo == null)
            {
                var repoSeoInfo = new RepoSeoInfo();
                var seo = (repoSeoInfo as RepoSeoInfo).GetByRawUrl(rawUrl);
                if (seo != null)
                {
                    seoInfo = mapper.Map<SxSeoInfo, SiteSeoInfo>(seo);
                    seoInfo.IsEmpty = false;
                }
                else
                {
                    seoInfo = new SiteSeoInfo { IsEmpty = true };
                }
                seoInfoCache.Add(rawUrl, seoInfo, _defaultPolicy);
            }

            if (seoInfo != null && !seoInfo.IsEmpty)
            {
                controller.ViewBag.Title = seoInfo.SeoTitle;
                controller.ViewBag.Description = seoInfo.SeoDescription;
                controller.ViewBag.Keywords = seoInfo.KeywordsString;
                controller.ViewBag.H1 = seoInfo.H1;
            }
        }

        public static void WriteBreadcrumbs(this Controller controller)
        {
            var request = controller.Request;
            var routes = request.RequestContext.RouteData.Values;
            var controllerName = routes["controller"].ToString().ToLowerInvariant();
            var actionName = routes["action"].ToString().ToLowerInvariant();
            var gameName = routes["game"] != null && !string.IsNullOrEmpty(routes["game"].ToString()) ? routes["game"].ToString() : null;

            var breadcrumbs = new List<VMBreadcrumb>();
            breadcrumbs.Add(new VMBreadcrumb { Title = "Главная", Url = "/" });
            if (controllerName == "articles")
            {
                if (actionName == "list")
                {
                    breadcrumbs.Add(new VMBreadcrumb { Title = "Статьи", Url = controller.Url.Action(MVC.Articles.List()) });
                    if (gameName != null)
                        breadcrumbs.Add(new VMBreadcrumb { Title = gameName });
                }
                else if (actionName == "details")
                {
                    breadcrumbs.Add(new VMBreadcrumb { Title = "Статьи", Url = controller.Url.Action(MVC.Articles.List()) });
                }
            }
            else if (controllerName == "news")
            {
                if (actionName == "list")
                {
                    breadcrumbs.Add(new VMBreadcrumb { Title = "Новости", Url = controller.Url.Action(MVC.News.List()) });
                    if (gameName != null)
                        breadcrumbs.Add(new VMBreadcrumb { Title = gameName });
                }
                else if (actionName == "details")
                {
                    breadcrumbs.Add(new VMBreadcrumb { Title = "Новости", Url = controller.Url.Action(MVC.News.List()) });
                }
            }

            controller.ViewBag.Breadcrumbs = breadcrumbs.ToArray();
        }
    }
}