using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace GE.WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.LowercaseUrls = true;

            #region articles
            routes.MapRoute(
                name: null,
                url: "articles",
                defaults: new { controller = "articles", action = "list", game = "", area = "" }
            );

            routes.MapRoute(
                name: null,
                url: "articles/preview",
                defaults: new { controller = "articles", action = "preview", area = "" }
            );

            routes.MapRoute(
                name: null,
                url: "articles/{game}",
                defaults: new { controller = "articles", action = "list", area = "" }
            );
            #endregion

            routes.MapRoute(
                name: null,
                url: "home/{game}",
                defaults: new { controller = "home", action = "index", area = "" }
            );

            routes.MapRoute(
                name: null,
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "home", action = "index", id = UrlParameter.Optional, area = "" }
            );
        }
    }
}
