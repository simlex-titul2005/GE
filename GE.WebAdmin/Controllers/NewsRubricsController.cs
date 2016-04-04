using GE.WebAdmin.Extantions.Repositories;
using GE.WebAdmin.Models;
using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using SX.WebCore.Abstract;
using SX.WebCore.HtmlHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GE.WebAdmin.Controllers
{
    public partial class NewsRubricsController : BaseController
    {
        SxDbRepository<string, NewsRubric, DbContext> _repo;
        public NewsRubricsController()
        {
            _repo = new RepoNewsRubric();
        }

        private static int _pageSize = 20;

        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ViewResult Index(int page = 1)
        {
            var filter = new WebCoreExtantions.Filter(page, _pageSize);
            var totalItems = (_repo as RepoNewsRubric).FilterCount(filter);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.PagerInfo = filter.PagerInfo;

            var viewModel = (_repo as RepoNewsRubric).QueryForAdmin(filter);
            return View(viewModel);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual PartialViewResult Index(VMNewsRubric filterModel, IDictionary<string, SxExtantions.SortDirection> order, int page = 1)
        {
            ViewBag.Filter = filterModel;
            ViewBag.Order = order;

            var filter = new WebCoreExtantions.Filter(page, _pageSize) { Orders = order, WhereExpressionObject = filterModel };
            var totalItems = (_repo as RepoNewsRubric).FilterCount(filter);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.PagerInfo = filter.PagerInfo;

            var viewModel = (_repo as RepoNewsRubric).QueryForAdmin(filter);

            return PartialView("_GridView", viewModel);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ViewResult Edit(string id = null)
        {
            var model = id != null ? _repo.GetByKey(id) : new NewsRubric();
            return View(Mapper.Map<NewsRubric, VMEditNewsRubric>(model));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Edit(VMEditNewsRubric model)
        {
            var redactModel = Mapper.Map<VMEditNewsRubric, NewsRubric>(model);
            if (ModelState.IsValid)
            {
                NewsRubric newModel = null;
                var existModel = _repo.GetByKey(model.Id);
                if (existModel == null)
                    newModel = _repo.Create(redactModel);
                else
                {
                    existModel.Description = model.Description;
                    _repo.Update(existModel, "Description");
                }

                return RedirectToAction(MVC.NewsRubrics.Index());
            }
            else
                return View(model);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public virtual PartialViewResult Delete(VMEditNewsRubric model)
        {
            _repo.Delete(model.Id);

            var filter = new WebCoreExtantions.Filter(1, _pageSize);
            var totalItems = (_repo as RepoNewsRubric).FilterCount(filter);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.PagerInfo = filter.PagerInfo;

            var viewModel = (_repo as RepoNewsRubric).QueryForAdmin(filter);

            return PartialView(MVC.NewsRubrics.Views._GridView, viewModel);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual ViewResult FindTable(int page = 1, int pageSize = 10)
        {
            var viewModel = new SxExtantions.SxPagedCollection<VMNewsRubric>
            {
                Collection = _repo.All
                .OrderBy(x => x.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToArray()
                .Select(x => Mapper.Map<NewsRubric, VMNewsRubric>(x))
                .ToArray(),
                PagerInfo = new SxExtantions.SxPagerInfo(page, pageSize)
                {
                    TotalItems = _repo.All.Count(),
                    PagerSize = 4
                }
            };

            return View(viewModel);
        }
    }
}