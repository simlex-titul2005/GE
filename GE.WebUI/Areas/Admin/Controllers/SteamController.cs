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
    }
}