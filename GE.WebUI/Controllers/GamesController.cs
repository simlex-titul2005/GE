using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using GE.WebUI.Models;
using SX.WebCore;
using SX.WebCore.Abstract;
using System.Web.Mvc;
using GE.WebUI.Extantions.Repositories;
using System.Linq;
using SX.WebCore.Repositories;

namespace GE.WebUI.Controllers
{
    public sealed class GamesController : BaseController
    {
        private SxDbRepository<int, Game, DbContext> _repo;
        private SxDbRepository<string, SxSiteSetting, DbContext> _repoSetting;
        public GamesController()
        {
            _repo = new RepoGame();
            _repoSetting = new SxRepoSiteSetting<DbContext>();
        }

        private const string __emptyGameIconPath = "emptyGameIconPath";
        private const string __emptyGameGoodImagePath = "emptyGameGoodImagePath";
        private const string __emptyGameBadImagePath = "emptyGameBadImagePath";
        [ChildActionOnly]
        public PartialViewResult GameList(int imgWidth = 570, int iconHeight = 80, int gnc=3)
        {
            var routes = Request.RequestContext.RouteData.Values;
            var controller = routes["controller"].ToString().ToLowerInvariant();
            var blackList = new string[] { "account", "sitequetions", "employees" };

            if (blackList.Contains(controller))
                return null;

            ViewBag.ControllerName = controller;
            if (ViewBag.ControllerName == "error") return null;
            ViewBag.ActionName = routes["action"];
            var gameName = routes["gameTitle"] ?? this.ControllerContext.ParentActionViewContext.ViewBag.GameName;
            ViewBag.GameName = gameName;
            if (ViewBag.ActionName == "details")
                ViewBag.GameName = ControllerContext.ParentActionViewContext.ViewBag.GameName;

            var viewModel = (_repo as RepoGame).GetGameMenu(imgWidth, iconHeight, gnc, gameName == null ? (string)null : (string)gameName);

            var emptyGameIconPath = _repoSetting.GetByKey(__emptyGameIconPath);
            var emptyGameGoodImagePath = _repoSetting.GetByKey(__emptyGameGoodImagePath);
            var emptyGameBadImagePath = _repoSetting.GetByKey(__emptyGameBadImagePath);
            viewModel.EmptyGame = new VMEmptyGame
            {
                IconPath = emptyGameIconPath != null ? emptyGameIconPath.Value : null,
                GoodImagePath = emptyGameGoodImagePath != null ? emptyGameGoodImagePath.Value : null,
                BadImagePath = emptyGameBadImagePath != null ? emptyGameBadImagePath.Value : null,
            };

            return PartialView("_GameList", viewModel);
        }

        [HttpGet]
        public ActionResult Details(string titleUrl)
        {
            if (string.IsNullOrEmpty(titleUrl) || Equals(titleUrl.ToUpper(), "DETAILS")) return new HttpNotFoundResult();

            var viewModel = (_repo as RepoGame).GetGameDetails(titleUrl);

            if (viewModel == null) return new HttpStatusCodeResult(404);

            ViewBag.GameName = titleUrl.ToLowerInvariant();
            return View(viewModel);
        }
    }
}