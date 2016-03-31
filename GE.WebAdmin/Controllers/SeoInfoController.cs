using GE.WebAdmin.Models;
using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using SX.WebCore;
using SX.WebCore.Abstract;
using SX.WebCore.HtmlHelpers;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GE.WebAdmin.Extantions.Repositories;
using static SX.WebCore.Enums;

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
            var filter = new WebCoreExtantions.Filter { PageSize = _pageSize, SkipCount = (page - 1) * _pageSize };
            var list = (_repo as RepoSeoInfo).QueryForAdmin(filter).ToArray();

            ViewData["Page"] = page;
            ViewData["PageSize"] = _pageSize;
            ViewData["RowsCount"] = (_repo as RepoSeoInfo).FilterCount(filter);

            return View(list);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual PartialViewResult Index(VMSeoInfo filterModel, IDictionary<string, SxExtantions.SortDirection> order, int page = 1)
        {
            ViewBag.Filter = filterModel;
            ViewBag.Order = order;

            var filter = new WebCoreExtantions.Filter { PageSize = _pageSize, SkipCount = (page - 1) * _pageSize, Orders=order, WhereExpressionObject = filterModel };
            var list = (_repo as RepoSeoInfo).QueryForAdmin(filter).ToArray();

            ViewData["Page"] = page;
            ViewData["PageSize"] = _pageSize;
            ViewData["RowsCount"] = (_repo as RepoSeoInfo).FilterCount(filter);

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

        [AcceptVerbs(HttpVerbs.Get)]
        public virtual PartialViewResult EditForMaterial(int mid, ModelCoreType mct, int? id)
        {
            var model = id.HasValue ? _repo.GetByKey(id) : new SxSeoInfo();
            var seoInfo = Mapper.Map<SxSeoInfo, VMEditSeoInfo>(model);
            if (id.HasValue)
                seoInfo.Keywords = model.Keywords.Select(x => Mapper.Map<SxSeoKeyword, VMSeoKeyword>(x)).ToArray();
            return PartialView(MVC.SeoInfo.Views._EditForMaterial, seoInfo);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public virtual PartialViewResult EditForMaterial(VMEditSeoInfo model)
        {
            SxSeoInfo newModel = null;
            if (ModelState.IsValid)
            {
                var isNew = model.Id == 0;
                var redactModel = Mapper.Map<VMEditSeoInfo, SxSeoInfo>(model);
                if (isNew)
                {
                    newModel = _repo.Create(redactModel);
                    TempData["ModelSeoInfoRedactInfo"] = "Успешно добавлено";
                }
                else
                {
                    newModel = _repo.Update(redactModel, "SeoTitle", "SeoDescription", "H1", "H1CssClass");
                    TempData["ModelSeoInfoRedactInfo"] = "Успешно обновлено";
                }
                var viewModel = Mapper.Map<SxSeoInfo, VMEditSeoInfo>(newModel);
                return PartialView(MVC.SeoInfo.Views._EditForMaterial, viewModel);
            }
            else
                return PartialView(MVC.SeoInfo.Views._EditForMaterial, model);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public virtual PartialViewResult DeleteForMaterial(VMEditSeoInfo model)
        {
            _repo.Delete(model.Id);
            TempData["ModelSeoInfoRedactInfo"] = "Успешно удалено";
            return PartialView(MVC.SeoInfo.Views._EditForMaterial, new VMEditSeoInfo { MaterialId = model.MaterialId, ModelCoreType = model.ModelCoreType, Id=0 });
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