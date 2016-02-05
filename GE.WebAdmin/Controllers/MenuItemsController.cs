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
    public partial class MenuItemsController : Controller
    {
        private SxDbRepository<int, SxMenuItem, DbContext> _repo;
        public MenuItemsController()
        {
            _repo = new RepoMenuItem();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ViewResult Edit(int menuId, int? id)
        {
            var model = id.HasValue ? _repo.GetByKey(id) : new SxMenuItem { MenuId = menuId };
            var viewModel = Mapper.Map<SxMenuItem, VMEditMenuItem>(model);
            return View(viewModel);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Edit(VMEditMenuItem model)
        {
            var redactModel = Mapper.Map<VMEditMenuItem, SxMenuItem>(model);
            if (ModelState.IsValid)
            {
                SxMenuItem newModel = null;
                if (model.Id == 0)
                    newModel = _repo.Create(redactModel);
                else
                    newModel = _repo.Update(redactModel, "Name");
                return RedirectToAction("Edit", "Menues", new { @id = newModel.MenuId });
            }
            else
                return View(model);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual PartialViewResult Delete(int menuId, int menuItemId)
        {
            _repo.Delete(menuItemId);
            var viewModel = new VMMenuItemList();
            viewModel.Items = _repo.All.Where(x => x.MenuId == menuId).Select(x => Mapper.Map<SxMenuItem, VMMenuItem>(x)).ToArray();
            return PartialView(MVC.MenuItems.Views._MenuItems, viewModel);
        }
    }
}