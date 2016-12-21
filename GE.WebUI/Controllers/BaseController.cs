using System;
using System.Collections.Generic;
using GE.WebUI.Infrastructure;
using SX.WebCore.MvcControllers.Abstract;
using SX.WebCore.ViewModels;

namespace GE.WebUI.Controllers
{
    public abstract class BaseController : SxBaseController
    {
        protected override Action<SxBaseController, HashSet<SxVMBreadcrumb>> FillBreadcrumbs
            => BreadcrumbsManager.WriteBreadcrumbs;
    }
}