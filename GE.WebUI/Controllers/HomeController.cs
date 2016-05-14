using System.Web.Mvc;
using System.Web.SessionState;

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