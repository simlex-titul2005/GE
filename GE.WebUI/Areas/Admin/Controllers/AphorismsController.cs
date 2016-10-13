using GE.WebUI.Models;
using GE.WebUI.ViewModels;
using SX.WebCore.Repositories;
using GE.WebUI.Infrastructure.Repositories;
using System.Web.Mvc;
using SX.WebCore;
using System.Linq;
using static SX.WebCore.Enums;
using System.Threading.Tasks;
using System;
using SX.WebCore.DbModels;

namespace GE.WebUI.Areas.Admin.Controllers
{
    public sealed class AphorismsController : MaterialsController<Aphorism, VMAphorism>
    {
        private static RepoAphorism _repo = new RepoAphorism();
        //TODO: изменить 6
        public AphorismsController() : base(6/*ModelCoreType.Aphorism*/) { }
        public override SxRepoMaterial<Aphorism, VMAphorism> Repo
        {
            get
            {
                return _repo;
            }
        }

        [NonAction]
        public override ActionResult Index(int page = 1)
        {
            return base.Index(page);
        }

        private static int _pageSize = 10;
        [HttpGet]
        public async Task<ActionResult> Index(int page = 1, string curCat = null)
        {
            if (curCat != null)
            {
                var category = Mapper.Map<SxMaterialCategory, VMMaterialCategory>(MaterialCategoriesController.Repo.GetByKey(curCat));
                ViewBag.Category = category;
            }

            ViewBag.CategoryId = curCat;
            var filter = new SxFilter(page, _pageSize) { WhereExpressionObject = new VMAphorism { CategoryId = curCat } };
            var viewModel = await Repo.ReadAsync(filter);
            if (page > 1 && !viewModel.Any())
                return new HttpNotFoundResult();

            ViewBag.Filter = filter;

            return View(viewModel);
        }

        [NonAction]
        public override Task<ActionResult> Index(VMAphorism filterModel, SxOrder order, int page = 1)
        {
            return base.Index(filterModel, order, page);
        }

        [HttpPost]
        public async Task<ActionResult> Index(string curCat, VMAphorism filterModel, SxOrder order, int page = 1)
        {
            filterModel.CategoryId = curCat;
            ViewBag.CategoryId = curCat;

            var filter = new SxFilter(page, _pageSize) { Order = order, WhereExpressionObject = filterModel };
            var viewModel = await Repo.ReadAsync(filter);
            if (page > 1 && !viewModel.Any())
                return new HttpNotFoundResult();

            ViewBag.Filter = filter;

            return PartialView("_GridView", viewModel);
        }

        protected override Action<VMAphorism> BeforeEditModel
        {
            get
            {
                return model => {
                    ViewBag.AuthorName = model.Author?.Name;
                    ViewBag.CategoryId = Request.QueryString.Get("cat");
                    ViewBag.DeleteInputs = "<input type=\"hidden\" name=\"CategoryId\" value=\""+ ViewBag.CategoryId + "\" />";
                };
            }
        }
        public override ActionResult Edit(int? id = default(int?))
        {
            ViewBag.Scripts = "$('#AuthorId').sx_gvl();";
            return base.Edit(id);
        }

        protected override string[] PropsForUpdate
        {
            get
            {
                return new string[] {
                    "Title", "Html", "Foreword", "AuthorId", "Show", "SourceUrl"
                };
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public override ActionResult Edit(VMAphorism model)
        {
            base.Edit(model);

            return RedirectToAction("Index", new { curCat = model.CategoryId });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public override async Task<ActionResult> Delete(Aphorism model)
        {
            await base.Delete(model);
            return RedirectToAction("Index", new { curCat = model.CategoryId });
        }
    }
}