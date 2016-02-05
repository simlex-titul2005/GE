using AutoMapper;
using GE.WebAdmin.Models;
using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using SX.WebCore;
using SX.WebCore.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GE.WebAdmin.Controllers
{
    public partial class MenuesController : Controller
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
                .Take(_pageSize).ToArray().Select(x => Mapper.Map<SxMenu, VMMenu>(x)).ToArray();

            ViewData["Page"] = page;
            ViewData["PageSize"] = _pageSize;
            ViewData["RowsCount"] = _repo.All.Count();

            return View(list);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ViewResult Edit(int? id)
        {
            var model = id.HasValue ? _repo.GetByKey(id) : new SxMenu();
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
                    newModel = _repo.Update(redactModel, "Name");
                return RedirectToAction("Index", new { @redactedId = newModel.Id });
            }
            else
                return View(model);
        }
    }
}