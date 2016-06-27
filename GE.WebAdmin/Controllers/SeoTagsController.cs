using System.Web.Mvc;

namespace GE.WebAdmin.Controllers
{
    [Authorize(Roles ="seo")]
    public partial class SeoTagsController : SX.WebCore.MvcControllers.SxSeoTagsController<WebCoreExtantions.DbContext>
    {
        
    }
}