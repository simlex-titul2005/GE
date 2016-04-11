using GE.WebAdmin.Models;
using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using SX.WebCore;
using SX.WebCore.Abstract;
using System;
using System.Linq;
using System.Web.Mvc;

namespace GE.WebAdmin.Controllers
{
    public partial class MenuItemsController : BaseController
    {
        private SxDbRepository<int, SxMenuItem, DbContext> _repoMenuItem;
        private SxDbRepository<Guid, SxRoute, DbContext> _repoRoute;
        public MenuItemsController()
        {
            _repoMenuItem = new RepoMenuItem();
            _repoRoute = new RepoRoute();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ViewResult Edit(int menuId, int? id=null)
        {
            var model = id.HasValue ? _repoMenuItem.GetByKey(id) : new SxMenuItem { MenuId = menuId };
            var viewModel = Mapper.Map<SxMenuItem, VMEditMenuItem>(model);
            
            var routes = _repoRoute.All.Select(x => Mapper.Map<SxRoute, VMRoute>(x));
            var routesList = new SelectList(routes, "Id", "Name");
            ViewData["Routes"] = routesList;

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
                    newModel = _repoMenuItem.Create(redactModel);
                else
                    newModel = _repoMenuItem.Update(redactModel, "Caption", "RouteId", "Title", "Show");
                return RedirectToAction("Edit", "Menues", new { @id = newModel.MenuId });
            }
            else
            {
                var routes = _repoRoute.All.Select(x => Mapper.Map<SxRoute, VMRoute>(x));
                var routesList = new SelectList(routes, "Id", "Name");
                ViewData["Routes"] = routesList;
                return View(model);
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual PartialViewResult Delete(VMMenuItem model)
        {
            _repoMenuItem.Delete(model.Id);
            var viewModel = new VMMenuItemList { MenuId = model.MenuId };
            viewModel.Items = _repoMenuItem.All.Where(x => x.MenuId == model.MenuId).Select(x => Mapper.Map<SxMenuItem, VMMenuItem>(x)).ToArray();
            return PartialView(MVC.MenuItems.Views._MenuItems, viewModel);
        }
    }
}