using GE.WebCoreExtantions;
using System.Web.Mvc;
using SX.WebCore.MvcControllers;

namespace GE.WebAdmin.Controllers
{
    [Authorize(Roles = "seo")]
    public partial class RedirectsController : Sx301RedirectsController<DbContext>
    {
        
    }
}