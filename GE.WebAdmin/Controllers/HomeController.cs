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

        [AcceptVerbs(HttpVerbs.Get)]
        public virtual PartialViewResult LastComments()
        {
            var query = @"SELECT dc.DateCreate,
       dc.UserName,
       dc.Html,
       dm.Id,
       dm.ModelCoreType
FROM   DV_MATERIAL            AS dm
       JOIN D_COMMENT         AS dc
            ON  dc.MaterialId = dm.Id
            AND dc.ModelCoreType = dm.ModelCoreType
       LEFT JOIN AspNetRoles  AS anr
            ON  anr.Id = dc.UserId
ORDER BY
       dc.DateCreate DESC";

            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DbContext"].ConnectionString))
            {
                var data = connection.Query<dynamic>(query).ToArray();
                return PartialView(MVC.Home.Views._LastComments, data);
            }
        }
    }
}