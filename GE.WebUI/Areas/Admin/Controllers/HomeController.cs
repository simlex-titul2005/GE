using System.Web.Mvc;

namespace GE.WebUI.Areas.Admin.Controllers
{
    public sealed class HomeController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}