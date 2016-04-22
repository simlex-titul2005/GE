using GE.WebAdmin.Models;
using SX.WebCore.HtmlHelpers;
using SX.WebCore.Managers;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using System;
using Microsoft.AspNet.Identity;
using SX.WebCore;
using Microsoft.Owin.Security;

namespace GE.WebAdmin.Controllers
{
    [Authorize(Roles = "admin")]
    public partial class UsersController : BaseController
    {
        private static readonly string _architectRole = "architect";
        private SxAppUserManager _userManager;
        private SxAppRoleManager _roleManager;
        private SxAppUserManager UserManager
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
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private static int _pageSize = 10;
        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ViewResult Index(int page = 1)
        {
            var filter = new WebCoreExtantions.Filter(page, _pageSize);
            var users = UserManager.Users;
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
        public virtual ViewResult Edit(string id=null)
        {
            var data = UserManager.FindById(id);
            var allRoles = RoleManager.Roles.Where(x=>x.Name!=_architectRole).ToArray();
            ViewBag.Roles = allRoles;
            var viewModel = getEditUser(data, allRoles);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual PartialViewResult EditRoles(string userId)
        {
            var allRoles = RoleManager.Roles.Where(x => x.Name != _architectRole).ToArray();
            ViewBag.Roles = allRoles;

            var roles = Request.Form.GetValues("role");
            var data = UserManager.FindById(userId);
            var userRoles = data.Roles.Join(allRoles, r1 => r1.RoleId, r2 => r2.Id, (r1, r2) => new { Id = r1.RoleId, Name = r2.Name })
                .Where(x => x.Name != _architectRole).ToArray();
            List<string> rolesForDelete = new List<string>();
            List<string> rolesForAdd = new List<string>();
            for (int i = 0; i < userRoles.Length; i++)
            {
                var userRole = userRoles[i];
                if (roles.SingleOrDefault(x => x == userRole.Name) == null)
                    rolesForDelete.Add(userRole.Name);
            }

            for (int i = 0; i < roles.Length; i++)
            {
                var role = roles[i];
                if (userRoles.SingleOrDefault(x => x.Name == role) == null)
                    rolesForAdd.Add(role);
            }

            if (rolesForDelete.Any())
            {
                UserManager.RemoveFromRoles(userId, rolesForDelete.ToArray());
            }

            if (rolesForAdd.Any())
            {
                UserManager.AddToRoles(userId, rolesForAdd.ToArray());
            }

            if (rolesForDelete.Any() || rolesForAdd.Any())
            {
                TempData["UserRoleMessage"] = "Роли успешно заданы";
                data = UserManager.FindById(userId);
            }

            var viewModel = getEditUser(data, allRoles);
            return PartialView(MVC.Users.Views._UserRoles, viewModel);
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
            var users = UserManager.Users.Where(x => emails.Contains(x.Email)).ToArray();
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

        private static VMEditUser getEditUser(SxAppUser data, SxAppRole[] allRoles)
        {
            var editUser = new VMEditUser
            {
                Id = data.Id,
                AvatarId = data.AvatarId,
                Email = data.Email,
                NikName = data.NikName,
                IsOnline = MvcApplication.UsersOnSite.ContainsValue(data.UserName)
            };
            editUser.Roles = data.Roles.Join(allRoles, u => u.RoleId, r => r.Id, (u, r) => new VMRole
            {
                Id = u.RoleId,
                Name = r.Name,
                Description = r.Description
            }).ToArray();

            return editUser;
        }
    }
}
