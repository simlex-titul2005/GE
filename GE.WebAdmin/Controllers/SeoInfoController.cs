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
using GE.WebAdmin.Extantions.Repositories;

namespace GE.WebAdmin.Controllers
{
    public partial class SeoInfoController : BaseController
    {
        SxDbRepository<int, SxSeoInfo, DbContext> _repo;
        public SeoInfoController()
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

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual PartialViewResult Index(VMSeoInfo filterModel, IDictionary<string, SxExtantions.SortDirection> order, int page = 1)
        {
            string rawUrl = filterModel != null ? filterModel.RawUrl : null;
            ViewBag.Filter = filterModel;
            ViewBag.Order = order;

            var filter = new GE.WebCoreExtantions.Filter { PageSize = _pageSize, SkipCount = (page - 1) * _pageSize };
            var list = (_repo as RepoSeoInfo).QueryForAdmin(filter).ToArray();

            ViewData["Page"] = page;
            ViewData["PageSize"] = _pageSize;
            ViewData["RowsCount"] = _repo.Count(null);

            return PartialView("_GridView", list);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ViewResult Edit(int? id)
        {
            var model = id.HasValue ? _repo.GetByKey(id) : new SxSeoInfo();
            var seoInfo = Mapper.Map<SxSeoInfo, VMEditSeoInfo>(model);
            if(id.HasValue)
                seoInfo.Keywords = model.Keywords.Select(x => Mapper.Map<SxSeoKeyword, VMSeoKeyword>(x)).ToArray();
            return View(seoInfo);
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
                {
                    var existInfo = (_repo as RepoSeoInfo).GetByRawUrl(model.RawUrl);
                    if(existInfo!=null)
                    {
                        ModelState.AddModelError("RawUrl", "Информация для страницы с таким url уже содержится в БД");
                        return View(model);
                    }
                    newModel = _repo.Create(redactModel);
                }
                else
                    newModel = _repo.Update(redactModel, "RawUrl", "SeoTitle", "SeoDescription", "H1", "H1CssClass");

                return RedirectToAction(MVC.SeoInfo.Index());
            }
            else
                return View(model);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Delete(VMEditSeoInfo model)
        {
            _repo.Delete(model.Id);
            return RedirectToAction(MVC.SeoInfo.Index());
        }
    }
}