using GE.WebUI.Infrastructure;
using SX.WebCore.MvcApplication;
using System;

namespace GE.WebUI
{
    public class MvcApplication : SxMvcApplication
    {
        private static readonly string[] _defNamespace = new string[] { "GE.WebUI.Controllers" };

        protected override void Application_Start(object sender, EventArgs e)
        {
            var args = new SxApplicationEventArgs
            {
                GetDbContextInstance = () => { return new DbContext(); },
                WebApiConfigRegister = WebApiConfig.Register,
                MapperConfigurationExpression = AutoMapperConfig.Register,

                //routes
                DefaultControllerNamespaces = _defNamespace,
                PreRouteAction=RouteConfig.PreRouteAction,
                PostRouteAction = RouteConfig.PostRouteAction
            };

            base.Application_Start(sender, args);
        }
    }
}
