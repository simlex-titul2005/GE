using GE.WebCoreExtantions;
using System.Web.Mvc;

namespace GE.WebAdmin.Controllers
{
    public partial class HomeController : BaseController
    {
        [HttpGet]
        public virtual ViewResult Index()
        {
            return View();
        }
    }
}