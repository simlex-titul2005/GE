using System.Web.Mvc;

namespace GE.WebUI.Controllers
{
    public sealed class HomeController : BaseController
    {
        public ViewResult Index(string game=null)
        {
            return View();
        }
    }
}