using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GE.WebAdmin.Controllers
{
    public partial class FAQController : Controller
    {
        public virtual ActionResult Index()
        {
            return View();
        }
    }
}