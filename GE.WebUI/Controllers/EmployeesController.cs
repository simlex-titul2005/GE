using GE.WebCoreExtantions;
using GE.WebUI.Models;
using SX.WebCore;
using SX.WebCore.Abstract;
using SX.WebCore.Repositories;
using System.Linq;
using System.Web.Mvc;

namespace GE.WebUI.Controllers
{
    public partial class EmployeesController : BaseController
    {
        private SxDbRepository<string, SxEmployee, DbContext> _repo;
        public EmployeesController()
        {
            _repo = new SxRepoEmployee<DbContext>();
        }

#if !DEBUG
        [OutputCache(Duration =900)]
#endif
        [HttpGet]
        public virtual ActionResult List()
        {
            var data = _repo.All.ToArray();
            var viewModel = data.Select(x => Mapper.Map<SxEmployee, VMEmployee>(x)).ToArray();
            return View(viewModel);
        }
    }
}