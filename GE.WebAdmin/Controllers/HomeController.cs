using Dapper;
using GE.WebCoreExtantions;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Linq;

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