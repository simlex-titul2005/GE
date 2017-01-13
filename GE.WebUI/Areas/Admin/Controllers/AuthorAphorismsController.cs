using GE.WebUI.Infrastructure.Repositories;
using GE.WebUI.Models;
using GE.WebUI.ViewModels;
using SX.WebCore;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace GE.WebUI.Areas.Admin.Controllers
{
    [Authorize(Roles = "author-article")]
    public sealed class AuthorAphorismsController : BaseController
    {
        public static RepoAuthorAphorism Repo { get; set; }=new RepoAuthorAphorism();
        private static int _pageSize = 20;
        
        [HttpGet]
        public ViewResult Index(int page = 1)
        {
            var filter = new SxFilter(page, _pageSize) { Order = new SxOrderItem { FieldName = "Name", Direction = SortDirection.Asc } };
            var viewModel = Repo.Read(filter);
            ViewBag.Filter = filter;


            return View(viewModel);
        }

        [HttpPost]
        public PartialViewResult Index(VMAuthorAphorism filterModel, SxOrderItem order, int page = 1)
        {
            var filter = new SxFilter(page, _pageSize) { Order = order, WhereExpressionObject = filterModel };
            var viewModel = Repo.Read(filter);
            ViewBag.Filter = filter;

            return PartialView("_GridView", viewModel);
        }

        [HttpGet]
        public ViewResult Edit(int? id = null)
        {
            var data = id.HasValue ? Repo.GetByKey(id) : new AuthorAphorism();
            var viewModel = Mapper.Map<AuthorAphorism, VMAuthorAphorism>(data);
            if (data.Picture != null)
                ViewData["PictureIdCaption"] = data.Picture.Caption;
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(VMAuthorAphorism model)
        {
            var isNew = model.Id == 0;
            if (isNew || string.IsNullOrWhiteSpace(model.TitleUrl))
                model.TitleUrl = UrlHelperExtensions.SeoFriendlyUrl(model.Name);

            var existByTitleModel = await Repo.GetByTitleUrlAsync(model.TitleUrl);
            if (existByTitleModel != null)
            {
                if(!Equals(existByTitleModel.Id, model.Id))
                    ModelState.AddModelError("TitleUrl", "Запись с таким строковым ключем уже содержиться в БД");
            }

            var redactModel = Mapper.Map<VMAuthorAphorism, AuthorAphorism>(model);

            if (isNew)
            {
                var exist = await Repo.GetByNameAsync(model.Name);
                if (exist != null)
                {
                    ModelState.AddModelError("Name", "Автор с таким именем уже существует в БД");
                }
            }

            if (ModelState.IsValid)
            {
                AuthorAphorism newModel = null;
                if (isNew)
                    newModel = Repo.Create(redactModel);
                else
                    newModel = Repo.Update(redactModel, true, "Name", "Description", "PictureId", "TitleUrl", "Foreword");

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
            if (await Repo.GetByKeyAsync(model.Id) == null)
                return new HttpNotFoundResult();

            await Repo.DeleteAsync(model);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public PartialViewResult FindGridView(VMAuthorAphorism filterModel, SxOrderItem order, int page = 1, int pageSize = 10)
        {
            var defaultOrder = new SxOrderItem { FieldName = "Name", Direction = SortDirection.Asc };
            var filter = new SxFilter(page, pageSize) { WhereExpressionObject = filterModel, Order = order == null || order.Direction == SortDirection.Unknown ? defaultOrder : order };
            filter.PagerInfo.PagerSize = 5;

            var viewModel = Repo.Read(filter).ToArray();

            ViewBag.Filter = filter;

            return PartialView("_FindGridView", viewModel);
        }

        //[HttpPost]
        //public async Task<ActionResult> GenerateTitleUrl()
        //{
        //    return await Task.Run(() =>
        //    {

        //        using (var dbContext = new DbContext())
        //        {
        //            foreach (var empty in dbContext.AuthorAphorisms.Where(x => x.TitleUrl == null || x.TitleUrl == "" && x.Name != null))
        //            {
        //                empty.TitleUrl = UrlHelperExtensions.SeoFriendlyUrl(empty.Name);
        //                empty.Foreword = empty.Description == null ? "Не задано описание для автора афоризмов" : empty.Description.Length <= 400 ? empty.Description : empty.Description.Substring(0, 400);
        //            }
        //            dbContext.SaveChanges();
        //        }

        //        return RedirectToAction("Index");
        //    });
        //}
    }
}