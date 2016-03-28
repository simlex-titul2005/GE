using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using SX.WebCore;
using SX.WebCore.Abstract;
using System.Web.Mvc;
using GE.WebAdmin.Extantions.Repositories;
using GE.WebAdmin.Models;
using System.Collections.Generic;
using SX.WebCore.HtmlHelpers;

namespace GE.WebAdmin.Controllers
{
    public partial class ForumPartsController : BaseController
    {
        private SxDbRepository<int, SxForumPart, DbContext> _repo;
        public ForumPartsController()
        {
            _repo = new RepoForumPart();
        }

        private static int _pageSize = 20;
        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ActionResult Index(int page = 1)
        {
            var filter = new GE.WebCoreExtantions.Filter { PageSize = _pageSize, SkipCount = (page - 1) * _pageSize };
            var data = (_repo as RepoForumPart).QueryForAdmin(filter);

            ViewData["Page"] = page;
            ViewData["PageSize"] = _pageSize;
            ViewData["RowsCount"] = (_repo as RepoForumPart).FilterCount(filter);

            return View(data);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual PartialViewResult Index(VMForumPart filterModel, IDictionary<string, SxExtantions.SortDirection> order, int page = 1)
        {
            string title = filterModel != null ? filterModel.Title : null;
            string html = filterModel != null ? filterModel.Html : null;
            ViewBag.Filter = filterModel;
            ViewBag.Order = order;

            var filter = new GE.WebCoreExtantions.Filter { PageSize = _pageSize, SkipCount = (page - 1) * _pageSize, WhereExpressionObject = filterModel };
            var data = (_repo as RepoForumPart).QueryForAdmin(filter, order);

            ViewData["Page"] = page;
            ViewData["PageSize"] = _pageSize;
            ViewData["RowsCount"] = (_repo as RepoForumPart).FilterCount(filter);

            return PartialView("_GridView", data);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ViewResult Edit(int? id)
        {
            var model = id.HasValue ? _repo.GetByKey(id) : new SxForumPart();
            var viewModel = Mapper.Map<SxForumPart, VMEditForumPart>(model);
            return View(viewModel);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Edit(VMEditForumPart model)
        {
            var redactModel = Mapper.Map<VMEditForumPart, SxForumPart>(model);
            if (ModelState.IsValid)
            {
                SxForumPart newModel = null;
                if (model.Id == 0)
                    newModel = _repo.Create(redactModel);
                else
                    newModel = _repo.Update(redactModel, "Title", "Html");

                return RedirectToAction(MVC.ForumParts.Index());
            }
            else
                return View(model);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Delete(VMEditSeoInfo model)
        {
            _repo.Delete(model.Id);
            return RedirectToAction(MVC.ForumParts.Index());
        }
    }
}