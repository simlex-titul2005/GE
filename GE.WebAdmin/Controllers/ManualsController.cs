using GE.WebAdmin.Models;
using GE.WebCoreExtantions;
using SX.WebCore;
using SX.WebCore.Abstract;
using SX.WebCore.HtmlHelpers;
using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Linq;

namespace GE.WebAdmin.Controllers
{
    public partial class ManualsController : BaseController
    {
        SxDbRepository<int, SxManual, DbContext> _repo;
        public ManualsController()
        {
            _repo = new SX.WebCore.Repositories.RepoManual<DbContext>();
        }

        private static int _pageSize = 20;

        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ViewResult Index(int page = 1)
        {
            var filter = new WebCoreExtantions.Filter(page, _pageSize);
            var totalItems = (_repo as SX.WebCore.Repositories.RepoManual<DbContext>).Count(filter);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.PagerInfo = filter.PagerInfo;

            var viewModel = (_repo as SX.WebCore.Repositories.RepoManual<DbContext>).Query(filter).ToArray()
                .Select(x=>Mapper.Map<SxManual, VMManual>(x)).ToArray();
            return View(viewModel);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual PartialViewResult Index(VMManual filterModel, IDictionary<string, SxExtantions.SortDirection> order, int page = 1)
        {
            ViewBag.Filter = filterModel;
            ViewBag.Order = order;

            var filter = new WebCoreExtantions.Filter(page, _pageSize) { Orders = order, WhereExpressionObject = filterModel };
            var totalItems = (_repo as SX.WebCore.Repositories.RepoManual<DbContext>).Count(filter);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.PagerInfo = filter.PagerInfo;

            var viewModel = (_repo as SX.WebCore.Repositories.RepoManual<DbContext>).Query(filter).ToArray()
                .Select(x => Mapper.Map<SxManual, VMManual>(x)).ToArray();

            return PartialView("_GridView", viewModel);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ViewResult Edit(int? id)
        {
            var model = id.HasValue ? _repo.GetByKey(id, Enums.ModelCoreType.Manual) : new SxManual { ModelCoreType = Enums.ModelCoreType.Manual };
            var viewModel = Mapper.Map<SxManual, VMEditManual>(model);
            return View(viewModel);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Edit(VMEditManual model)
        {
            if (ModelState.IsValid)
            {
                var isNew = model.Id == 0;
                var redactModel = Mapper.Map<VMEditManual, SxManual>(model);

                SxManual newModel = null;

                if (isNew)
                {
                    var titleUrl = StringHelper.SeoFriendlyUrl(model.Title);
                    redactModel.UserId = User.Identity.GetUserId();
                    redactModel.TitleUrl = titleUrl;
                    newModel = _repo.Create(redactModel);
                }
                else
                {
                    newModel = _repo.Update(redactModel, "Title", "Html", "Foreword", "GroupId");
                }

                return RedirectToAction(MVC.Manuals.Index());
            }
            else
                return View(model);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Delete(VMEditManual model)
        {
            _repo.Delete(model.Id, model.ModelCoreType);
            return RedirectToAction(MVC.Manuals.Index());
        }
    }
}