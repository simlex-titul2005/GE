using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using GE.WebCoreExtantions;
using SX.WebCore.Managers;
using SX.WebCore;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.MicrosoftAccount;
using System.Configuration;

[assembly: OwinStartup(typeof(AspNetIdentityApp.Startup))]
namespace AspNetIdentityApp
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // настраиваем контекст и менеджер
            app.CreatePerOwinContext<DbContext>(DbContext.Create<DbContext>);
            app.CreatePerOwinContext<SxAppUserManager>(SxAppUserManager.Create<DbContext>);
            app.CreatePerOwinContext<SxAppSignInManager>(SxAppSignInManager.Create);

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<SxAppUserManager, SxAppUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);


            //app.UseMicrosoftAccountAuthentication(
            //    clientId: ConfigurationManager.AppSettings["MSOAuthClientId"],
            //    clientSecret: ConfigurationManager.AppSettings["MSOAuthClientSecret"]);

            //app.UseTwitterAuthentication(
            //   consumerKey: ConfigurationManager.AppSettings["TwitterOAuthClientId"],
            //   consumerSecret: ConfigurationManager.AppSettings["TwitterOAuthClientSecret"]);

            //app.UseFacebookAuthentication(
            //   appId: ConfigurationManager.AppSettings["FacebookOAuthClientId"],
            //   appSecret: ConfigurationManager.AppSettings["FacebookOAuthClientSecret"]);

            app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = ConfigurationManager.AppSettings["GoogleOAuthClientId"],
                ClientSecret = ConfigurationManager.AppSettings["GoogleOAuthClientSecret"]
            });
        }
    }
}