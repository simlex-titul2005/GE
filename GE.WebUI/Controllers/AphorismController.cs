using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using GE.WebUI.Models;
using SX.WebCore.Attrubutes;
using System.Web.Mvc;

namespace GE.WebUI.Controllers
{
    public sealed class AphorismController : BaseController
    {
        private static RepoAphorism _repo;
        public AphorismController()
        {
            if(_repo==null)
                _repo = new RepoAphorism();
        }

        [HttpGet, NotLogRequest]
        public ActionResult Random(int? id = null)
        {
            var data = (_repo as RepoAphorism).GetRandom(id);
            ViewBag.AphorismLettersCount = data.Html.Length;
            var viewModel = Mapper.Map<Aphorism, VMAphorism>(data);
            return PartialView("_Random", viewModel);
        }
    }
}