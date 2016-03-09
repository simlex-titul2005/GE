using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GE.WebUI.Controllers
{
    public partial class ErrorController : Controller
    {
        public virtual ActionResult NotFound()
        {
            Response.StatusCode = 404;
            return View();
        }
    }
}