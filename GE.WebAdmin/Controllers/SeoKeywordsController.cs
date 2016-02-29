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
    public partial class SeoKeywordsController : BaseController
    {
        private SxDbRepository<int, SxSeoKeyword, DbContext> _repo;
        public SeoKeywordsController()
        {
            _repo = new RepoSeoKeywords();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ViewResult Edit(int seoInfoId, int? id=null)
        {
            var model = id.HasValue ? _repo.GetByKey(id) : new SxSeoKeyword { SeoInfoId = seoInfoId };
            var viewModel = Mapper.Map<SxSeoKeyword, VMEditSeoKeyword>(model);
            return View(viewModel);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Edit(VMEditSeoKeyword model)
        {
            var redactModel = Mapper.Map<VMEditSeoKeyword, SxSeoKeyword>(model);
            if (ModelState.IsValid)
            {
                SxSeoKeyword newModel = null;
                if (model.Id == 0)
                    newModel = _repo.Create(redactModel);
                else
                    newModel = _repo.Update(redactModel, "SeoInfoId", "Value");
                return RedirectToAction(MVC.SeoInfo.Edit(model.SeoInfoId));
            }
            else
                return View(model);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual ActionResult Delete(VMEditSeoKeyword model)
        {
            var seoInfoId = model.SeoInfoId;
            _repo.Delete(model.Id);
            return RedirectToAction(MVC.SeoInfo.Edit(seoInfoId));
        }
    }
}