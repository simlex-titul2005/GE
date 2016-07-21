using GE.WebCoreExtantions;
using SX.WebCore;
using SX.WebCore.Repositories;
using SX.WebCore.ViewModels;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace GE.WebUI.Controllers
{
    public sealed class EmployeesController : BaseController
    {
        private static SxRepoEmployee<GE.WebCoreExtantions.DbContext> _repo;
        public EmployeesController()
        {
            if(_repo==null)
                _repo = new SxRepoEmployee<GE.WebCoreExtantions.DbContext>();
        }

#if !DEBUG
        [OutputCache(Duration =900)]
#endif
        [HttpGet]
        public ActionResult List()
        {
            var data = _repo.All.Include(x=>x.User).ToArray();
            var viewModel = data.Select(x => Mapper.Map<SxEmployee, SxVMEmployee>(x)).ToArray();
            return View(viewModel);
        }
    }
}