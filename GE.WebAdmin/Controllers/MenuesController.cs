using GE.WebAdmin.Models;
using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using SX.WebCore;
using SX.WebCore.Abstract;
using SX.WebCore.HtmlHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
            var list = _repo.All
                .Skip((page - 1) * _pageSize)
                .Take(_pageSize).Select(x => Mapper.Map<SxMenu, VMMenu>(x)).ToArray();

            ViewData["Page"] = page;
            ViewData["PageSize"] = _pageSize;
            ViewData["RowsCount"] = _repo.All.Count();

            return View(list);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual PartialViewResult Index(VMMenu filter, IDictionary<string, SxExtantions.SortDirection> order, int page = 1)
        {
            string name = filter != null ? filter.Name : null;
            ViewBag.Filter = filter;
            ViewBag.Order = order;

            //select
            var temp = _repo.All;
            if (name != null)
                temp = temp.Where(x => x.Name.Contains(name));

            //order
            var orders = order.Where(x => x.Value != SxExtantions.SortDirection.Unknown);
            if (orders.Count() != 0)
            {
                foreach (var o in orders)
                {
                    if (o.Key == "Name")
                    {
                        if (o.Value == SxExtantions.SortDirection.Desc)
                            temp = temp.OrderByDescending(x => x.Name);
                        else if (o.Value == SxExtantions.SortDirection.Asc)
                            temp = temp.OrderBy(x => x.Name);
                    }
                }
            }

            var list = temp.Skip((page - 1) * _pageSize)
                .Take(_pageSize).Select(x => Mapper.Map<SxMenu, VMMenu>(x)).ToArray();

            ViewData["Page"] = page;
            ViewData["PageSize"] = _pageSize;
            ViewData["RowsCount"] = temp.Count();

            return PartialView("_GridView", list);
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