using GE.WebCoreExtantions;
using SX.WebCore.MvcControllers;
using System.Web.Mvc;

namespace GE.WebAdmin.Controllers
{
    [Authorize(Roles ="seo")]
    public partial class SeoTagsController : SxSeoTagsController<DbContext>
    {
        
    }
}