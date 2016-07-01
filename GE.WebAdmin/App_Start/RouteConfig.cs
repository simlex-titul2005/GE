using System.Web.Mvc;
using System.Web.Routing;

namespace GE.WebAdmin
{
    public class RouteConfig
    {
        private static readonly string[] _defNamespace = new string[] { "GE.WebAdmin.Controllers" };

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.LowercaseUrls = true;

            routes.MapRoute(
                name: null,
                url: "sitetests/testmatrix/{testId}/{page}",
                defaults: new { controller = "sitetests", action = "testmatrix", area = "" },
                namespaces: _defNamespace
            );

            routes.MapRoute(
                name: null,
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "home", action = "index", id = UrlParameter.Optional, page=1, area="" },
                namespaces: _defNamespace
            );
        }
    }
}
