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
    public partial class ClicksController : BaseController
    {
        private SxDbRepository<Guid, SxClick, DbContext> _repo;
        public ClicksController()
        {
            _repo = new RepoClick();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual EmptyResult Click(string rawUrl, string target, int clickTypeId)
        {
            if (Request.IsLocal || !Request.IsAjaxRequest()) return null;

            var urlRef = Request.UrlReferrer != null ? Request.UrlReferrer.LocalPath : null;
            var click = new SxClick {
                ClickTypeId = clickTypeId,
                LinkTarget = !string.IsNullOrEmpty(target)?target:null,
                RawUrl=rawUrl,
                UrlRef=urlRef
            };
            Task.Run(() => {
                _repo.Create(click);
            });
            return null;
        }
    }
}