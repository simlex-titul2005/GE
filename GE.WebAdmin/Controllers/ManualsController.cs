using GE.WebAdmin.Models;
using GE.WebCoreExtantions;
using SX.WebCore;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Linq;
using static SX.WebCore.HtmlHelpers.SxExtantions;
using SX.WebCore.Repositories;

namespace GE.WebAdmin.Controllers
{
    public partial class ManualsController : BaseController
    {
        private static SxRepoManual<DbContext> _repo;
        public ManualsController()
        {
            if(_repo==null)
                _repo = new SxRepoManual<DbContext>();
        }

        private static int _pageSize = 20;

        [HttpGet]
        public virtual ViewResult Index(int page = 1)
        {
            var defaultOrder = new SxOrder { FieldName = "dm.DateCreate", Direction = SortDirection.Desc };
            var filter = new SxFilter(page, _pageSize) { Order=defaultOrder};

            var viewModel = _repo.Read(filter).Select(x=>Mapper.Map<SxManual, VMManual>(x)).ToArray();

            ViewBag.Filter = filter;

            return View(viewModel);
        }

        [HttpPost]
        public virtual PartialViewResult Index(VMManual filterModel, SxOrder order, int page = 1)
        {
            var defaultOrder = new SxOrder { FieldName = "dm.DateCreate", Direction = SortDirection.Desc };
            var filter = new SxFilter(page, _pageSize) { Order = order==null || order.Direction==SortDirection.Unknown?defaultOrder:order, WhereExpressionObject = filterModel };

            var viewModel = _repo.Read(filter).Select(x => Mapper.Map<SxManual, VMManual>(x)).ToArray();

            ViewBag.Filter = filter;

            return PartialView("_GridView", viewModel);
        }

        [HttpGet]
        public virtual ViewResult Edit(int? id)
        {
            var model = id.HasValue ? _repo.GetByKey(id, Enums.ModelCoreType.Manual) : new SxManual { ModelCoreType = Enums.ModelCoreType.Manual };
            var viewModel = Mapper.Map<SxManual, VMEditManual>(model);
            ViewBag.ModelCoreType = model.ModelCoreType;
            if (model.Category != null)
                ViewBag.MaterialCategoryTitle = model.Category.Title;
            return View(viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult Edit(VMEditManual model)
        {
            if (ModelState.IsValid)
            {
                var isNew = model.Id == 0;
                var redactModel = Mapper.Map<VMEditManual, SxManual>(model);

                SxManual newModel = null;

                if (isNew)
                {
                    var titleUrl = Url.SeoFriendlyUrl(model.Title);
                    redactModel.UserId = User.Identity.GetUserId();
                    redactModel.TitleUrl = titleUrl;
                    newModel = _repo.Create(redactModel);
                }
                else
                {
                    newModel = _repo.Update(redactModel, true, "Title", "Html", "Foreword", "CategoryId");
                }

                return RedirectToAction("index");
            }
            else
                return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult Delete(VMEditManual model)
        {
            _repo.Delete(model.Id, model.ModelCoreType);
            return RedirectToAction("index");
        }
    }
}