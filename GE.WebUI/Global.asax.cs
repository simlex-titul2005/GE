using AutoMapper;
using GE.WebUI.Models;
using SX.WebCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
            SiteSettings.InitializeSiteSettings();
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
