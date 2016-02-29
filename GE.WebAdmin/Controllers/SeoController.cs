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
    public partial class SeoController : BaseController
    {
        SxDbRepository<int, SxSeoInfo, DbContext> _repo;
        public SeoController()
        {
            _repo = new RepoSeoInfo();
        }

        private static int _pageSize = 20;
        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ViewResult Index(int page = 1)
        {
            var list = _repo.All
                .Skip((page - 1) * _pageSize)
                .Take(_pageSize).Select(x => Mapper.Map<SxSeoInfo, VMSeoInfo>(x)).ToArray();

            ViewData["Page"] = page;
            ViewData["PageSize"] = _pageSize;
            ViewData["RowsCount"] = _repo.All.Count();

            return View(list);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ViewResult Edit(int? id)
        {
            var model = id.HasValue ? _repo.GetByKey(id) : new SxSeoInfo();
            return View(Mapper.Map<SxSeoInfo, VMEditSeoInfo>(model));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Edit(VMEditSeoInfo model)
        {
            var redactModel = Mapper.Map<VMEditSeoInfo, SxSeoInfo>(model);
            if (ModelState.IsValid)
            {
                SxSeoInfo newModel = null;
                if (model.Id == 0)
                    newModel = _repo.Create(redactModel);
                else
                    newModel = _repo.Update(redactModel, "RawUrl", "SeoTitle", "SeoDescription");

                return RedirectToAction(MVC.Seo.Index());
            }
            else
                return View(model);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Delete(VMEditSeoInfo model)
        {
            _repo.Delete(model.Id);
            return RedirectToAction(MVC.Seo.Index());
        }
    }
}