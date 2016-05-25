using System.Web.Mvc;

namespace GE.WebUI.Controllers
{
    public class ContactsController : BaseController
    {
        [HttpGet]
        public ActionResult List()
        {
            return View();
        }
    }
}