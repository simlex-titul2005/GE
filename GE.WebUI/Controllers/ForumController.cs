using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using SX.WebCore;
using SX.WebCore.Abstract;
using System.Linq;
using System.Web.Mvc;
using GE.WebUI.Models;

namespace GE.WebUI.Controllers
{
    public partial class ForumController : BaseController
    {
        private SxDbRepository<int, SxForumPart, DbContext> _repo;
        public ForumController()
        {
            _repo = new RepoForumPart();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ViewResult List(GE.WebCoreExtantions.Filter filter)
        {
            var data = _repo.Query(filter).Select(x=>Mapper.Map<SxForumPart, VMForumPart>(x)).ToArray();
            return View(data);
        }
    }
}