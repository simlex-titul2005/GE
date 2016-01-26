using AutoMapper;
using GE.WebAdmin.Models;
using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GE.WebAdmin.Controllers
{
    public partial class ArticlesController : Controller
    {
        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ViewResult Index(int page=1, int size=20)
        {
            var dbRepo = new RepoArticle();

            var list = dbRepo.All
                .Skip((page - 1) * size)
                .Take(size).ToArray().Select(x => Mapper.Map<Article, VMArticle>(x)).ToArray();

            ViewData["Page"] = page;
            ViewData["PageSize"] = size;
            ViewData["RowsCount"] = dbRepo.All.Count();

            return View(list);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public virtual PartialViewResult Index(VMArticle filterArticle, int page = 1, int size = 20)
        {
            int id = filterArticle != null ? filterArticle.Id : 0;
            string title = filterArticle != null ? filterArticle.Title : null;
            string html = filterArticle != null ? filterArticle.Html : null;
            ViewBag.Filter = filterArticle;
            
            var dbRepo = new RepoArticle();
            var temp = dbRepo.All;
            if (id != 0)
                temp = temp.Where(x => x.Id == id);
            if (title != null)
                temp = temp.Where(x => x.Title.Contains(title));
            if (html != null)
                temp = temp.Where(x => x.Html.Contains(html));

            var list = temp.Skip((page - 1) * size)
                .Take(size).ToArray().Select(x => Mapper.Map<Article, VMArticle>(x)).ToArray();

            ViewData["Page"] = page;
            ViewData["PageSize"] = size;
            ViewData["RowsCount"] = dbRepo.All.Count();

            return PartialView(MVC.Articles.Views._GridView, list);
        }
    }
}