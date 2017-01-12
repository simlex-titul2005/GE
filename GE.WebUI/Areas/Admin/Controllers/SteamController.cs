using GE.WebUI.ViewModels;
using SX.WebCore;
using System.Web.Mvc;
using System.Linq;
using System.Threading.Tasks;
using GE.WebUI.Infrastructure.Repositories;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace GE.WebUI.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    public sealed class SteamController : BaseController
    {
        private static RepoSteamApp Repo { get; set; } = new RepoSteamApp();

        private static readonly int _appPageSize = 20;
        [HttpGet]
        public async Task<ActionResult> AppIndex(int page = 1)
        {
            var filter = new SxFilter(page, _appPageSize);
            var viewModel = await Repo.ReadAsync(filter);
            ViewBag.Filter = filter;
            return View(model: viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> AppIndex(VMSteamApp filterModel, SxOrderItem order, int page = 1)
        {
            var filter = new SxFilter(page, _appPageSize) { Order = order != null && order.Direction != SortDirection.Unknown ? order : null, WhereExpressionObject = filterModel };

            var viewModel = await Repo.ReadAsync(filter);
            if (page > filter.PagerInfo.TotalPages) page = 1;
            if (page > 1 && !viewModel.Any()) return new HttpNotFoundResult();

            ViewBag.Filter = filter;
            return PartialView("_GridViewApp", viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> AppIndexFree(VMSteamApp filterModel, SxOrderItem order, int page = 1, int pageSize=10)
        {
            var filter = new SxFilter(page, pageSize) { Order = order != null && order.Direction != SortDirection.Unknown ? order : null, WhereExpressionObject = filterModel, AddintionalInfo = new object[] { true, null } };

            var viewModel = await Repo.ReadAsync(filter);
            if (page > filter.PagerInfo.TotalPages) page = 1;
            if (page > 1 && !viewModel.Any()) return new HttpNotFoundResult();

            ViewBag.Filter = filter;
            return PartialView("_GridViewAppFree", viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> AppIndexLinked(VMSteamApp filterModel, SxOrderItem order, int page = 1, int pageSize = 10, int? gameId=null)
        {
            var gid = string.IsNullOrEmpty(Request.Form.Get("gameId"))?null: Request.Form.Get("gameId");
            ViewBag.GameId = gid ?? gameId.ToString();

            var filter = new SxFilter(page, pageSize) { Order = order != null && order.Direction != SortDirection.Unknown ? order : null, WhereExpressionObject = filterModel, AddintionalInfo = new object[] { null, ViewBag.GameId } };

            var viewModel = await Repo.ReadAsync(filter);
            if (page > filter.PagerInfo.TotalPages) page = 1;
            if (page > 1 && !viewModel.Any()) return new HttpNotFoundResult();

            ViewBag.Filter = filter;
            return PartialView("_GridViewAppLinked", viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<JsonResult> LinkSteamApps(int gameId, int[] steamAppIds)
        {
            var data = await Repo.LinkSteamAppsAsync(gameId, steamAppIds);
            return Json(data);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<JsonResult> UnlinkSteamApps(int gameId, int[] steamAppIds)
        {
            var data = await Repo.UnlinkSteamAppsAsync(gameId, steamAppIds);
            return Json(data);
        }
    }
}