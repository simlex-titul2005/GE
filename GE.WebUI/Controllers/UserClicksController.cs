using GE.WebCoreExtantions;
using Microsoft.AspNet.Identity;
using SX.WebCore;
using SX.WebCore.Abstract;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using static SX.WebCore.Enums;
using System.Linq;
using SX.WebCore.Repositories;

namespace GE.WebUI.Controllers
{
    public partial class UserClicksController : BaseController
    {
        private SxDbRepository<Guid, SxUserClick, DbContext> _repo;
        private SxDbRepository<Guid, SxLike, DbContext> _repoLike;
        public UserClicksController()
        {
            _repo = new RepoUserClick<DbContext>();
            _repoLike = new RepoLike<DbContext>();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<int> Click(int mid, ModelCoreType mct, UserClickType uct)
        {
            return await Task.Run(() =>
            {
                var userId = User.Identity.GetUserId();
                var sessionId = Session.SessionID;
                var result = 0;

                switch (uct)
                {
                    case UserClickType.Like:
                        var likeDirection = (Enums.LikeDirection)Enum.Parse(typeof(Enums.LikeDirection), Request.Form["likeDirection"] != null ? Request.Form["likeDirection"].ToString() : "0");
                        result = addlike(mid, mct, sessionId, userId, likeDirection);
                        break;
                    default:
                        result = addUserClick(mid, mct, uct, sessionId, userId);
                        break;
                }

                return result;

            });
        }

        private int addlike(int mid, ModelCoreType mct, string sessionId, string userId, LikeDirection likeDirection)
        {
            var existLike = _repoLike.All.SingleOrDefault(x => x.UserClick != null && x.UserClick.MaterialId == mid && x.UserClick.ModelCoreType == mct && (x.UserClick.SessionId == sessionId || x.UserClick.UserId == userId));

            if (existLike == null && likeDirection != LikeDirection.Unknown)
            {
                var userClick = new SxUserClick { MaterialId = mid, ModelCoreType = mct, SessionId = sessionId, UserId = userId, ClickType = Enums.UserClickType.Like };
                var newUserClick = _repo.Create(userClick);
                var like = new SxLike { Direction = likeDirection, UserClickId = newUserClick.Id };
                var newLike = _repoLike.Create(like);
            }
            var count = _repoLike.All.Where(x => x.UserClick != null && x.UserClick.MaterialId == mid && x.UserClick.ModelCoreType == mct && x.Direction == likeDirection).Count();
            return count;
        }

        private int addUserClick(int mid, ModelCoreType mct, UserClickType uct, string sessionId, string userId)
        {
            var existClick = _repo.All.SingleOrDefault(x => x.MaterialId == mid && x.ModelCoreType == mct && x.ClickType == uct && (x.SessionId == sessionId || x.UserId == userId));
            if (existClick == null)
            {
                var userClick = new SxUserClick { MaterialId = mid, ModelCoreType = mct, SessionId = sessionId, UserId = userId, ClickType = uct };
                var newUserClick = _repo.Create(userClick);
            }

            var count = _repo.All.Where(x => x.MaterialId == mid && x.ModelCoreType == mct && x.ClickType == uct).Count();
            return count;
        }
    }
}