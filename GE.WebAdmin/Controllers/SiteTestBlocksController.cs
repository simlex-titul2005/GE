using System.Web.Mvc;

namespace GE.WebAdmin.Controllers
{
    [Authorize(Roles = "admin")]
    public partial class SiteTestBlocksController : SX.WebCore.MvcControllers.SxSiteTestBlocksController<WebCoreExtantions.DbContext>
    {
        
    }
}