using AutoMapper;
using System.Runtime.Caching;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace GE.WebUI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static MapperConfiguration _mapperConfiguration;
        private static MemoryCache _cache;
        protected void Application_Start()
        {
            _cache = new MemoryCache("GAME_EXE_CACHE");
            ErrorProvider.Configure(Server.MapPath("~/Logs"));
            _mapperConfiguration = AutoMapperConfig.MapperConfigurationInstance;

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        public static MapperConfiguration MapperConfiguration
        {
            get
            {
                return _mapperConfiguration;
            }
        }

        public static MemoryCache AppCache
        {
            get
            {
                return _cache;
            }
        }

        protected void Session_Start()
        {
            
        }

        protected void Application_Error()
        {
            var ex = Server.GetLastError();
            ErrorProvider.WriteMessage(ex);
        }
    }
}
