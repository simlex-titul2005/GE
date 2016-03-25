using AutoMapper;
using System.Data.Entity;
using System.Web.Mvc;
using System.Web.Routing;

namespace GE.WebAdmin
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static MapperConfiguration _mapperConfiguration;
        protected void Application_Start()
        {
            Database.SetInitializer<GE.WebCoreExtantions.DbContext>(null);
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
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
    }
}
