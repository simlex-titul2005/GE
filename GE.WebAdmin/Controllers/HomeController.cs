﻿using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GE.WebAdmin.Controllers
{
    public partial class HomeController : BaseController
    {
        public virtual ViewResult Index(DbContext dbContext)
        {
            return View();
        }
    }
}