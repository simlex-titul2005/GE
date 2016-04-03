using GE.WebAdmin.Models;
using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using SX.WebCore;
using SX.WebCore.Abstract;
using SX.WebCore.HtmlHelpers;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace GE.WebAdmin.Controllers
{
    public partial class ClickTypesController : BaseController
    {
        SxDbRepository<int, SxClickType, DbContext> _repo;
        public ClickTypesController()
        {
            _repo = new RepoClickType();
        }

        private static int _pageSize = 20;

        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ViewResult Index(int page = 1)
        {
            var filter = new WebCoreExtantions.Filter(page, _pageSize);
            var totalItems = _repo.Count(filter);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.PagerInfo = filter.PagerInfo;

            var viewModel = _repo.Query(filter).Select(x => Mapper.Map<SxClickType, VMClickType>(x)).ToArray();
            return View(viewModel);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual PartialViewResult Index(VMClickType filterModel, IDictionary<string, SxExtantions.SortDirection> order, int page = 1)
        {
            ViewBag.Filter = filterModel;
            ViewBag.Order = order;

            var filter = new WebCoreExtantions.Filter(page, _pageSize) { Orders = order, WhereExpressionObject = filterModel };
            var totalItems = _repo.Count(filter);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.PagerInfo = filter.PagerInfo;

            var viewModel = _repo.Query(filter).Select(x => Mapper.Map<SxClickType, VMClickType>(x)).ToArray();

            return PartialView("_GridView", viewModel);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ViewResult Edit(int? id)
        {
            var model = id.HasValue ? _repo.GetByKey(id) : new SxClickType();
            var viewModel=Mapper.Map<SxClickType, VMEditClickType>(model);
            return View(viewModel);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Edit(VMEditClickType model)
        {
            var redactModel = Mapper.Map<VMEditClickType, SxClickType>(model);
            if (ModelState.IsValid)
            {
                SxClickType newModel = null;
                if (model.Id == 0)
                    newModel = _repo.Create(redactModel);
                else
                    newModel = _repo.Update(redactModel, "Name", "Description");
                return RedirectToAction(MVC.ClickTypes.Index());
            }
            else
                return View(redactModel);
        }
    }
}