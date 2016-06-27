using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using GE.WebUI.Models;
using SX.WebCore.Abstract;
using SX.WebCore.Attrubutes;
using System.Web.Mvc;

namespace GE.WebUI.Controllers
{
    public partial class AphorismController : BaseController
    {
        private SxDbRepository<int, Aphorism, DbContext> _repo;
        public AphorismController()
        {
            _repo = new RepoAphorism();
        }

        [HttpGet, NotLogRequest]
        public virtual ActionResult Random(int? id = null)
        {
            var data = (_repo as RepoAphorism).GetRandom(id);
            ViewBag.AphorismLettersCount = data.Html.Length;
            var viewModel = Mapper.Map<Aphorism, VMAphorism>(data);
            return PartialView("_Random", viewModel);
        }
    }
}