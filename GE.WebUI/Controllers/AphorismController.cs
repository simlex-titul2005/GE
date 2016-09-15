using GE.WebUI.Infrastructure.Repositories;
using GE.WebUI.Models;
using GE.WebUI.ViewModels;
using SX.WebCore.Attrubutes;
using System.Web.Mvc;

namespace GE.WebUI.Controllers
{
    public sealed class AphorismController : BaseController
    {
        private static RepoAphorism _repo = new RepoAphorism();
        public RepoAphorism Repo
        {
            get { return _repo; }
            set { _repo = value; }
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