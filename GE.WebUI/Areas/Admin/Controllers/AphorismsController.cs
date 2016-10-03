using GE.WebUI.Models;
using GE.WebUI.ViewModels;
using SX.WebCore.MvcControllers;
using SX.WebCore.Repositories;
using GE.WebUI.Infrastructure.Repositories;
using System.Web.Mvc;
using SX.WebCore;
using System.Linq;
using System.Threading.Tasks;
using static SX.WebCore.Enums;
using Microsoft.AspNet.Identity;

namespace GE.WebUI.Areas.Admin.Controllers
{
    public sealed class AphorismsController : SxMaterialsController<Aphorism, VMAphorism>
    {
        public AphorismsController() : base(ModelCoreType.Aphorism) { }
        public override SxRepoMaterial<Aphorism, VMAphorism> Repo
        {
            get
            {
                return new RepoAphorism();
            }
        }

        private static int _pageSize = 10;
        //[HttpGet]
        //public ActionResult Index(int page = 1, string curCat = null)
        //{
        //    if (curCat != null)
        //    {
        //        var category = SxMaterialCategoriesController<VMMaterialCategory>.Repo.GetByKey(curCat);
        //        ViewBag.Category = category;
        //    }

        //    ViewBag.CategoryId = curCat;
        //    var filter = new SxFilter(page, _pageSize) { WhereExpressionObject = new VMAphorism { CategoryId = curCat } };
        //    var viewModel = Repo.Read(filter);
        //    if (page > 1 && !viewModel.Any())
        //        return new HttpNotFoundResult();

        //    ViewBag.Filter = filter;

        //    return View(viewModel);
        //}

        //[HttpPost]
        //public async Task<ActionResult> Index(string curCat, VMAphorism filterModel, SxOrder order, int page = 1)
        //{
        //    filterModel.CategoryId = curCat;
        //    ViewBag.CategoryId = curCat;

        //    var filter = new SxFilter(page, _pageSize) { Order = order, WhereExpressionObject = filterModel };
        //    var viewModel = await Repo.ReadAsync(filter);
        //    if (page > 1 && !viewModel.Any())
        //        return new HttpNotFoundResult();

        //    ViewBag.Filter = filter;

        //    return PartialView("_GridView", viewModel);
        //}

        [HttpGet]
        public ActionResult Edit(string cat, ModelCoreType mct, int? id = null)
        {
            if (cat == null) return new HttpNotFoundResult();

            var data = id.HasValue ? Repo.GetByKey(id, mct) : new Aphorism { CategoryId = cat, Show = true };
            if (data.Author != null)
                ViewBag.AuthorName = data.Author.Name;
            var viewModel = Mapper.Map<Aphorism, VMAphorism>(data);
            return View(viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(VMAphorism model)
        {
            if (string.IsNullOrEmpty(model.TitleUrl))
            {
                var titleUrl = Url.SeoFriendlyUrl(model.Title);
                var existModel = (Repo as RepoAphorism).GetExistsModel(titleUrl);
                if (existModel != null && existModel.Id != model.Id)
                    ModelState.AddModelError("Title", "Строковый ключ должен быть уникальным");
                else
                {
                    model.TitleUrl = titleUrl;
                    ModelState.Remove("TitleUrl");
                }
            }

            var isNew = model.Id == 0;
            var redactModel = Mapper.Map<VMAphorism, Aphorism>(model);
            redactModel.ModelCoreType = ModelCoreType.Aphorism;
            redactModel.UserId = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                if (isNew)
                    Repo.Create(redactModel);
                else
                    Repo.Update(redactModel, true, "Title", "Html", "Foreword", "AuthorId", "Show", "SourceUrl");
                return RedirectToAction("index", new { curCat = model.CategoryId });
            }
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Delete(string cat, int id, ModelCoreType mct)
        {
            var data = Repo.GetByKey(id, mct);
            if (data == null)
                return new HttpNotFoundResult();

            Repo.Delete(new Aphorism
            {
                Id = id,
                ModelCoreType = mct
            });
            return RedirectToAction("Index", new { curCat = cat });
        }
    }
}