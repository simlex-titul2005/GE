using GE.WebCoreExtantions;
using SX.WebCore;
using SX.WebCore.Abstract;
using SX.WebCore.Repositories;
using System.Web.Mvc;

namespace GE.WebUI.Controllers
{
    public partial class SiteTestsController : BaseController
    {
        private static SxDbRepository<int, SxSiteTest, DbContext> _repo;
        public SiteTestsController()
        {
            if (_repo == null)
                _repo = new RepoSiteTest<DbContext>();
        }

        [ChildActionOnly]
        public virtual PartialViewResult RandomList()
        {
            var data = (_repo as RepoSiteTest<DbContext>).RandomList();
            return PartialView("_RandomList", data);
        }
    }
}