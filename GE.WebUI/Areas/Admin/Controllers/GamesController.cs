using GE.WebUI.Infrastructure.Repositories;
using GE.WebUI.Models;
using GE.WebUI.ViewModels;
using SX.WebCore;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace GE.WebUI.Areas.Admin.Controllers
{
    public class GamesController : BaseController
    {
        public static RepoGame Repo { get; set; }=new RepoGame();

        private static int _pageSize = 20;

        [HttpGet]
        public ActionResult Index(int page = 1)
        {
            var order = new SxOrderItem { FieldName = "Title", Direction = SortDirection.Desc };
            var filter = new SxFilter(page, _pageSize) { Order = order };

            var viewModel = Repo.Read(filter);
            if (page > 1 && !viewModel.Any())
                return new HttpNotFoundResult();

            ViewBag.Filter = filter;

            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Index(VMGame filterModel, SxOrderItem order, int page = 1)
        {
            var filter = new SxFilter(page, _pageSize) { Order = order != null && order.Direction != SortDirection.Unknown ? order : null, WhereExpressionObject = filterModel };

            var viewModel = await Repo.ReadAsync(filter);
            if (page > 1 && !viewModel.Any())
                return new HttpNotFoundResult();

            ViewBag.Filter = filter;

            return PartialView("_GridView", viewModel);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int? id)
        {
            var model = id.HasValue ? await Repo.GetByKeyAsync(id) : new Game();
            if (id.HasValue && model == null) return new HttpNotFoundResult();
            ViewData["FrontPictureIdCaption"] = model.FrontPicture?.Caption;
            ViewData["GoodPictureIdCaption"] = model.GoodPicture?.Caption;
            ViewData["BadPictureIdCaption"] = model.BadPicture?.Caption;
            return View(Mapper.Map<Game, VMGame>(model));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(VMGame model)
        {
            if (ModelState.IsValid)
            {
                var redactModel = Mapper.Map<VMGame, Game>(model);
                redactModel=model.Id==0? Repo.Create(redactModel): Repo.Update(redactModel);
                return RedirectToAction("Index");
            }
            else
                return View(model);
        }

        [HttpPost, AllowAnonymous]
        public async Task<ActionResult> FindGridView(VMGame filterModel, SxOrderItem order, int page = 1, int pageSize = 10)
        {
            var defaultOrder = new SxOrderItem { FieldName = "Title", Direction = SortDirection.Asc };
            var filter = new SxFilter(page, pageSize) { WhereExpressionObject = filterModel, Order = order == null || order.Direction == SortDirection.Unknown ? defaultOrder : order };

            var viewModel = await Repo.ReadAsync(filter);

            ViewBag.Filter = filter;

            return PartialView("_FindGridView", viewModel);
        }
    }
}