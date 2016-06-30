using System.Web.Mvc;

namespace GE.WebAdmin.Controllers
{
    [Authorize(Roles = "admin")]
    public partial class SiteTestSubjectsController : SX.WebCore.MvcControllers.SxSiteTestSubjectsController<WebCoreExtantions.DbContext>
    {
        
    }
}