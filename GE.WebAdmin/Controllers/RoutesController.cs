using GE.WebAdmin.Models;
using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using SX.WebCore;
using SX.WebCore.Abstract;
using System;
using System.Linq;
using GE.WebAdmin.Extantions.Repositories;
using System.Web.Mvc;
using System.Collections.Generic;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace GE.WebAdmin.Controllers
{
    [Authorize(Roles ="architect")]
    public partial class RoutesController : BaseController
    {
        private SxDbRepository<Guid, SxRoute, DbContext> _repo;
        public RoutesController()
        {
            _repo = new RepoRoute();
        }

        private static int _pageSize = 20;

        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ViewResult Index(int page = 1)
        {
            var filter = new WebCoreExtantions.Filter { SkipCount = (page - 1) * _pageSize, PageSize = _pageSize };
            var list = (_repo as RepoRoute).QueryForAdmin(filter).ToArray();

            ViewData["Page"] = page;
            ViewData["PageSize"] = _pageSize;
            ViewData["RowsCount"] = (_repo as RepoRoute).FilterCount(filter);

            return View(list);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual PartialViewResult Index(VMRoute filter, IDictionary<string, SortDirection> order, int page = 1)
        {
            ViewBag.Filter = filter;
            ViewBag.Order = order;

            var f = new WebCoreExtantions.Filter { SkipCount = (page - 1) * _pageSize, PageSize = _pageSize, Orders = order, WhereExpressionObject = filter };
            var list = (_repo as RepoRoute).QueryForAdmin(f).ToArray();

            ViewData["Page"] = page;
            ViewData["PageSize"] = _pageSize;
            ViewData["RowsCount"] = (_repo as RepoRoute).FilterCount(f);

            return PartialView("_GridView", list);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ViewResult Edit(Guid? id)
        {
            var model = id.HasValue ? _repo.GetByKey(id) : new SxRoute();
            var viewModel = Mapper.Map<SxRoute, VMEditRoute>(model);
            return View(viewModel);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Edit(VMEditRoute model)
        {
            var redactModel = Mapper.Map<VMEditRoute, SxRoute>(model);
            if (ModelState.IsValid)
            {
                SxRoute newModel = null;
                if (model.Id == Guid.Empty)
                    newModel = _repo.Create(redactModel);
                else
                    newModel = _repo.Update(redactModel, "Name", "Domain", "Controller", "Action");
                return RedirectToAction("Index");
            }
            else
                return View(model);
        }
    }
}