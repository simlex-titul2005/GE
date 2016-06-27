using System.Web.Mvc;

namespace GE.WebAdmin.Controllers
{
    [Authorize(Roles = "admin")]
    public partial class SiteTestQuestionsController : SX.WebCore.MvcControllers.SxSiteTestQuestionsController<WebCoreExtantions.DbContext>
    {
        
    }
}