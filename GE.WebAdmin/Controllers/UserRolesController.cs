using System.Web.Mvc;

namespace GE.WebAdmin.Controllers
{
    [Authorize(Roles = "admin")]
    public partial class UserRolesController : SX.WebCore.MvcControllers.SxUserRolesController<WebCoreExtantions.DbContext>
    {
        
    }
}