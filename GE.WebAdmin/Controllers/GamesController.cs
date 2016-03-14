using GE.WebAdmin.Models;
using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using SX.WebCore.HtmlHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
            var list = _repo.All
                .Skip((page - 1) * _pageSize)
                .Take(_pageSize).OrderBy(x=>x.Title).Select(x => Mapper.Map<Game, VMGame>(x)).ToArray();

            ViewData["Page"] = page;
            ViewData["PageSize"] = _pageSize;
            ViewData["RowsCount"] = _repo.All.Count();

            return View(list);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual PartialViewResult Index(VMGame filter, IDictionary<string, SxExtantions.SortDirection> order, int page = 1)
        {
            string title = filter != null ? filter.Title : null;
            ViewBag.Filter = filter;
            ViewBag.Order = order;

            //select
            var temp = _repo.All;
            if (title != null)
                temp = temp.Where(x => x.Title.Contains(title));

            //order
            var orders = order.Where(x => x.Value != SxExtantions.SortDirection.Unknown);
            if (orders.Count() != 0)
            {
                foreach (var o in orders)
                {
                    if (o.Key == "Name")
                    {
                        if (o.Value == SxExtantions.SortDirection.Desc)
                            temp = temp.OrderByDescending(x => x.Title);
                        else if (o.Value == SxExtantions.SortDirection.Asc)
                            temp = temp.OrderBy(x => x.Title);
                    }
                }
            }

            var list = temp.Skip((page - 1) * _pageSize)
                .Take(_pageSize).Select(x => Mapper.Map<Game, VMGame>(x)).ToArray();

            ViewData["Page"] = page;
            ViewData["PageSize"] = _pageSize;
            ViewData["RowsCount"] = temp.Count();

            return PartialView("_GridView", list);
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

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual ViewResult FindTable(int page = 1, int pageSize = 10)
        {
            var viewModel = new SxExtantions.SxPagedCollection<VMGame>
            {
                Collection = _repo.All
                .OrderBy(x=>x.Title)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => Mapper.Map<Game, VMGame>(x)).ToArray(),
                PagerInfo = new SxExtantions.SxPagerInfo
                {
                    Page = page,
                    PageSize = pageSize,
                    TotalItems = _repo.All.Count(),
                    PagerSize = 4
                }
            };

            return View(viewModel);
        }
    }
}