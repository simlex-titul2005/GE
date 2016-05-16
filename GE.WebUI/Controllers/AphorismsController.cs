using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using GE.WebUI.Models;
using SX.WebCore.Abstract;
using SX.WebCore.Attrubutes;
using System.Web.Mvc;
using GE.WebUI.Extantions.Repositories;

namespace GE.WebUI.Controllers
{
    public partial class AphorismsController : BaseController
    {
        private SxDbRepository<int, Aphorism, DbContext> _repo;
        public AphorismsController()
        {
            _repo = new RepoAphorism();
        }

        [HttpGet, NotLogRequest]
        public virtual PartialViewResult Random(int? id = null)
        {
            var data = (_repo as RepoAphorism).GetRandom(id);
            ViewBag.AphorismLettersCount = data.Html.Length;
            var viewModel = Mapper.Map<Aphorism, VMAphorism>(data);
            return PartialView(MVC.Aphorisms.Views._Random, viewModel);
        }

        [HttpGet]
        public virtual ViewResult Details(string categoryId, string titleUrl)
        {
            var vierwModel = (_repo as RepoAphorism).GetByTitleUrl(titleUrl);
            return View(vierwModel);
        }
    }
}