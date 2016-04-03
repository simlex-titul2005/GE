using System.Web.Mvc;
using System.Web.Routing;

namespace GE.WebAdmin
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.LowercaseUrls = true;

            routes.MapRoute(
                name: null,
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "home", action = "index", id = UrlParameter.Optional, page=1, area="" }
            );
        }
    }
}
