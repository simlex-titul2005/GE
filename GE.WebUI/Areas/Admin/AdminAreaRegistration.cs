using System.Web.Mvc;

namespace GE.WebUI.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                name: null,
                url: "Admin",
                defaults: new { controller = "Home", action = "Index" }
            );

            context.MapRoute(
                name: null,
                url: "AuthorAphorisms/FindGridView",
                defaults: new { controller = "AuthorAphorisms", action = "FindGridView" }
            );

            context.MapRoute(
                name: null,
                url: "SiteTests/TestMatrix/{testId}/{page}",
                defaults: new { controller = "SiteTests", action = "TestMatrix" }
            );

            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}