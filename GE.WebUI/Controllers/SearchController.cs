using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using Dapper;
using GE.WebUI.Models;
using System.Web.SessionState;

namespace GE.WebUI.Controllers
{
    public partial class SearchController : BaseController
    {
        [AcceptVerbs(HttpVerbs.Get)]
        public virtual ActionResult List(string key)
        {
            if (!Request.IsAjaxRequest()) return null;

            if (string.IsNullOrEmpty(key)) return View(model: null);

            var query = @"SELECT
	dm.TitleUrl,
	dm.ModelCoreType,
	dm.DateCreate,
	dm.Title,
	dm.Foreword
FROM DV_MATERIAL AS dm
WHERE dm.Title LIKE '%'+@Key+'%' OR dm.Foreword LIKE '%'+@Key+'%'
ORDER BY dm.DateCreate DESC";
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DbContext"].ConnectionString))
            {
                var data = conn.Query<VMSearchResult>(query, new { Key = key }).ToArray();
                return View(data);
            }
        }
    }
}