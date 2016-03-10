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
using GE.WebUI.Extantions.Controllers;

namespace GE.WebUI.Controllers
{
    public abstract partial class BaseController : Controller
    {
        private static IMapper _mapper;
        private static MemoryCache _seoInfoCache;
        private static MemoryCache _redirectsCache;
        static BaseController()
        {
            if(_mapper==null)
                _mapper = MvcApplication.MapperConfiguration.CreateMapper();

            if (_seoInfoCache == null)
                _seoInfoCache = new MemoryCache("SEO_INFO_CACHE");

            if (_redirectsCache == null)
                _redirectsCache = new MemoryCache("REDIRECTS_CACHE");
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

            var redirect = this.GetRedirectUrl(_redirectsCache);
            if (redirect != null && redirect.NewUrl!=null)
            {
                filterContext.Result = new RedirectResult(redirect.NewUrl);
                return;
            }

            this.WriteRequestInfo();

            this.WriteSeoInfo(_seoInfoCache, _mapper);

            this.WriteBreadcrumbs();
        }
    }
}