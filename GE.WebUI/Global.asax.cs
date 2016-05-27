using AutoMapper;
using SX.WebCore.Providers;
using System.Web.Mvc;
using System.Web.Routing;

namespace GE.WebUI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static MapperConfiguration _mapperConfiguration;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            _mapperConfiguration = AutoMapperConfig.MapperConfigurationInstance;
        }

        public static MapperConfiguration MapperConfiguration
        {
            get
            {
                return _mapperConfiguration;
            }
        }

        protected void Session_Start()
        {
            
        }
    }
}
