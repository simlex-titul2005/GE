using GE.WebAdmin.Models;
using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using SX.WebCore;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace GE.WebAdmin.Controllers
{
    [Authorize(Roles = "admin")]
    public sealed class AuthorAphorismsController : BaseController
    {
        private static int _pageSize = 20;
        private static RepoAuthorAphorism _repo;
        public AuthorAphorismsController()
        {
            if (_repo == null)
                _repo = new RepoAuthorAphorism();
        }

        [HttpGet]
        public ViewResult Index(int page = 1)
        {
            var filter = new SxFilter(page, _pageSize) { Order = new SxOrder { FieldName = "Name", Direction = SortDirection.Asc } };
            var viewModel = _repo.Read(filter).ToArray().Select(x => Mapper.Map<AuthorAphorism, VMAuthorAphorism>(x)).ToArray();
            ViewBag.Filter = filter;

           
            return View(viewModel);
        }

        [HttpPost]
        public PartialViewResult Index(VMAuthorAphorism filterModel, SxOrder order, int page = 1)
        {
            var filter = new SxFilter(page, _pageSize) { Order = order, WhereExpressionObject = filterModel };
            var viewModel = (_repo as RepoAuthorAphorism).Read(filter).ToArray().Select(x => Mapper.Map<AuthorAphorism, VMAuthorAphorism>(x)).ToArray();
            ViewBag.Filter = filter;

            return PartialView("_GridView", viewModel);
        }

        [HttpGet]
        public ViewResult Edit(int? id = null)
        {
            var data = id.HasValue ? _repo.GetByKey(id) : new AuthorAphorism();
            var viewModel = Mapper.Map<AuthorAphorism, VMEditAuthorAphorism>(data);
            if (data.Picture != null)
                ViewData["PictureIdCaption"] = data.Picture.Caption;
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VMEditAuthorAphorism model)
        {
            var isNew = model.Id == 0;
            if (isNew || string.IsNullOrWhiteSpace(model.TitleUrl))
                model.TitleUrl = UrlHelperExtensions.SeoFriendlyUrl(model.Name);

            var existByTitleUrl = _repo.All.SingleOrDefault(x => x.TitleUrl == model.TitleUrl);
            if (existByTitleUrl != null)
            {
                ModelState.AddModelError("TitleUrl", "Запись с таким строковым ключем уже содержиться в БД");
                if (!isNew && existByTitleUrl.Id == model.Id)
                    ModelState["TitleUrl"].Errors.Clear();
            }


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
                    newModel = _repo.Update(redactModel, true, "Name", "Description", "PictureId", "TitleUrl", "Foreword");

                return RedirectToAction("index");
            }
            else
            {
                return View(model);
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(AuthorAphorism model)
        {
            if (await _repo.GetByKeyAsync(model.Id) == null)
                return new HttpNotFoundResult();

            await _repo.DeleteAsync(model);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public PartialViewResult FindGridView(VMAuthorAphorism filterModel, SxOrder order, int page = 1, int pageSize = 10)
        {
            var defaultOrder = new SxOrder { FieldName = "Name", Direction = SortDirection.Asc };
            var filter = new SxFilter(page, pageSize) { WhereExpressionObject = filterModel, Order = order == null || order.Direction == SortDirection.Unknown ? defaultOrder : order };
            filter.PagerInfo.PagerSize = 5;

            var viewModel = _repo.Read(filter).Select(x => Mapper.Map<AuthorAphorism, VMAuthorAphorism>(x)).ToArray();

            ViewBag.Filter = filter;

            return PartialView("_FindGridView", viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> GenerateTitleUrl()
        {
            return await Task.Run(() =>
            {

                using (var dbContext = new DbContext())
                {
                    foreach (var empty in dbContext.AuthorAphorisms.Where(x => x.TitleUrl == null || x.TitleUrl == "" && x.Name != null))
                    {
                        empty.TitleUrl = UrlHelperExtensions.SeoFriendlyUrl(empty.Name);
                        empty.Foreword = empty.Description == null ? "Не задано описание для автора афоризмов" : empty.Description.Length <= 400 ? empty.Description : empty.Description.Substring(0, 400);
                    }
                    dbContext.SaveChanges();
                }

                return RedirectToAction("Index");
            });
        }
    }
}