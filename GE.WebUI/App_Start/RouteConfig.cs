using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace GE.WebUI
{
    public class RouteConfig
    {
        private static readonly string[] _defNamespace = new string[] { "GE.WebUI.Controllers" };

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.LowercaseUrls = true;

            routes.MapRoute(
                name: null,
                url: "robots.txt",
                defaults: new { controller = "seo", action = "robotstxt", area = "" },
                namespaces: _defNamespace
            );

            routes.MapRoute(
                name: null,
                url: "sitemap.xml",
                defaults: new { controller = "seo", action = "sitemap", area = "" },
                namespaces: _defNamespace
            );

            #region aphorisms

            routes.MapRoute(
                name: null,
                url: "aphorisms",
                defaults: new { controller = "aphorisms", action = "list", categoryId = (string)null, page = 1, area = "" },
                namespaces: _defNamespace,
                constraints: new { action = "^List" }
            );

            routes.MapRoute(
                name: null,
                url: "aphorisms/page{page}",
                defaults: new { controller = "aphorisms", action = "list", categoryId=(string)null, page = 1, area = "" },
                namespaces: _defNamespace,
                constraints: new { action = "^List" }
            );

            routes.MapRoute(
                name: null,
                url: "aphorisms/{categoryId}",
                defaults: new { controller = "aphorisms", action = "list", page=1, area = "" },
                namespaces: _defNamespace,
                constraints: new { action = "^List" }
            );

            routes.MapRoute(
                name: null,
                url: "aphorisms/{categoryId}/page{page}",
                defaults: new { controller = "aphorisms", action = "list", page = 1, area = "" },
                namespaces: _defNamespace,
                constraints: new { action = "^List" }
            );

            routes.MapRoute(
                name: null,
                url: "aphorisms/{categoryId}/{titleUrl}",
                defaults: new { controller = "aphorisms", action = "details", area = "" },
                namespaces: _defNamespace,
                constraints: new { action= "^Details" }
            );



            #endregion

            #region articles
            routes.IgnoreRoute("articles/details/{id}");

            routes.MapRoute(
                name: null,
                url: "articles/bydatematerial",
                defaults: new { controller = "articles", action = "bydatematerial", area = "" },
                namespaces: _defNamespace
            );

            routes.MapRoute(
                name: null,
                url: "articles/preview",
                defaults: new { controller = "articles", action = "preview", area = "" },
                namespaces: _defNamespace
            );
            routes.MapRoute(
                name: null,
                url: "articles",
                defaults: new { controller = "articles", action = "list", gameTitle = (string)null, page = 1, area = "" },
                namespaces: _defNamespace
            );
            routes.MapRoute(
                name: null,
                url: "articles/page{page}",
                defaults: new { controller = "articles", action = "list", gameTitle = (string)null, page = 1, area = "" },
                namespaces: _defNamespace
            );
            routes.MapRoute(
                name: null,
                url: "articles/{gameTitle}",
                defaults: new { controller = "articles", action = "list", page = 1, area = "" },
                namespaces: _defNamespace
            );
            routes.MapRoute(
                name: null,
                url: "articles/{gameTitle}/page{page}",
                defaults: new { controller = "articles", action = "list", page = 1, area = "" },
                namespaces: _defNamespace
            );
            routes.MapRoute(
                name: null,
                url: "articles/{year}/{month}/{day}/{titleUrl}",
                defaults: new { controller = "articles", action = "details", area = "" },
                constraints: new { year = @"\d{4}" },
                namespaces: _defNamespace
            );
            #endregion

            #region employees
            routes.MapRoute(
                name: null,
                url: "employees",
                defaults: new { controller = "employees", action = "list", area = "" },
                namespaces: _defNamespace
            );
            #endregion

            #region forum
            routes.MapRoute(
                name: null,
                url: "forum",
                defaults: new { controller = "forum", action = "list", gameTitle = (string)null, page = 1, area = "" },
                namespaces: _defNamespace
            );

            routes.MapRoute(
                name: null,
                url: "forum/page{page}",
                defaults: new { controller = "forum", action = "list", gameTitle = (string)null, page = 1, area = "" },
                namespaces: _defNamespace
            );
            routes.MapRoute(
                name: null,
                url: "forum/{gameTitle}",
                defaults: new { controller = "forum", action = "list", page = 1, area = "" },
                namespaces: _defNamespace
            );
            routes.MapRoute(
                name: null,
                url: "forum/{gameTitle}/page{page}",
                defaults: new { controller = "forum", action = "list", page = 1, area = "" },
                namespaces: _defNamespace
            );
            #endregion

            #region games
            routes.MapRoute(
                name: null,
                url: "games/{titleUrl}",
                defaults: new { controller = "games", action = "details", area = "" },
                namespaces: _defNamespace
            );
            #endregion

            #region news
            routes.IgnoreRoute("news/details/{id}");

            routes.MapRoute(
                name: null,
                url: "news/bydatematerial",
                defaults: new { controller = "news", action = "bydatematerial", area = "" },
                namespaces: _defNamespace
            );
            routes.MapRoute(
                name: null,
                url: "news",
                defaults: new { controller = "news", action = "list", gameTitle = (string)null, page = 1, area = "" },
                namespaces: _defNamespace
            );
            routes.MapRoute(
                name: null,
                url: "news/page{page}",
                defaults: new { controller = "news", action = "list", gameTitle = (string)null, page = 1, area = "" },
                namespaces: _defNamespace
            );
            routes.MapRoute(
                name: null,
                url: "news/{gameTitle}",
                defaults: new { controller = "news", action = "list", page = 1, area = "" },
                namespaces: _defNamespace
            );
            routes.MapRoute(
                name: null,
                url: "news/{gameTitle}/page{page}",
                defaults: new { controller = "news", action = "list", page = 1, area = "" },
                namespaces: _defNamespace
            );
            routes.MapRoute(
                name: null,
                url: "news/{year}/{month}/{day}/{titleUrl}",
                defaults: new { controller = "news", action = "details", area = "" },
                constraints: new { year = @"\d{4}" },
                namespaces: _defNamespace
            );
            #endregion

            routes.MapRoute(
                name: null,
                url: "home/{gameTitle}",
                defaults: new { controller = "home", action = "index", gameTitle = UrlParameter.Optional, area = "" },
                namespaces: _defNamespace
            );

            routes.MapRoute(
                name: null,
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "home", action = "index", id = UrlParameter.Optional, area = "" },
                namespaces: _defNamespace
            );
        }

        public class AphorismConstraint : IRouteConstraint
        {
            private string titleUrl;
            public AphorismConstraint(string titleUrl)
            {
                this.titleUrl = titleUrl;
            }
            public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
            {
                return !titleUrl.Contains("page");
            }
        }
    }
}
