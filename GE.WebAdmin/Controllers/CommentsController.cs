using Dapper;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace GE.WebAdmin.Controllers
{
    public partial class CommentsController : BaseController
    {
#if !DEBUG
        [OutputCache(Duration = 900)]
#endif
        [AcceptVerbs(HttpVerbs.Get)]
        public virtual PartialViewResult LastComments(int amount = 8)
        {
            var query = @"SELECT TOP(@amount) dc.DateCreate,
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
                var data = connection.Query<dynamic>(query, new { amount = amount }).ToArray();
                return PartialView("~/views/Comments/_LastComments.cshtml", data);
            }
        }
    }
}