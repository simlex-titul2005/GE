using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using SX.WebCore;
using SX.WebCore.Abstract;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace GE.WebUI.Controllers
{
    public class VotesController : BaseController
    {
        private SxDbRepository<int, SxVote, DbContext> _repo;
        public VotesController()
        {
            _repo = new RepoVote();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public EmptyResult Create(Enums.ModelCoreType mct, int mid, byte up = 0)
        {
            if (!User.Identity.IsAuthenticated) return null;

            Task.Run(() =>
            {
                var model = new SxVote { ModelCoreType = mct, MaterialId = mid, IsUp = up, UserId=User.Identity.GetUserId() };
                _repo.Create(model);
            });
            return null;
        }
    }
}