using GE.WebAdmin.Models;
using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using SX.WebCore;
using SX.WebCore.Abstract;
using SX.WebCore.HtmlHelpers;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GE.WebAdmin.Extantions.Repositories;

namespace GE.WebAdmin.Controllers
{
    public partial class MenuesController : BaseController
    {
        private SxDbRepository<int, SxMenu, DbContext> _repo;
        public MenuesController()
        {
            _repo = new RepoMenu();
        }

        private static int _pageSize = 20;

        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ViewResult Index(int page = 1)
        {
            var filter = new WebCoreExtantions.Filter(page, _pageSize);
            var totalItems = (_repo as RepoMenu).FilterCount(filter);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.PagerInfo = filter.PagerInfo;

            var viewModel = (_repo as RepoMenu).QueryForAdmin(filter);
            return View(viewModel);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual PartialViewResult Index(VMMenu filterModel, IDictionary<string, SxExtantions.SortDirection> order, int page = 1)
        {
            ViewBag.Filter = filterModel;
            ViewBag.Order = order;

            var filter = new WebCoreExtantions.Filter(page, _pageSize) { Orders = order, WhereExpressionObject = filterModel };
            var totalItems = (_repo as RepoMenu).FilterCount(filter);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.PagerInfo = filter.PagerInfo;

            var viewModel = (_repo as RepoMenu).QueryForAdmin(filter);

            return PartialView("_GridView", viewModel);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ViewResult Edit(int? id)
        {
            var model = id.HasValue ? _repo.GetByKey(id) : new SxMenu { Items = new SxMenuItem[0] };
            var viewModel = Mapper.Map<SxMenu, VMEditMenu>(model);
            viewModel.Items = model.Items.Select(x => Mapper.Map<SxMenuItem, VMMenuItem>(x)).ToArray();
            return View(viewModel);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Edit(VMEditMenu model)
        {
            var redactModel = Mapper.Map<VMEditMenu, SxMenu>(model);
            if (ModelState.IsValid)
            {
                SxMenu newModel = null;
                if (model.Id == 0)
                    newModel = _repo.Create(redactModel);
                else
                    newModel = _repo.Update(redactModel, "Name", "Description");
                return RedirectToAction("Index");
            }
            else
                return View(model);
        }
    }
}