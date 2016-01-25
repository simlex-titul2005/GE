using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GE.WebUI.Controllers
{
    public partial class HomeController : Controller
    {
        public virtual ViewResult Index(DbContext dbContext)
        {
            var dbRepo = new RepoArticle();
            return View(dbRepo.All);
        }
    }
}