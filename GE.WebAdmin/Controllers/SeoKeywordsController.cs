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
using GE.WebAdmin.Extantions.Repositories;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace GE.WebAdmin.Controllers
{
    public partial class SeoKeywordsController : BaseController
    {
        private SxDbRepository<int, SxSeoKeyword, DbContext> _repo;
        public SeoKeywordsController()
        {
            _repo = new RepoSeoKeywords();
        }

        private static readonly int _pageSize=10;
        [AcceptVerbs(HttpVerbs.Get)]
        public virtual PartialViewResult Index(int seoInfoId, int page = 1)
        {
            ViewBag.SeoInfoId = seoInfoId;

            var filter = new WebCoreExtantions.Filter(page, _pageSize){ WhereExpressionObject = new VMSeoKeyword { SeoInfoId = seoInfoId }};
            var totalItems = (_repo as RepoSeoKeywords).FilterCount(filter);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.PagerInfo = filter.PagerInfo;

            var viewModel = (_repo as RepoSeoKeywords).QueryForAdmin(filter);

            return PartialView(MVC.SeoKeywords.Views._GridView, viewModel);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual PartialViewResult Index(VMSeoKeyword filterModel, IDictionary<string, SortDirection> order, int page = 1)
        {
            ViewBag.SeoInfoId = filterModel.SeoInfoId;
            ViewBag.Filter = filterModel;
            ViewBag.Order = order;

            var filter = new WebCoreExtantions.Filter(page, _pageSize) { Orders=order, WhereExpressionObject = filterModel };
            var totalItems = (_repo as RepoSeoKeywords).FilterCount(filter);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.PagerInfo = filter.PagerInfo;

            var viewModel = (_repo as RepoSeoKeywords).QueryForAdmin(filter);

            return PartialView(MVC.SeoKeywords.Views._GridView, viewModel);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public virtual RedirectToRouteResult Edit(VMEditSeoKeyword model)
        {
            var redactModel = Mapper.Map<VMEditSeoKeyword, SxSeoKeyword>(model);
            if (ModelState.IsValid)
            {
                SxSeoKeyword newModel = null;
                if (model.Id == 0)
                {
                    var exist = _repo.All.FirstOrDefault(x => x.SeoInfoId == model.SeoInfoId && x.Value == model.Value)!=null;
                    if(!exist)
                        newModel = _repo.Create(redactModel);
                }
                else
                    newModel = _repo.Update(redactModel, true, "SeoInfoId", "Value");
            }

            return RedirectToAction(MVC.SeoKeywords.Index(seoInfoId: model.SeoInfoId));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public virtual RedirectToRouteResult Delete(VMEditSeoKeyword model)
        {
            var seoInfoId = model.SeoInfoId;
            _repo.Delete(model.Id);
            return RedirectToAction(MVC.SeoKeywords.Index(seoInfoId: seoInfoId));
        }
    }
}