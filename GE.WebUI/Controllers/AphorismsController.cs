using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using GE.WebUI.Models;
using SX.WebCore.Abstract;
using SX.WebCore.Attrubutes;
using System.Web.Mvc;
using GE.WebUI.Extantions.Repositories;
using System.Linq;
using SX.WebCore;

namespace GE.WebUI.Controllers
{
    public partial class AphorismsController : BaseController
    {
        private SxDbRepository<int, Aphorism, DbContext> _repo;
        public AphorismsController()
        {
            _repo = new RepoAphorism();
        }

        [HttpGet]
        public virtual ViewResult Details(string categoryId, string titleUrl)
        {
            var vierwModel = (_repo as RepoAphorism).GetByTitleUrl(categoryId, titleUrl);
            return View(vierwModel);
        }

        [OutputCache(Duration = 900, VaryByParam = "curCat;onlyNotCurrent")]
        [HttpGet, ChildActionOnly]
        public virtual PartialViewResult Categories(string curCat = null, bool onlyNotCurrent = true)
        {
            var data = (_repo as RepoAphorism).GetAphorismCategories(curCat);
            var viewModel = onlyNotCurrent ? data.Where(x => !x.IsCurrent).ToArray() : data;

            return PartialView(MVC.Aphorisms.Views._Categories, viewModel);
        }

        private static int _pageSize = 20;
        [HttpGet]
        public virtual ViewResult List(string categoryId = null, int page = 1)
        {
            var author = Request.QueryString["author"];
            var filter = new WebCoreExtantions.Filter(page, _pageSize) { WhereExpressionObject = new VMAphorism { CategoryId = categoryId, Author=new VMAuthorAphorism { Name=author} } };
            var totalItems = (_repo as RepoAphorism).Count(filter);
            filter.PagerInfo.TotalItems = totalItems;
            filter.PagerInfo.PagerSize = 5;
            ViewBag.PagerInfo = filter.PagerInfo;
            ViewBag.AuthorName = author;

            var data = (_repo as RepoAphorism).Query(filter).ToArray();
            var viewModel = data.Select(x => Mapper.Map<Aphorism, VMAphorism>(x)).ToArray();
            ViewBag.CurrentAphorismCategory = categoryId == null ? null : Mapper.Map<SxMaterialCategory, VMMaterialCategory>(_repo.All.FirstOrDefault(x => x.CategoryId == categoryId).Category);
            return View(viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual RedirectToRouteResult Search(string author, string html)
        {
            return RedirectToAction("list", new { author = author, html = html });
        }
    }
}