using GE.WebAdmin.Models;
using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using SX.WebCore;
using System.Linq;
using System.Web.Mvc;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace GE.WebAdmin.Controllers
{
    [Authorize(Roles = "admin")]
    public partial class AuthorAphorismsController : BaseController
    {
        private static int _pageSize = 20;
        private static RepoAuthorAphorism _repo;
        public AuthorAphorismsController()
        {
            if(_repo==null)
                _repo = new RepoAuthorAphorism();
        }

        [HttpGet]
        public virtual ViewResult Index(int page = 1)
        {
            var filter = new SxFilter(page, _pageSize);
            var totalItems = _repo.Count(filter);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.Filter = filter;

            var viewModel = _repo.Query(filter).ToArray().Select(x => Mapper.Map<AuthorAphorism, VMAuthorAphorism>(x)).ToArray();
            return View(viewModel);
        }

        [HttpPost]
        public virtual PartialViewResult Index(VMAuthorAphorism filterModel, SxOrder order, int page = 1)
        {
            var filter = new SxFilter(page, _pageSize) { Order = order, WhereExpressionObject = filterModel };
            var totalItems = _repo.Count(filter);
            filter.PagerInfo.TotalItems = totalItems;
            ViewBag.Filter = filter;

            var viewModel = (_repo as RepoAuthorAphorism).Query(filter).ToArray().Select(x => Mapper.Map<AuthorAphorism, VMAuthorAphorism>(x)).ToArray();

            return PartialView("_GridView", viewModel);
        }

        [HttpGet]
        public virtual ViewResult Edit(int? id = null)
        {
            var data = id.HasValue ? _repo.GetByKey(id) : new AuthorAphorism();
            var viewModel = Mapper.Map<AuthorAphorism, VMEditAuthorAphorism>(data);
            if (data.Picture != null)
                ViewData["PictureIdCaption"] = data.Picture.Caption;
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Edit(VMEditAuthorAphorism model)
        {
            var isNew = model.Id == 0;
            var redactModel = Mapper.Map<VMEditAuthorAphorism, AuthorAphorism>(model);

            if (isNew)
            {
                var exist = _repo.All.FirstOrDefault(x => x.Name == model.Name);
                if (exist != null)
                {
                    ModelState.AddModelError("Name", "Автор с таким именем уже существует в БД");
                }
            }

            if (ModelState.IsValid)
            {
                AuthorAphorism newModel = null;
                if (isNew)
                    newModel = _repo.Create(redactModel);
                else
                    newModel = _repo.Update(redactModel, true, "Name", "Description", "PictureId");

                return RedirectToAction("index");
            }
            else
            {
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Delete(VMEditAuthorAphorism model)
        {
            _repo.Delete(model.Id);
            return RedirectToAction("index");
        }

        [HttpPost]
        public virtual PartialViewResult FindGridView(VMAuthorAphorism filterModel, SxOrder order, int page = 1, int pageSize = 10)
        {
            var defaultOrder = new SxOrder { FieldName = "Name", Direction = SortDirection.Asc };
            var filter = new SxFilter(page, pageSize) { WhereExpressionObject= filterModel, Order= order==null || order.Direction==SortDirection.Unknown ? defaultOrder:order };
            var totalItems = (_repo as RepoAuthorAphorism).Count(filter);
            filter.PagerInfo.TotalItems = totalItems;
            filter.PagerInfo.PagerSize = 5;
            ViewBag.Filter = filter;

            var viewModel = _repo.Query(filter).Select(x => Mapper.Map<AuthorAphorism, VMAuthorAphorism>(x)).ToArray();

            return PartialView("_FindGridView", viewModel);
        }
    }
}