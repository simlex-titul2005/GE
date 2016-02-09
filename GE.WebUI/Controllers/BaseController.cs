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
    public abstract class BaseController : Controller
    {
        private IMapper _mapper;
        private SxDbRepository<Guid, SxRequest, DbContext> _repo;
        public BaseController()
        {
            _mapper = MvcApplication.MapperConfiguration.CreateMapper();
            _repo = new RepoRequest();
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
        }

        private static void writeRequestInfo(SxDbRepository<Guid, SxRequest, DbContext> repo, HttpRequestBase request)
        {
            Task.Run(() =>
            {
                repo.Create(new SxRequest(request));
            });
        }
    }
}