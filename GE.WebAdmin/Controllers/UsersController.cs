using GE.WebAdmin.Models;
using SX.WebCore.HtmlHelpers;
using SX.WebCore.Managers;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using System;

namespace GE.WebAdmin.Controllers
{
    [Authorize(Roles = "admin")]
    public partial class UsersController : BaseController
    {
        private SxAppUserManager _userManager;
        private SxAppRoleManager _roleManager;
        private SxAppUserManager UsereManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().Get<SxAppUserManager>();
            }
            set
            {
                _userManager = value;
            }
        }
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

        private static int _pageSize = 10;
        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ViewResult Index(int page = 1)
        {
            var filter = new WebCoreExtantions.Filter(page, _pageSize);
            var users = UsereManager.Users;
            var roles = RoleManager.Roles.Select(x=> new { RoleId=x.Id, RoleName=x.Name }).ToArray();

            var data = users.OrderByDescending(x=>x.DateCreate).Skip((page - 1) * _pageSize).Take(_pageSize).ToArray()
                .Select(x => new VMUser {
                    Id=x.Id,
                    Email=x.Email,
                    NikName=x.NikName,
                    Roles= x.Roles.Select(r=>new VMRole { Id=r.RoleId }).ToArray()
                }).ToArray();
            for (int i = 0; i < data.Length; i++)
            {
                for (int y = 0; y < data[i].Roles.Length; y++)
                {
                    data[i].Roles[y].Name = roles.FirstOrDefault(r => r.RoleId == data[i].Roles[y].Id).RoleName;
                }
            }

            filter.PagerInfo.TotalItems= users.Count();
            ViewBag.PagerInfo = filter.PagerInfo;

            return View(data);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual PartialViewResult Index(VMUser filterModel, IDictionary<string, SxExtantions.SortDirection> order, int page = 1)
        {
            //string nikName = filterModel != null ? filterModel.NikName : null;
            //string desc = filterModel != null ? filterModel.Description : null;
            //ViewBag.Filter = filterModel;
            //ViewBag.Order = order;

            //var filter = new GE.WebCoreExtantions.Filter { PageSize = _rolePageSize, SkipCount = (page - 1) * _rolePageSize };
            //IQueryable<SxAppRole> roles = RoleManager.Roles;

            ////where clause
            //if (!string.IsNullOrEmpty(name))
            //    roles = roles.Where(x => x.Name.Contains(name));
            //if (!string.IsNullOrEmpty(desc))
            //    roles = roles.Where(x => x.Description.Contains(desc));

            ////order
            //if (order["Name"] != SxExtantions.SortDirection.Unknown)
            //    roles = order["Name"] == SxExtantions.SortDirection.Asc
            //        ? roles.OrderBy(x => x.Name)
            //        : roles.OrderByDescending(x => x.Name);
            //else if (order["Description"] != SxExtantions.SortDirection.Unknown)
            //    roles = order["Description"] == SxExtantions.SortDirection.Asc
            //        ? roles.OrderBy(x => x.Description)
            //        : roles.OrderByDescending(x => x.Description);
            //else
            //    roles = RoleManager.Roles.OrderBy(x => x.Name);

            //var list = roles.Skip((int)filter.SkipCount).Take(_rolePageSize).ToArray().Select(x => Mapper.Map<SxAppRole, VMRole>(x)).ToArray();

            //ViewData["Page"] = page;
            //ViewData["PageSize"] = _rolePageSize;
            //ViewData["RowsCount"] = roles.Count();

            //return PartialView(MVC.Roles.Views._GridView, list);
            throw new NotImplementedException();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ViewResult Edit(string id)
        {
            //var model = !string.IsNullOrEmpty(id) ? RoleManager.Roles.FirstOrDefault(x => x.Id == id) : new SxAppRole { Id = null };
            //var viewModel = Mapper.Map<SxAppRole, VMEditRole>(model);
            //return View(viewModel);
            throw new NotImplementedException();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Delete(VMEditRole model)
        {
            return RedirectToAction(MVC.Roles.Index());
            throw new NotImplementedException();
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public virtual PartialViewResult UsersOnSite()
        {
            var usersOnSite = MvcApplication.UsersOnSite;
            var emails = usersOnSite.Select(x => x.Value).ToArray();
            var users = UsereManager.Users.Where(x => emails.Contains(x.Email)).ToArray();
            var roles = RoleManager.Roles.ToArray();
            var viewModel = new List<VMUser>();
            foreach (var user in users)
            {
                viewModel.Add(new VMUser {
                    Id=user.Id,
                    AvatarId=user.AvatarId,
                    Email=user.Email,
                    NikName=user.NikName,
                    Roles= user.Roles.Join(roles, r=>r.RoleId, u=>u.Id, (r,u)=> new VMRole {
                        Id=u.Id,
                        Name=u.Name,
                        Description=u.Description
                    }).ToArray()
                });
            }

            return PartialView(MVC.Users.Views._UsersOnSite, viewModel.ToArray());
        }
    }
}
