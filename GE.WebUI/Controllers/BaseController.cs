using GE.WebUI.Infrastructure;
using SX.WebCore.MvcControllers.Abstract;

namespace GE.WebUI.Controllers
{
    public abstract class BaseController : SxBaseController
    {
        public BaseController()
        {
            FillBreadcrumbs = BreadcrumbsManager.WriteBreadcrumbs;
        }
    }
}