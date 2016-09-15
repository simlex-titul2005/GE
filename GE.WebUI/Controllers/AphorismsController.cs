﻿using GE.WebUI.Models;
using System.Web.Mvc;
using System.Linq;
using SX.WebCore;
using System.Threading.Tasks;
using SX.WebCore.ViewModels;
using GE.WebUI.Infrastructure.Repositories;
using GE.WebUI.ViewModels;

namespace GE.WebUI.Controllers
{
    public sealed class AphorismsController : BaseController
    {
        private static RepoAphorism _repo = new RepoAphorism();
        public RepoAphorism Repo
        {
            get { return _repo; }
            set { _repo = value; }
        }

        [HttpGet]
        public ActionResult Details(string categoryId, string titleUrl)
        {
            var viewModel = Repo.GetByTitleUrl(categoryId, titleUrl);

            if (viewModel == null || viewModel.Aphorism == null)
                return new HttpNotFoundResult();

            if (viewModel.Aphorism.Author!=null)
                ViewBag.Author = viewModel.Aphorism.Author;
            else
                ViewBag.Category = getCurrentCategory(categoryId);

            //update views count
            if (!Request.IsLocal)
            {
                Task.Run(() =>
                {
                    (_repo as RepoAphorism).AddUserView(viewModel.Aphorism.Id, Enums.ModelCoreType.Aphorism);
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
            var viewModel = (_repo as RepoAphorism).GetAphorismCategories(curCat);

            return PartialView("_Categories", model: viewModel);
        }

        private static int _pageSize = 20;
        [HttpGet]
        public ViewResult List(string categoryId = null, int page = 1)
        {
            var author = Request.QueryString["author"];
            if (!string.IsNullOrWhiteSpace(author))
                ViewBag.Author = Mapper.Map<AuthorAphorism, VMAuthorAphorism>(new RepoAuthorAphorism().GetByTitleUrl(author));
            if(!string.IsNullOrWhiteSpace(categoryId))
                ViewBag.Category = getCurrentCategory(categoryId);


            var filter = new SxFilter(page, _pageSize) {
                WhereExpressionObject = new VMAphorism { CategoryId = categoryId, Author = new VMAuthorAphorism { TitleUrl = author } },
                OnlyShow = true
            };
            filter.PagerInfo.PagerSize = 5;
            var viewModel = _repo.Read(filter);

            ViewBag.PagerInfo = filter.PagerInfo;
            
            return View(viewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public RedirectToRouteResult Search(string author, string html)
        {
            return RedirectToAction("List", new { author = author, html = html });
        }

        private SxVMMaterialCategory getCurrentCategory(string categoryId=null)
        {
            if (categoryId == null) return null;

            var data = new RepoMaterialCategory().GetByKey(categoryId);
            return Mapper.Map<MaterialCategory, SxVMMaterialCategory>(data);
        }
    }
}