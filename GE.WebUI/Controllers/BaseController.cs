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
        private static MemoryCache _seoInfoCache;
        public BaseController()
        {
            _mapper = MvcApplication.MapperConfiguration.CreateMapper();
            _repo = new RepoRequest();
            _repoSeoInfo = new RepoSeoInfo();
            if (_seoInfoCache == null)
                _seoInfoCache = new MemoryCache("SEO_INFO_CACHE");
        }

        public BaseController(int displayWidth)
        {
            
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

            writeRequestInfo(_repo, Request);

            //seo
            var rawUrl = filterContext.RequestContext.HttpContext.Request.RawUrl;

            var seoInfo = (SiteSeoInfo)_seoInfoCache.Get(rawUrl);
            if(seoInfo==null)
            {
                var seo=(_repoSeoInfo as RepoSeoInfo).GetByRawUrl(rawUrl);
                if(seo!=null)
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

        private static void writeRequestInfo(SxDbRepository<Guid, SxRequest, DbContext> repo, HttpRequestBase request)
        {
            var sessionId = request.RequestContext.HttpContext.Session.SessionID;
            Task.Run(() =>
            {
                var r = new SxRequest(request, sessionId);
                if ((repo as RepoRequest).Exists(r)) return;

                repo.Create(new SxRequest(request, sessionId));
            });
        }
    }
}