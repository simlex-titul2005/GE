using AutoMapper;
using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using GE.WebUI.Models;
using SX.WebCore;
using SX.WebCore.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GE.WebUI.Controllers
{
    public partial class MenuesController : BaseController
    {
        private SxDbRepository<int, SxMenu, DbContext> _repo;
        public MenuesController()
        {
            _repo = new RepoMenu();
        }

        [OutputCache(Duration = 900, VaryByParam = "menuMarker;cssClass")]
        [ChildActionOnly]
        public virtual PartialViewResult Menu(int menuMarker, string cssClass=null)
        {
            ViewBag.MenuCssClass = cssClass;
            var menu = _repo.GetByKey(menuMarker);
            var viewModel = Mapper.Map<SxMenu, VMMenu>(menu);
            return PartialView(MVC.Menues.Views._MenuItems, viewModel);
        }
    }
}