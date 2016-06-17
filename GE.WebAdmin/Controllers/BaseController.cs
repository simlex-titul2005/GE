using GE.WebCoreExtantions;
using SX.WebCore.MvcControllers;
using System.Web.Mvc;
using System.Web.SessionState;

namespace GE.WebAdmin
{
    [Authorize]
    [SessionState(SessionStateBehavior.Disabled)]
    public abstract partial class BaseController : SxBaseController<DbContext>
    {
        
    }
}