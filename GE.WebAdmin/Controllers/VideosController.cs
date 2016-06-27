using System.Web.Mvc;

namespace GE.WebAdmin.Controllers
{
    [Authorize(Roles = "video-redactor")]
    public partial class VideosController : SX.WebCore.MvcControllers.SxVideosController<WebCoreExtantions.DbContext>
    {
        
    }
}