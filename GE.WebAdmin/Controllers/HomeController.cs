using System.Web.Mvc;

namespace GE.WebAdmin.Controllers
{
    public sealed class HomeController : BaseController
    {
        [HttpGet]
        public ViewResult Index()
        {
            return View();
        }
    }
}