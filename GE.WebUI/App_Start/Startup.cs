using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using SX.WebCore.Managers;
using SX.WebCore;
using GE.WebUI.Infrastructure;

[assembly: OwinStartup(typeof(SX.WebCore.MvcApplication.SxOwinStartup<DbContext>))]