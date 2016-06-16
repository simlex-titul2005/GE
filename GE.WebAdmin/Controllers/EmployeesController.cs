using GE.WebAdmin.Extantions.Repositories;
using GE.WebAdmin.Models;
using GE.WebCoreExtantions;
using SX.WebCore;
using SX.WebCore.Abstract;
using SX.WebCore.Repositories;
using System.Collections.Generic;
using System.Web.Mvc;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace GE.WebAdmin.Controllers
{
    public partial class EmployeesController : BaseController
    {
        private SxDbRepository<string, SxEmployee, DbContext> _repo;
        private static int _pageSize = 20;

        public EmployeesController()
        {
            _repo = new SxRepoEmployee<DbContext>();
        }

        [HttpGet]
        public virtual ViewResult Index(int page = 1)
        {
            var filter = new WebCoreExtantions.Filter(page, _pageSize);
            var totalItems = (_repo as SxRepoEmployee<DbContext>).FilterCount(filter);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.PagerInfo = filter.PagerInfo;

            var viewModel = (_repo as SxRepoEmployee<DbContext>).QueryForAdmin(filter);
            return View(viewModel);
        }

        [HttpPost]
        public virtual PartialViewResult Index(VMEmployee filterModel, IDictionary<string, SortDirection> order, int page = 1)
        {
            ViewBag.Filter = filterModel;
            ViewBag.Order = order;

            var filter = new WebCoreExtantions.Filter(page, _pageSize) { Orders = order, WhereExpressionObject = filterModel };
            var totalItems = (_repo as SxRepoEmployee<DbContext>).FilterCount(filter);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.PagerInfo = filter.PagerInfo;

            var viewModel = (_repo as SxRepoEmployee<DbContext>).QueryForAdmin(filter);

            return PartialView("_GridView", viewModel);
        }

        [HttpGet]
        public virtual ViewResult Edit(string id = null)
        {
            var model = id != null ? _repo.GetByKey(id) : new SxEmployee();
            return View(Mapper.Map<SxEmployee, VMEditEmployee>(model));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult Edit(VMEditEmployee model)
        {
            var redactModel = Mapper.Map<VMEditEmployee, SxEmployee>(model);
            if (ModelState.IsValid)
            {
                SxEmployee newModel = null;
                if (model.Id == null)
                    newModel = _repo.Create(redactModel);
                else
                    newModel = _repo.Update(redactModel, true, "Surname", "Name", "Patronymic", "Description");
                return RedirectToAction("Index");
            }
            else
                return View(model);
        }
    }
}