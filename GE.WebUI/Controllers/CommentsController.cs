using GE.WebCoreExtantions;
using GE.WebUI.Models;
using SX.WebCore;
using SX.WebCore.Abstract;
using System.Web.Mvc;
using static SX.WebCore.Enums;
using SX.WebCore.Attrubutes;
using System.Linq;
using SX.WebCore.Repositories;
using Microsoft.AspNet.Identity;

namespace GE.WebUI.Controllers
{
    public partial class CommentsController : BaseController
    {
        private SxDbRepository<int, SxComment, DbContext> _repo;
        public CommentsController()
        {
            _repo = new SxRepoComment<DbContext>();
        }

        [HttpGet, NotLogRequest]
        public virtual PartialViewResult List(int mid, ModelCoreType mct, int page=1)
        {
            var viewModel = getResult(mid, mct);
            return PartialView("_List", viewModel);
        }

        [HttpGet]
        public virtual PartialViewResult Edit(int mid, ModelCoreType mct)
        {
            var viewModel = new VMEditComment {
                MaterialId = mid,
                ModelCoreType = mct,
                SecretCode=Session.SessionID
            };

            ViewBag.NewCommentTitle = getNewCommentTitle(mct);

            return PartialView("_Edit", viewModel);
        }
        private static string getNewCommentTitle(ModelCoreType mct)
        {
            switch(mct)
            {
                case ModelCoreType.Article:
                    return "Комментировать статью";
                case ModelCoreType.News:
                    return "Комментировать новость";
                case ModelCoreType.Aphorism:
                    return "Комментировать афоризм";
                default:
                    return "Комментировать материал";
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual PartialViewResult Edit(VMEditComment model)
        {
            var sessionId = Session.SessionID;
            var isAuth = User.Identity.IsAuthenticated;
            if(isAuth)
            {
                ModelState["UserName"].Errors.Clear();
                ModelState["Email"].Errors.Clear();
            }
            if (sessionId==model.SecretCode && ModelState.IsValid)
            {
                if(isAuth)
                    model.UserId = User.Identity.GetUserId();

                var isNew = model.Id == 0;
                var redactModel = Mapper.Map<VMEditComment, SxComment>(model);
                if (isNew)
                    _repo.Create(redactModel);
            }

            var viewModel = getResult(model.MaterialId, model.ModelCoreType);
            return PartialView("_List", viewModel);
        }

        private VMComment[] getResult(int mid, ModelCoreType mct)
        {
            var filter = new SxFilter { MaterialId = mid, ModelCoreType = mct };
            var data = (_repo as SxRepoComment<DbContext>).Query(filter).ToArray();
            var viewModel = data.Select(x => Mapper.Map<SxComment, VMComment>(x)).ToArray();
            return viewModel;
        }
    }
}