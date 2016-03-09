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

        public virtual FileResult Robotstxt()
        {
            var fileContent = SiteSettings.Get(SX.WebCore.Resources.Settings.robotsFileSetting);
            if (fileContent != null)
                return File(Encoding.UTF8.GetBytes(fileContent.ToString()),
                     "text/plain",
                      string.Format("{0}.txt", "robots"));
            else return null;
        }
    }
}