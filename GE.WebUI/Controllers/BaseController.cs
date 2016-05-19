using AutoMapper;
using System.Runtime.Caching;
using System.Web.Mvc;
using GE.WebUI.Extantions.Controllers;
using System.Linq;
using SX.WebCore.Attrubutes;

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
            var urlRef = Request.UrlReferrer;
            if(urlRef!=null)
            {
                if (this.GetBannedUrls().Contains(urlRef.ToString()))
                    filterContext.Result = new HttpStatusCodeResult(403);
            }

            base.OnActionExecuting(filterContext);

            var notLogRequest = filterContext.ActionDescriptor.GetCustomAttributes(true).FirstOrDefault(x => x.GetType() == typeof(NotLogRequestAttribute)) != null;

            if (filterContext.IsChildAction || notLogRequest) return;

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