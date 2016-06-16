using GE.WebCoreExtantions;
using SX.WebCore.MvcApplication;

namespace GE.WebUI
{
    public class MvcApplication : SxApplication<DbContext>
    {
        public MvcApplication() : base(
            WebApiConfig.Register,
            RouteConfig.RegisterRoutes,
            AutoMapperConfig.MapperConfigurationInstance,
            isLogRequests: true) { }
    }
}
