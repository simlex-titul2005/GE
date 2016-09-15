﻿using GE.WebUI.Infrastructure;
using SX.WebCore;
using SX.WebCore.MvcControllers;
using System.Linq;
using System.Web.Mvc;

namespace GE.WebUI.Controllers
{
    public sealed class EmployeesController : SxEmployeesController<DbContext>
    {

        private static readonly int _pageSize = 10;
#if !DEBUG
        [OutputCache(Duration =900)]
#endif
        [HttpGet, AllowAnonymous]
        public ActionResult List(int page=1)
        {
            var filter = new SxFilter(page, _pageSize);
            var viewModel = Repo.Read(filter);
            if (page > 1 && !viewModel.Any())
                return new HttpNotFoundResult();

            ViewBag.Filter = filter;

            return View(viewModel);
        }
    }
}