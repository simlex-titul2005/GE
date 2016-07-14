using GE.WebCoreExtantions;
using SX.WebCore.MvcControllers;
using System.Web.Mvc;

namespace GE.WebAdmin.Controllers
{
    [Authorize(Roles = "admin")]
    public partial class SiteTestsController : SxSiteTestsController<DbContext>
    {
        
    }
}