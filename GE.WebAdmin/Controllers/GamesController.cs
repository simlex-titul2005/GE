using GE.WebAdmin.Extantions.Repositories;
using GE.WebAdmin.Models;
using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using SX.WebCore.HtmlHelpers;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace GE.WebAdmin.Controllers
{
    public partial class GamesController : BaseController
    {
        SX.WebCore.Abstract.SxDbRepository<int, Game, DbContext> _repo;
        public GamesController()
        {
            _repo = new RepoGame();
        }

        private static int _pageSize = 20;

        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ViewResult Index(int page = 1)
        {
            var filter = new WebCoreExtantions.Filter(page, _pageSize);
            var totalItems = (_repo as RepoGame).FilterCount(filter);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.PagerInfo = filter.PagerInfo;

            var viewModel = (_repo as RepoGame).QueryForAdmin(filter);
            return View(viewModel);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual PartialViewResult Index(VMGame filterModel, IDictionary<string, SxExtantions.SortDirection> order, int page = 1)
        {
            ViewBag.Filter = filterModel;
            ViewBag.Order = order;

            var filter = new WebCoreExtantions.Filter(page, _pageSize) { Orders = order, WhereExpressionObject = filterModel };
            var totalItems = (_repo as RepoGame).FilterCount(filter);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.PagerInfo = filter.PagerInfo;

            var viewModel = (_repo as RepoGame).QueryForAdmin(filter);

            return PartialView("_GridView", viewModel);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ViewResult Edit(int? id)
        {
            var model = id.HasValue ? _repo.GetByKey(id) : new Game();
            return View(Mapper.Map<Game, VMEditGame>(model));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Edit(VMEditGame model)
        {
            var redactModel = Mapper.Map<VMEditGame, Game>(model);
            if (ModelState.IsValid)
            {
                Game newModel = null;
                if (model.Id == 0)
                    newModel = _repo.Create(redactModel);
                else
                    newModel = _repo.Update(redactModel, "Title", "TitleAbbr", "Description", "Show", "FrontPictureId", "GoodPictureId", "BadPictureId", "TitleUrl");
                return RedirectToAction(MVC.Games.Index());
            }
            else
                return View(model);
        }

        [HttpPost]
        public virtual PartialViewResult FindGridView(VMGame filterModel, int page = 1, int pageSize = 10)
        {
            ViewBag.Filter = filterModel;
            var filter = new WebCoreExtantions.Filter(page, pageSize);
            filter.WhereExpressionObject = filterModel;
            var totalItems = (_repo as RepoGame).FilterCount(filter);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.PagerInfo = filter.PagerInfo;

            var viewModel = (_repo as RepoGame).QueryForAdmin(filter);

            return PartialView(MVC.Games.Views._FindGridView, viewModel);
        }
    }
}