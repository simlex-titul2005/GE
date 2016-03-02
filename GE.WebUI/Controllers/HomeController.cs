using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GE.WebUI.Controllers
{
    public partial class HomeController : BaseController
    {
        public virtual ViewResult Index(string game=null)
        {
            return View();
        }
    }
}