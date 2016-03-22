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
            var filter = new GE.WebCoreExtantions.Filter { SkipCount = (page - 1) * _pageSize, PageSize = _pageSize };
            var list = _repo.Query(filter).Select(x=>Mapper.Map<SxClickType, VMClickType>(x)).ToArray();

            ViewData["Page"] = page;
            ViewData["PageSize"] = _pageSize;
            ViewData["RowsCount"] = _repo.Count(filter);

            return View(list);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual PartialViewResult Index(VMClickType filter, IDictionary<string, SxExtantions.SortDirection> order, int page = 1)
        {
            //string name = filter != null ? filter.Name : null;
            ViewBag.Filter = filter;
            ViewBag.Order = order;

            //select
            //var temp = _repo.All;
            //if (name != null)
            //    temp = temp.Where(x => x.Name.Contains(name));

            //order
            //var orders = order.Where(x => x.Value != SxExtantions.SortDirection.Unknown);
            //if (orders.Count() != 0)
            //{
            //    foreach (var o in orders)
            //    {
            //        if (o.Key == "Id")
            //        {
            //            if (o.Value == SxExtantions.SortDirection.Desc)
            //                temp = temp.OrderByDescending(x => x.Name);
            //            else if (o.Value == SxExtantions.SortDirection.Asc)
            //                temp = temp.OrderBy(x => x.Name);
            //        }
            //    }
            //}

            var f = new GE.WebCoreExtantions.Filter { SkipCount = (page - 1) * _pageSize, PageSize = _pageSize };
            var list = _repo.Query(f).Select(x => Mapper.Map<SxClickType, VMClickType>(x)).ToArray();

            ViewData["Page"] = page;
            ViewData["PageSize"] = _pageSize;
            ViewData["RowsCount"] = _repo.Count(f);

            return PartialView("_GridView", list);
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