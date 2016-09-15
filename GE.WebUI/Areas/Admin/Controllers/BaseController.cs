using GE.WebUI.Infrastructure;
using SX.WebCore.MvcControllers;

namespace GE.WebUI.Areas.Admin.Controllers
{
    public abstract class BaseController : SxBaseController<DbContext>
    {
    }
}