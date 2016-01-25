using GE.WebCoreExtantions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GE.WebUI.Controllers
{
    public class HomeController : Controller
    {
        public ViewResult Index(DbContext dbContext)
        {
            dbContext = new DbContext();
            var dbRepo = new SX.WebCore.SxDbRepository<int, Article>(dbContext);
            return View(dbRepo.All);
        }
    }
}