using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using SX.WebCore;
using SX.WebCore.Abstract;
using System.Web.Mvc;

namespace GE.WebUI.Controllers
{
    public partial class LikesController : BaseController
    {
        private SX.WebCore.Repositories.RepoLike<DbContext> _repo;
        public LikesController()
        {
            _repo = new SX.WebCore.Repositories.RepoLike<DbContext>();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual ActionResult Like(SxLike like)
        {
            if (!Request.IsAjaxRequest()) return new HttpStatusCodeResult(404);
            var data = _repo.CreateLike(like);
            return Json(data);
        }
    }
}