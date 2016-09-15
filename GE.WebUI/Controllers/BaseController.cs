using SX.WebCore.MvcControllers;
using GE.WebUI.Infrastructure;

namespace GE.WebUI.Controllers
{
    public abstract partial class BaseController : SxBaseController<DbContext>
    {
        public BaseController()
        {
            WriteBreadcrumbs = BreadcrumbsManager.WriteBreadcrumbs;
        }
    }
}