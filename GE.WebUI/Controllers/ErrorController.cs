using System.Web.Mvc;
using System.Web.SessionState;

namespace GE.WebUI.Controllers
{
    public partial class ErrorController : Controller
    {
        public virtual ActionResult NotFound()
        {
            Response.StatusCode = 404;
            return View();
        }

        public virtual ActionResult ServerError()
        {
            Response.StatusCode = 500;
            return View();
        }
    }
}