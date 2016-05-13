using GE.WebAdmin.Models;
using GE.WebCoreExtantions;
using SX.WebCore;
using SX.WebCore.Abstract;
using SX.WebCore.Repositories;
using System.Linq;
using System.Web.Mvc;

namespace GE.WebAdmin.Controllers
{
    public partial class FAQController : BaseController
    {
        private SxDbRepository<int, SxManual, DbContext> _repo;
        public FAQController()
        {
            _repo = new RepoManual<DbContext>();
        }

        [HttpGet]
        public virtual ActionResult Index(string curCat = null)
        {
            ViewBag.CategoryId = curCat;

            var viewModel = (_repo as RepoManual<DbContext>).GetManualsByCategoryId(curCat)
                .Select(x => Mapper.Map<SxManual, VMFAQ>(x)).ToArray();

            return View(curCat == null ? new VMFAQ[0] : viewModel);
        }

        [HttpGet]
        public virtual ActionResult OldIndex()
        {
            return View();
        }
    }
}