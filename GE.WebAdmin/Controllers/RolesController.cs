using GE.WebAdmin.Models;
using SX.WebCore;
using SX.WebCore.HtmlHelpers;
using SX.WebCore.Managers;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using System;

namespace GE.WebAdmin.Controllers
{
    [Authorize(Roles = "admin")]
    public partial class RolesController : BaseController
    {
        private SxAppRoleManager _roleManager;
        private SxAppRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<SxAppRoleManager>();
            }
            set
            {
                _roleManager = value;
            }
        }

        private static int _rolePageSize = 10;
        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ViewResult Index(int page = 1)
        {
            var filter = new WebCoreExtantions.Filter(page, _rolePageSize);
            
            var roles = RoleManager.Roles.OrderBy(x => x.Name).ToArray();
            var data = roles.Select(x => Mapper.Map<SxAppRole, VMRole>(x)).ToArray();

            filter.PagerInfo.TotalItems = roles.Count();
            ViewBag.PagerInfo = filter.PagerInfo;

            return View(data);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual PartialViewResult Index(VMRole filterModel, IDictionary<string, SxExtantions.SortDirection> order, int page = 1)
        {
            string name = filterModel != null ? filterModel.Name : null;
            string desc = filterModel != null ? filterModel.Description : null;
            ViewBag.Filter = filterModel;
            ViewBag.Order = order;

            var filter = new WebCoreExtantions.Filter(page, _rolePageSize);
            IQueryable<SxAppRole> roles = RoleManager.Roles;

            //where clause
            if (!string.IsNullOrEmpty(name))
                roles=roles.Where(x => x.Name.Contains(name));
            if (!string.IsNullOrEmpty(desc))
                roles=roles.Where(x => x.Description.Contains(desc));

            //order
            if (order["Name"] != SxExtantions.SortDirection.Unknown)
                roles = order["Name"] == SxExtantions.SortDirection.Asc
                    ? roles.OrderBy(x => x.Name)
                    : roles.OrderByDescending(x => x.Name);
            else if (order["Description"] != SxExtantions.SortDirection.Unknown)
                roles = order["Description"] == SxExtantions.SortDirection.Asc
                    ? roles.OrderBy(x => x.Description)
                    : roles.OrderByDescending(x => x.Description);
            else
                roles = RoleManager.Roles.OrderBy(x => x.Name);

            var list = roles.Skip(filter.PagerInfo.SkipCount).Take(_rolePageSize).ToArray().Select(x => Mapper.Map<SxAppRole, VMRole>(x)).ToArray();

            filter.PagerInfo.TotalItems= roles.Count();
            ViewBag.PagerInfo = filter.PagerInfo;

            return PartialView(MVC.Roles.Views._GridView, list);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ViewResult Edit(string id)
        {
            var model = !string.IsNullOrEmpty(id) ? RoleManager.Roles.FirstOrDefault(x=>x.Id==id) : new SxAppRole { Id=null};
            var viewModel = Mapper.Map<SxAppRole, VMEditRole>(model);
            return View(viewModel);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> Edit(VMEditRole model)
        {
            var redactModel = Mapper.Map<VMEditRole, SxAppRole>(model);
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(model.Id))
                {
                    redactModel.Id = Guid.NewGuid().ToString();
                    await RoleManager.CreateAsync(redactModel);
                }
                else
                {
                    var oldRole = await RoleManager.FindByIdAsync(model.Id);
                    oldRole.Name = model.Name;
                    oldRole.Description = model.Description;
                    await RoleManager.UpdateAsync(oldRole);
                }
                return RedirectToAction(MVC.Roles.Index());
            }
            else
                return View(model);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Delete(VMEditRole model)
        {
            return RedirectToAction(MVC.Roles.Index());
        }
    }
}