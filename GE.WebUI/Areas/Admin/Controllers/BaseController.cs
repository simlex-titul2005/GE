using SX.WebCore.MvcControllers.Abstract;
using System.Web.Mvc;

namespace GE.WebUI.Areas.Admin.Controllers
{
    [Authorize]
    public abstract class BaseController : SxBaseController
    {
    }
}