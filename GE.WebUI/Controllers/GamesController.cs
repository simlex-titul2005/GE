using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using GE.WebUI.Models;
using SX.WebCore;
using SX.WebCore.Abstract;
using System.Web.Mvc;
using GE.WebUI.Extantions.Repositories;

namespace GE.WebUI.Controllers
{
    public partial class GamesController : BaseController
    {
        private SxDbRepository<int, Game, DbContext> _repo;
        private SxDbRepository<string, SxSiteSetting, DbContext> _repoSetting;
        public GamesController()
        {
            _repo = new RepoGame();
            _repoSetting = new RepoSiteSetting();
        }

        private const string __emptyGameIconPath = "emptyGameIconPath";
        private const string __emptyGameGoodImagePath = "emptyGameGoodImagePath";
        private const string __emptyGameBadImagePath = "emptyGameBadImagePath";
        [ChildActionOnly]
        public virtual PartialViewResult GameList(int imgWidth = 570, int iconHeight = 80, int gnc=3)
        {
            var routes = Request.RequestContext.RouteData.Values;
            var controller = routes["controller"].ToString().ToLowerInvariant();

            if (controller == "account" || controller=="sitequetions")
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

            return PartialView(MVC.Games.Views._GameList, viewModel);
        }

        [HttpGet]
        public virtual ActionResult Details(string titleUrl)
        {
            var viewModel = (_repo as RepoGame).GetGameDetails(titleUrl);

            if (viewModel == null) return new HttpStatusCodeResult(404);

            ViewBag.GameName = titleUrl.ToLowerInvariant();
            return View(viewModel);
        }
    }
}