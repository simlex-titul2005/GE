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
        public virtual ViewResult Index(DbContext dbContext)
        {
            var dbRepo = new RepoArticle();
            var list = dbRepo.All.ToArray().Select(x => Mapper.Map<Article, VMArticle>(x)).ToArray();
            return View(list);
        }
    }
}