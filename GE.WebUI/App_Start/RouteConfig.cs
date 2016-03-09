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
            
            routes.MapRoute(
                name: null,
                url: "robots.txt",
                defaults: new { controller = "home", action = "robotstxt", area = "" }
            );

            #region articles
            routes.MapRoute(
                name: null,
                url: "articles/details/{titleUrl}",
                defaults: new { controller = "articles", action = "details", area = "" }
            );

            routes.MapRoute(
                name: null,
                url: "articles",
                defaults: new { controller = "articles", action = "list", game = "", page=1, area = "" }
            );

            routes.MapRoute(
               name: null,
               url: "articles/page{page}",
               defaults: new { controller = "articles", action = "list", game = "", page = 1, area = "" }
           );

            routes.MapRoute(
                name: null,
                url: "articles/preview",
                defaults: new { controller = "articles", action = "preview", area = "" }
            );
            
            routes.MapRoute(
                name: null,
                url: "articles/{game}",
                defaults: new { controller = "articles", action = "list", page=1, area = "" }
            );

            routes.MapRoute(
                name: null,
                url: "articles/{game}/page{page}",
                defaults: new { controller = "articles", action = "list", page=1, area = "" }
            );
            #endregion

            #region news
            routes.MapRoute(
                name: null,
                url: "news/details/{titleUrl}",
                defaults: new { controller = "news", action = "details", area = "" }
            );

            routes.MapRoute(
                name: null,
                url: "news",
                defaults: new { controller = "news", action = "list", game = "", page = 1, area = "" }
            );

            routes.MapRoute(
               name: null,
               url: "news/page{page}",
               defaults: new { controller = "news", action = "list", game = "", page = 1, area = "" }
           );

            routes.MapRoute(
                name: null,
                url: "news/{game}",
                defaults: new { controller = "news", action = "list", page = 1, area = "" }
            );

            routes.MapRoute(
                name: null,
                url: "news/{game}/page{page}",
                defaults: new { controller = "news", action = "list", page = 1, area = "" }
            );
            #endregion

            routes.MapRoute(
                name: null,
                url: "home/{game}",
                defaults: new { controller = "home", action = "index", game="", area = "" }
            );

            routes.MapRoute(
                name: null,
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "home", action = "index", id = UrlParameter.Optional, area = "" }
            );
        }
    }
}
