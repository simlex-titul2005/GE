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
            var order = new SxOrder { FieldName = "Title", Direction = SortDirection.Desc };
            var filter = new SxFilter(page, _pageSize) { Order = order };

            var viewModel = Repo.Read(filter);
            if (page > 1 && !viewModel.Any())
                return new HttpNotFoundResult();

            ViewBag.Filter = filter;

            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Index(VMGame filterModel, SxOrder order, int page = 1)
        {
            var filter = new SxFilter(page, _pageSize) { Order = order != null && order.Direction != SortDirection.Unknown ? order : null, WhereExpressionObject = filterModel };

            var viewModel = await Repo.ReadAsync(filter);
            if (page > 1 && !viewModel.Any())
                return new HttpNotFoundResult();

            ViewBag.Filter = filter;

            return PartialView("_GridView", viewModel);
        }

        [HttpGet]
        public ViewResult Edit(int? id)
        {
            var model = id.HasValue ? Repo.GetByKey(id) : new Game();
            ViewData["FrontPictureIdCaption"] = model.FrontPicture?.Caption;
            ViewData["GoodPictureIdCaption"] = model.GoodPicture?.Caption;
            ViewData["BadPictureIdCaption"] = model.BadPicture?.Caption;
            return View(Mapper.Map<Game, VMGame>(model));
        }

        //[HttpPost, ValidateAntiForgeryToken]
        //public ActionResult Edit(VMEditGame model)
        //{
        //    var redactModel = Mapper.Map<VMEditGame, Game>(model);
        //    if (ModelState.IsValid)
        //    {
        //        Game newModel = null;
        //        if (model.Id == 0)
        //            newModel = _repo.Create(redactModel);
        //        else
        //            newModel = _repo.Update(redactModel, true, "Title", "TitleAbbr", "Description", "Show", "FrontPictureId", "GoodPictureId", "BadPictureId", "TitleUrl", "FullDescription");
        //        return RedirectToAction("index");
        //    }
        //    else
        //        return View(model);
        //}

        //[HttpPost, AllowAnonymous]
        //public PartialViewResult FindGridView(VMGame filterModel, SxOrder order, int page = 1, int pageSize = 10)
        //{
        //    var defaultOrder = new SxOrder { FieldName = "Title", Direction = SortDirection.Asc };
        //    var filter = new SxFilter(page, pageSize) { WhereExpressionObject = filterModel, Order = order == null || order.Direction == SortDirection.Unknown ? defaultOrder : order };
        //    var totalItems = (_repo as RepoGame).FilterCount(filter);
        //    filter.PagerInfo.TotalItems = totalItems;
        //    ViewBag.Filter = filter;

        //    var viewModel = (_repo as RepoGame).QueryForAdmin(filter);

        //    return PartialView("~/views/Games/_FindGridView.cshtml", viewModel);
        //}
    }
}