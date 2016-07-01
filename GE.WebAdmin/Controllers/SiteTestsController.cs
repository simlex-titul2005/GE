using System.Web.Mvc;

namespace GE.WebAdmin.Controllers
{
    [Authorize(Roles = "admin")]
    public partial class SiteTestsController : SX.WebCore.MvcControllers.SxSiteTestsController<WebCoreExtantions.DbContext>
    {
        
    }
}