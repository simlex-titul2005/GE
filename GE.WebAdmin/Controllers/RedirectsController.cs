using GE.WebAdmin.Models;
using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using SX.WebCore;
using SX.WebCore.Abstract;
using SX.WebCore.HtmlHelpers;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using GE.WebAdmin.Extantions.Repositories;
using SX.WebCore.Repositories;

namespace GE.WebAdmin.Controllers
{
    [Authorize(Roles = "seo")]
    public partial class RedirectsController : SX.WebCore.MvcControllers.Sx301RedirectsController<WebCoreExtantions.DbContext>
    {
        
    }
}