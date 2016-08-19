using Microsoft.AspNet.Identity;
using SX.WebCore.MvcApplication;
using SX.WebCore.Repositories;
using SX.WebCore.Resources;
using System;
using System.Data.Entity;

namespace GE.WebAdmin
{
    public class MvcApplication : SxApplication<WebCoreExtantions.DbContext>
    {
        private static DateTime _lastStartDate;

        protected override void Application_Start(object sender, EventArgs e)
        {
            var args = new SxApplicationEventArgs();
            args.WebApiConfigRegister = WebApiConfig.Register;
            args.RegisterRoutes = RouteConfig.RegisterRoutes;
            args.MapperConfiguration = AutoMapperConfig.MapperConfigurationInstance;
            args.LoggingRequest = false;
            base.Application_Start(sender, args);

            _lastStartDate = DateTime.Now;
            Database.SetInitializer<WebCoreExtantions.DbContext>(null);
            var siteDomainItem = new SxRepoSiteSetting<WebCoreExtantions.DbContext>().GetByKey(Settings.siteDomain);
            SiteDomain = siteDomainItem?.Value;
        }

        public static DateTime LastStartDate
        {
            get
            {
                return _lastStartDate;
            }
        }

        protected override void Session_Start()
        {
            var sessionId = Session.SessionID;
            if (!UsersOnSite.ContainsKey(sessionId))
                UsersOnSite.Add(sessionId, null);

            if (User.Identity.IsAuthenticated)
                UsersOnSite[sessionId] = User.Identity.GetUserName();
        }

        protected override void Session_End()
        {
            var sessionId = Session.SessionID;
            if (UsersOnSite.ContainsKey(sessionId))
                UsersOnSite.Remove(sessionId);
        }
    }
}
