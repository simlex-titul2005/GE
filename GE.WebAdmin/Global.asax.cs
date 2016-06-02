using AutoMapper;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Data.Entity;
using System.Runtime.Caching;
using System.Web.Mvc;
using System.Web.Routing;

namespace GE.WebAdmin
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static Dictionary<string, string> _usersOnSite;
        private static MemoryCache _cache;
        public static Dictionary<string, string> UsersOnSite
        {
            get
            {
                if (_usersOnSite == null)
                    _usersOnSite = new Dictionary<string, string>();

                return _usersOnSite;
            }
        }

        private static MapperConfiguration _mapperConfiguration;
        protected void Application_Start()
        {
            _cache = new MemoryCache("SXADMIN_CACHE");
            ErrorProvider.Configure(Server.MapPath("~/Logs"));
            Database.SetInitializer<WebCoreExtantions.DbContext>(null);
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

        public static MemoryCache AppCache
        {
            get
            {
                return _cache;
            }
        }

        protected void Session_Start()
        {
            var sessionId = Session.SessionID;
            if (!UsersOnSite.ContainsKey(sessionId))
                UsersOnSite.Add(sessionId, null);

            if (User.Identity.IsAuthenticated)
                UsersOnSite[sessionId] = User.Identity.GetUserName();
        }

        protected void Session_End()
        {
            var sessionId = Session.SessionID;
            if (UsersOnSite.ContainsKey(sessionId))
                UsersOnSite.Remove(sessionId);
        }

        protected void Application_Error()
        {
            var ex = Server.GetLastError();
            ErrorProvider.WriteMessage(ex);
        }
    }
}
