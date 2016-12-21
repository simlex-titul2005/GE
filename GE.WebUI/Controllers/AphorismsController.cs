using GE.WebUI.Models;
using System.Web.Mvc;
using SX.WebCore;
using System.Threading.Tasks;
using SX.WebCore.ViewModels;
using GE.WebUI.Infrastructure.Repositories;
using GE.WebUI.ViewModels;
using GE.WebUI.Infrastructure;

namespace GE.WebUI.Controllers
{
    public sealed class AphorismsController : BaseController
    {
        public static RepoAphorism Repo { get; set; } = new RepoAphorism();

        public AphorismsController()
        {
            FillBreadcrumbs = BreadcrumbsManager.WriteBreadcrumbs;
        }

        [HttpGet]
        public async Task<ActionResult> Details(string categoryId, string titleUrl)
        {
            var viewModel = await Repo.GetByTitleUrlAsync(categoryId, titleUrl);

            if (viewModel == null || viewModel.Aphorism == null)
                return new HttpNotFoundResult();

            ViewBag.Author = viewModel.Aphorism.Author;
            ViewBag.Category = viewModel.Aphorism.Category;

            //update views count
            if (!Request.IsLocal)
            {
                await Repo.AddUserViewAsync(viewModel.Aphorism.Id, MvcApplication.ModelCoreTypeProvider[nameof(Aphorism)], ()=> {
                    viewModel.Aphorism.ViewsCount++;
                });
            }
            return View(viewModel);
        }

#if !DEBUG
        [OutputCache(Duration = 900, VaryByParam = "curCat")]
#endif
        [HttpGet, ChildActionOnly]
        public PartialViewResult Categories(string curCat = null)
        {
            var viewModel = (Repo as RepoAphorism).GetAphorismCategories(curCat);

            return PartialView("_Categories", model: viewModel);
        }

        private static int _pageSize = 20;
        [HttpGet]
        public async Task<ActionResult> List(string categoryId = null, int page = 1)
        {
            var author = Request.QueryString["author"];
            if (!string.IsNullOrEmpty(author))
            {
                var a = await AuthorAphorismsController.Repo.GetByTitleUrlAsync(author);
                if (a == null) return new HttpNotFoundResult();
                ViewBag.Author = Mapper.Map<AuthorAphorism, VMAuthorAphorism>(a);
            }
            if (!string.IsNullOrEmpty(categoryId))
            {
                var c = await getCurrentCategory(categoryId);
                if (c == null) return new HttpNotFoundResult();
                ViewBag.Category = c;
            }

            var filter = new SxFilter(page, _pageSize)
            {
                WhereExpressionObject = new VMAphorism { CategoryId = categoryId, Author = new VMAuthorAphorism { TitleUrl = author } },
                OnlyShow = true,
                CategoryId=ViewBag.Category?.Id
            };
            filter.PagerInfo.PagerSize = 5;
            var viewModel = await Repo.ReadAsync(filter);

            ViewBag.PagerInfo = filter.PagerInfo;

            return View(viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public RedirectToRouteResult Search(string author, string html)
        {
            return RedirectToAction("List", new { author = author, html = html });
        }

        private async Task<SxVMMaterialCategory> getCurrentCategory(string categoryId = null)
        {
            if (categoryId == null) return null;

            var data = await MaterialCategoriesController.Repo.GetByKeyAsync(categoryId);
            return Mapper.Map<MaterialCategory, SxVMMaterialCategory>(data);
        }
    }
}