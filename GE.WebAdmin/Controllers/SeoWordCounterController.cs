using System.Web.Mvc;
using System.Web.SessionState;

namespace GE.WebAdmin.Controllers
{
    [Authorize(Roles = "seo")]
    [SessionState(SessionStateBehavior.Default)]
    public partial class SeoWordCounterController : SX.WebCore.MvcControllers.SxSeoWordCounterController<WebCoreExtantions.DbContext>
    {
        
    }
}