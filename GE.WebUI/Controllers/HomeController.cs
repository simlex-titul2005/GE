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