using GE.WebAdmin.Extantions.Repositories;
using GE.WebAdmin.Models;
using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using SX.WebCore;
using System.Web.Mvc;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace GE.WebAdmin.Controllers
{
    [Authorize(Roles ="admin")]
    public sealed class GamesController : BaseController
    {
        private static RepoGame _repo;
        private static int _pageSize = 20;
        public GamesController()
        {
            if(_repo==null)
                _repo = new RepoGame();
        }

        

        [HttpGet]
        public ViewResult Index(int page = 1)
        {
            var defaultOrder = new SxOrder { FieldName = "Title", Direction = SortDirection.Asc };
            var filter = new SxFilter(page, _pageSize) { Order= defaultOrder };
            var totalItems = (_repo as RepoGame).FilterCount(filter);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.Filter = filter;

            var viewModel = (_repo as RepoGame).QueryForAdmin(filter);
            return View(viewModel);
        }

        [HttpPost]
        public PartialViewResult Index(VMGame filterModel, SxOrder order, int page = 1)
        {
            var filter = new SxFilter(page, _pageSize) { Order = order, WhereExpressionObject = filterModel };
            var totalItems = (_repo as RepoGame).FilterCount(filter);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.Filter = filter;

            var viewModel = (_repo as RepoGame).QueryForAdmin(filter);

            return PartialView("_GridView", viewModel);
        }

        [HttpGet]
        public ViewResult Edit(int? id)
        {
            var model = id.HasValue ? _repo.GetByKey(id) : new Game();
            ViewData["FrontPictureIdCaption"] = model.FrontPicture?.Caption;
            ViewData["GoodPictureIdCaption"] = model.GoodPicture?.Caption;
            ViewData["BadPictureIdCaption"] = model.BadPicture?.Caption;
            return View(Mapper.Map<Game, VMEditGame>(model));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(VMEditGame model)
        {
            var redactModel = Mapper.Map<VMEditGame, Game>(model);
            if (ModelState.IsValid)
            {
                Game newModel = null;
                if (model.Id == 0)
                    newModel = _repo.Create(redactModel);
                else
                    newModel = _repo.Update(redactModel, true, "Title", "TitleAbbr", "Description", "Show", "FrontPictureId", "GoodPictureId", "BadPictureId", "TitleUrl", "FullDescription");
                return RedirectToAction("index");
            }
            else
                return View(model);
        }

        [HttpPost, AllowAnonymous]
        public PartialViewResult FindGridView(VMGame filterModel, SxOrder order, int page = 1, int pageSize = 10)
        {
            var defaultOrder = new SxOrder { FieldName = "Title", Direction = SortDirection.Asc };
            var filter = new SxFilter(page, pageSize) { WhereExpressionObject=filterModel, Order=order==null || order.Direction==SortDirection.Unknown? defaultOrder:order};
            var totalItems = (_repo as RepoGame).FilterCount(filter);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.Filter = filter;

            var viewModel = (_repo as RepoGame).QueryForAdmin(filter);

            return PartialView("~/views/Games/_FindGridView.cshtml", viewModel);
        }
    }
}