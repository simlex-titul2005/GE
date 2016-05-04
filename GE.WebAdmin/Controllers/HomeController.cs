using GE.WebCoreExtantions;
using System.Web.Mvc;

namespace GE.WebAdmin.Controllers
{
    public partial class HomeController : BaseController
    {
        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ViewResult Index(DbContext dbContext)
        {
            return View();
        }
    }
}