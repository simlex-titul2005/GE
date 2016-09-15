using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using SX.WebCore.Managers;
using SX.WebCore;
using GE.WebUI.Infrastructure;

[assembly: OwinStartup(typeof(GE.WebUI.Startup))]
namespace GE.WebUI
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext<DbContext>(DbContext.Create<DbContext>);
            app.CreatePerOwinContext<SxAppUserManager>(SxAppUserManager.Create<DbContext>);
            app.CreatePerOwinContext<SxAppSignInManager>(SxAppSignInManager.Create);
            app.CreatePerOwinContext<SxAppRoleManager>(SxAppRoleManager.Create<DbContext>);

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/account/login"),
                Provider = new CookieAuthenticationProvider
                {
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<SxAppUserManager, SxAppUser>(
                            validateInterval: TimeSpan.FromMinutes(30),
                            regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            //app.MapSignalR();
        }
    }
}