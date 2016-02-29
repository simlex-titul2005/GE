using AutoMapper;
using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using SX.WebCore;
using SX.WebCore.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace GE.WebUI.Controllers
{
    public abstract partial class BaseController : Controller
    {
        private IMapper _mapper;
        private SxDbRepository<Guid, SxRequest, DbContext> _repo;
        private SxDbRepository<int, SxSeoInfo, DbContext> _repoSeoInfo;
        public BaseController()
        {
            _mapper = MvcApplication.MapperConfiguration.CreateMapper();
            _repo = new RepoRequest();
            _repoSeoInfo = new RepoSeoInfo();
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
            var url = filterContext.RequestContext.HttpContext.Request.RawUrl;
            var seoInfo = (_repoSeoInfo as RepoSeoInfo).GetByRawUrl(url);
            if(seoInfo!=null)
            {
                ViewBag.Title = seoInfo.SeoTitle;
                ViewBag.Description = seoInfo.SeoDescription;
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