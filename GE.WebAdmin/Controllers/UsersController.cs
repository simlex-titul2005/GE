using System.Web.Mvc;

namespace GE.WebAdmin.Controllers
{
    [Authorize(Roles = "admin")]
    public partial class UsersController : SX.WebCore.MvcControllers.SxUsersController<WebCoreExtantions.DbContext>
    {
        
    }
}
