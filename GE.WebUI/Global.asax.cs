using GE.WebCoreExtantions;
using SX.WebCore.MvcApplication;
using System;

namespace GE.WebUI
{
    public class MvcApplication : SxApplication<DbContext>
    {
        protected override void Application_Start(object sender, EventArgs e)
        {
            var args = new SxApplicationEventArgs();
            args.WebApiConfigRegister = WebApiConfig.Register;
            args.RegisterRoutes = RouteConfig.RegisterRoutes;
            args.MapperConfiguration = AutoMapperConfig.MapperConfigurationInstance;
            args.LoggingRequest = true;

            base.Application_Start(sender, args);
        }
    }
}
