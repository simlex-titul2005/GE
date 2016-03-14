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
    public partial class RedirectsController : BaseController
    {
        SxDbRepository<Guid, SxRedirect, DbContext> _repo;
        public RedirectsController()
        {
            _repo = new RepoRedirect();
        }

        private static int _pageSize = 20;

        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ViewResult Index(int page = 1)
        {
            var list = (_repo as RepoRedirect).QueryForAdmin(new GE.WebCoreExtantions.Filter { PageSize = _pageSize, SkipCount = (page - 1) * _pageSize })
                .Select(x => Mapper.Map<SxRedirect, VMRedirect>(x)).ToArray();

            ViewData["Page"] = page;
            ViewData["PageSize"] = _pageSize;
            ViewData["RowsCount"] = _repo.All.Count();

            return View(list);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual PartialViewResult Index(VMRedirect filterModel, IDictionary<string, SxExtantions.SortDirection> order, int page = 1)
        {
            string oldUrl = filterModel != null ? filterModel.OldUrl : null;
            string newUrl = filterModel != null ? filterModel.NewUrl : null;
            ViewBag.Filter = filterModel;
            ViewBag.Order = order;

            var filter = new GE.WebCoreExtantions.Filter { PageSize = _pageSize, SkipCount = (page - 1) * _pageSize };
            var list = (_repo as RepoRedirect).QueryForAdmin(filter)
                .Select(x => Mapper.Map<SxRedirect, VMRedirect>(x)).ToArray();

            ViewData["Page"] = page;
            ViewData["PageSize"] = _pageSize;
            ViewData["RowsCount"] = _repo.Count(null);

            return PartialView("_GridView", list);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ViewResult Edit(Guid? id)
        {
            var model = id.HasValue ? _repo.GetByKey(id) : new SxRedirect();
            var viewModel = Mapper.Map<SxRedirect, VMEditRedirect>(model);
            return View(viewModel);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Edit(VMEditRedirect model)
        {
            var redactModel = Mapper.Map<VMEditRedirect, SxRedirect>(model);
            if (ModelState.IsValid)
            {
                SxRedirect newModel = null;
                if (model.Id==Guid.Empty)
                    newModel = _repo.Create(redactModel);
                else
                    newModel = _repo.Update(redactModel, "OldUrl", "NewUrl");
                return RedirectToAction(MVC.Redirects.Index());
            }
            else
                return View(redactModel);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Delete(VMEditRedirect model)
        {
            _repo.Delete(model.Id);
            return RedirectToAction(MVC.Redirects.Index());
        }
    }
}