using System.Web.Mvc;
using System.Linq;
using GE.WebUI.Infrastructure.Repositories;
using SX.WebCore.MvcControllers;
using GE.WebUI.ViewModels;

namespace GE.WebUI.Controllers
{
    public sealed class GamesController : BaseController
    {
        public static RepoGame Repo { get; set; } = new RepoGame();

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
            var gameName = routes["gameTitle"] ?? ControllerContext.ParentActionViewContext.ViewBag.GameName;
            ViewBag.GameName = gameName;
            if (ViewBag.ActionName == "details")
                ViewBag.GameName = ControllerContext.ParentActionViewContext.ViewBag.GameName;

            var viewModel = (Repo as RepoGame).GetGameMenu(imgWidth, iconHeight, gnc, gameName == null ? (string)null : (string)gameName);

            var emptyGameIconPath = SxSiteSettingsController.Repo.GetByKey(__emptyGameIconPath)?.Value;
            var emptyGameGoodImagePath = SxSiteSettingsController.Repo.GetByKey(__emptyGameGoodImagePath)?.Value;
            var emptyGameBadImagePath = SxSiteSettingsController.Repo.GetByKey(__emptyGameBadImagePath)?.Value;
            viewModel.EmptyGame = new VMGameMenuEmptyGame
            {
                IconPath = emptyGameIconPath != null ? Url.Action("Picture", "Pictures", new { id = emptyGameIconPath }) : null,
                GoodImagePath = emptyGameGoodImagePath != null ? Url.Action("Picture", "Pictures", new { id = emptyGameGoodImagePath }) : null,
                BadImagePath = emptyGameBadImagePath != null ? Url.Action("Picture", "Pictures", new { id = emptyGameBadImagePath }) : null,
            };

            return PartialView("_GameList", viewModel);
        }

        [HttpGet]
        public ActionResult Details(string titleUrl)
        {
            if (string.IsNullOrEmpty(titleUrl) || Equals(titleUrl.ToUpper(), "DETAILS")) return new HttpNotFoundResult();

            var viewModel = (Repo as RepoGame).GetGameDetails(titleUrl);

            if (viewModel == null) return new HttpStatusCodeResult(404);

            ViewBag.GameName = titleUrl.ToLowerInvariant();
            return View(viewModel);
        }
    }
}