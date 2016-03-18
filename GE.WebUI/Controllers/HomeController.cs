using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

        [OutputCache(Duration =900)]
        public virtual ContentResult Robotstxt()
        {
            var fileContent = SiteSettings.Get(SX.WebCore.Resources.Settings.robotsFileSetting);
            if (fileContent != null)
                return Content(fileContent.Value, "text/plain", Encoding.UTF8);
            else return null;
        }
    }
}