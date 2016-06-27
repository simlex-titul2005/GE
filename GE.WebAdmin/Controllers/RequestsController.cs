using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using SX.WebCore;
using SX.WebCore.Abstract;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using GE.WebAdmin.Extantions.Repositories;
using GE.WebAdmin.Models;
using SX.WebCore.HtmlHelpers;

namespace GE.WebAdmin.Controllers
{
    [Authorize(Roles ="seo")]
    public partial class RequestsController : SX.WebCore.MvcControllers.SxRequestsController<WebCoreExtantions.DbContext>
    {
        
    }
}