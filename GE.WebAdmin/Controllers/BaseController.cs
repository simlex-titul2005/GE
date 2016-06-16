using GE.WebCoreExtantions;
using SX.WebCore.MvcControllers;
using System.Web.Mvc;

namespace GE.WebAdmin
{
    [Authorize]
    public abstract partial class BaseController : SxBaseController<DbContext>
    {
        
    }
}