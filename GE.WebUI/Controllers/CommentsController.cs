using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using GE.WebUI.Models;
using SX.WebCore;
using SX.WebCore.Abstract;
using System.Web.Mvc;
using static SX.WebCore.Enums;
using GE.WebUI.Extantions.Repositories;
using Microsoft.AspNet.Identity;

namespace GE.WebUI.Controllers
{
    public partial class CommentsController : BaseController
    {
        private SxDbRepository<int, SxComment, DbContext> _repo;
        public CommentsController()
        {
            _repo = new RepoComment();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public virtual PartialViewResult List(int mid, ModelCoreType mct)
        {
            var viewModel = (_repo as RepoComment).GetComments(mid, mct);
            return PartialView(MVC.Comments.Views._List, viewModel);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public virtual PartialViewResult Create(int mid, ModelCoreType mct)
        {
            var viewModel = new VMEditComment { MaterialId = mid, ModelCoreType = mct, UserName=User.Identity.IsAuthenticated?User.Identity.GetUserName(): null };
            return PartialView(MVC.Comments.Views._Create, viewModel);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Create(VMEditComment model)
        {
            var redactModel = Mapper.Map<VMEditComment, SxComment>(model);
            if (User.Identity.IsAuthenticated)
            {
                redactModel.UserId = User.Identity.GetUserId();
            }

            if(ModelState.IsValid)
            {
                _repo.Create(redactModel);

                var viewModel = (_repo as RepoComment).GetComments(model.MaterialId, model.ModelCoreType);
                return PartialView(MVC.Comments.Views._List, viewModel);
            }
            return PartialView(MVC.Comments.Views._Create, model);
        }
    }
}