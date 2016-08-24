using System.Web.Mvc;

namespace GE.WebUI.Controllers
{
    public sealed class ForAuthorsController : Controller
    {
        [OutputCache(Duration = 3600)]
        public ActionResult Index()
        {
            return View();
        }
    }
}