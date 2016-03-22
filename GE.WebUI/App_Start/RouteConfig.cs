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

            #region forum
            routes.MapRoute(
                name: null,
                url: "forum",
                defaults: new { controller = "forum", action = "list", gameTitle = (string)null, page = 1, area = "" }
            );

            routes.MapRoute(
                name: null,
                url: "forum/page{page}",
                defaults: new { controller = "forum", action = "list", gameTitle = (string)null, page = 1, area = "" }
            );
            routes.MapRoute(
                name: null,
                url: "forum/{gameTitle}",
                defaults: new { controller = "forum", action = "list", page = 1, area = "" }
            );
            routes.MapRoute(
                name: null,
                url: "forum/{gameTitle}/page{page}",
                defaults: new { controller = "forum", action = "list", page = 1, area = "" }
            );
            #endregion

            #region articles
            routes.MapRoute(
                name: null,
                url: "articles/preview",
                defaults: new { controller = "articles", action = "preview", area = "" }
            );
            routes.MapRoute(
                name: null,
                url: "articles",
                defaults: new { controller = "articles", action = "list", gameTitle = (string)null, page = 1, area = "" }
            );
            routes.MapRoute(
                name: null,
                url: "articles/page{page}",
                defaults: new { controller = "articles", action = "list", gameTitle = (string)null, page = 1, area = "" }
            );
            routes.MapRoute(
                name: null,
                url: "articles/{gameTitle}",
                defaults: new { controller = "articles", action = "list", page = 1, area = "" }
            );
            routes.MapRoute(
                name: null,
                url: "articles/{gameTitle}/page{page}",
                defaults: new { controller = "articles", action = "list", page = 1, area = "" }
            );
            routes.MapRoute(
                name: null,
                url: "articles/{year}/{month}/{day}/{titleUrl}",
                defaults: new { controller = "articles", action = "details", area = "" },
                constraints: new { year = @"\d{4}" }
            );
            #endregion

            #region news
            routes.MapRoute(
                name: null,
                url: "news",
                defaults: new { controller = "news", action = "list", gameTitle = (string)null, page = 1, area = "" }
            );
            routes.MapRoute(
                name: null,
                url: "news/page{page}",
                defaults: new { controller = "news", action = "list", gameTitle = (string)null, page = 1, area = "" }
            );
            routes.MapRoute(
                name: null,
                url: "news/{gameTitle}",
                defaults: new { controller = "news", action = "list", page = 1, area = "" }
            );
            routes.MapRoute(
                name: null,
                url: "news/{gameTitle}/page{page}",
                defaults: new { controller = "news", action = "list", page = 1, area = "" }
            );
            routes.MapRoute(
                name: null,
                url: "news/{year}/{month}/{day}/{titleUrl}",
                defaults: new { controller = "news", action = "details", area = "" },
                constraints: new { year = @"\d{4}" }
            );
            #endregion

            routes.MapRoute(
                name: null,
                url: "home/{gameTitle}",
                defaults: new { controller = "home", action = "index", gameTitle = UrlParameter.Optional, area = "" }
            );

            routes.MapRoute(
                name: null,
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "home", action = "index", id = UrlParameter.Optional, area = "" }
            );
        }
    }
}
