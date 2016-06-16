using System.Web.Mvc;
using GE.WebUI.Extantions.Controllers;
using SX.WebCore.MvcControllers;
using GE.WebCoreExtantions;

namespace GE.WebUI.Controllers
{
    public abstract partial class BaseController : SxBaseController<DbContext>
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            this.WriteBreadcrumbs();
        }
    }
}