using System.Web.Mvc;
using System.Web.Routing;

namespace GE.WebUI
{
    public class RouteConfig
    {
        private static readonly string[] _defNamespace = { "GE.WebUI.Controllers" };

        public static void PreRouteAction(RouteCollection routes)
        {
            routes.IgnoreRoute("Articles/Details/{id}");
            routes.IgnoreRoute("News/Details/{id}");

            #region aphorisms

            routes.MapRoute(
                name: null,
                url: "AuthorAphorisms/{titleUrl}",
                defaults: new { controller = "AuthorAphorisms", action = "Details", area = "" },
                namespaces: _defNamespace
            );

            routes.MapRoute(
                name: null,
                url: "Aphorisms",
                defaults: new { controller = "Aphorisms", action = "List", categoryId = (string)null, page = 1, area = "" },
                namespaces: _defNamespace,
                constraints: new { action = "^List" }
            );

            routes.MapRoute(
                name: null,
                url: "Aphorisms/Page{page}",
                defaults: new { controller = "Aphorisms", action = "List", categoryId = (string)null, page = 1, area = "" },
                namespaces: _defNamespace,
                constraints: new { action = "^List" }
            );

            routes.MapRoute(
                name: null,
                url: "Aphorisms/{categoryId}",
                defaults: new { controller = "Aphorisms", action = "List", page = 1, area = "" },
                namespaces: _defNamespace,
                constraints: new { action = "^List" }
            );

            routes.MapRoute(
                name: null,
                url: "Aphorisms/{categoryId}/page{page}",
                defaults: new { controller = "Aphorisms", action = "List", page = 1, area = "" },
                namespaces: _defNamespace,
                constraints: new { action = "^List" }
            );

            routes.MapRoute(
                name: null,
                url: "Aphorisms/{categoryId}/{titleUrl}",
                defaults: new { controller = "Aphorisms", action = "Details", area = "" },
                namespaces: _defNamespace,
                constraints: new { action = "^Details" }
            );
            #endregion

            #region infographics
            routes.MapRoute(name: null, url: "Infographics/{id}", defaults: new { controller = "Infographics", action = "Details" }, namespaces: _defNamespace);
            #endregion

            #region games
            routes.MapRoute(
                name: null,
                url: "Games/{titleUrl}",
                defaults: new { controller = "Games", action = "Details", area = "" },
                namespaces: _defNamespace
            );
            #endregion

            #region rss
            routes.MapRoute(name: null, url: "Rss", defaults: new { controller = "Rss", action = "List" }, namespaces: _defNamespace);
            #endregion

            #region site test
            routes.MapRoute(
                name: null,
                url: "sitetests",
                defaults: new { controller = "SiteTests", action = "List", page = 1, area = "" },
                namespaces: _defNamespace,
                constraints: new { httpMethod = new HttpMethodConstraint("get") }
            );

            routes.MapRoute(
                name: null,
                url: "sitetests/page{page}",
                defaults: new { controller = "SiteTests", action = "List", page = 1, area = "" },
                namespaces: _defNamespace,
                constraints: new { httpMethod = new HttpMethodConstraint("get") }
            );

            routes.MapRoute(
                name: null,
                url: "sitetests/{titleUrl}",
                defaults: new { controller = "SiteTests", action = "Details", area = "" },
                namespaces: _defNamespace,
                constraints: new { httpMethod = new HttpMethodConstraint("get") }
            );
            #endregion

            #region youtube videos
            routes.MapRoute(name: null, url: "YoutubeVideos", defaults: new { controller = "YoutubeVideos", action = "List", amount = 9, cat=20, area = "" }, namespaces: _defNamespace);
            routes.MapRoute(name: null, url: "YoutubeVideos/Archive", defaults: new { controller = "YoutubeVideos", action = "Archive", area = "" }, namespaces: _defNamespace);
            routes.MapRoute(name: null, url: "YoutubeVideos/GetJson", defaults: new { controller = "YoutubeVideos", action = "GetJson", area = "" }, namespaces: _defNamespace);
            routes.MapRoute(name: null, url: "YoutubeVideos/{videoId}", defaults: new { controller = "YoutubeVideos", action = "Details", area = "" }, namespaces: _defNamespace);
            #endregion
        }

        public static void PostRouteAction(RouteCollection routes)
        {
            routes.MapRoute(
                        name: null,
                        url: "{controller}/AddLike/{mid}/{ld}",
                        defaults: new { controller = "Articles", action = "AddLike", area = "" },
                        namespaces: _defNamespace,
                        constraints: new { httpMethod = new HttpMethodConstraint("post") }
                    );



            #region articles
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
                defaults: new { controller = "articles", action = "List", gameTitle = (string)null, page = 1, area = "" },
                namespaces: _defNamespace
            );
            routes.MapRoute(
                name: null,
                url: "articles/page{page}",
                defaults: new { controller = "articles", action = "List", gameTitle = (string)null, page = 1, area = "" },
                namespaces: _defNamespace
            );
            routes.MapRoute(
                name: null,
                url: "articles/{gameTitle}",
                defaults: new { controller = "articles", action = "List", page = 1, area = "" },
                namespaces: _defNamespace
            );
            routes.MapRoute(
                name: null,
                url: "articles/{gameTitle}/page{page}",
                defaults: new { controller = "articles", action = "List", page = 1, area = "" },
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

            #region banners
            routes.MapRoute(
                name: null,
                url: "Banners/Click/{bannerId}",
                defaults: new { controller = "Banners", action = "Click", area = "" },
                namespaces: _defNamespace
            );
            #endregion

            #region employees
            routes.MapRoute(
                name: null,
                url: "employees",
                defaults: new { controller = "employees", action = "List", area = "" },
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
                defaults: new { controller = "news", action = "List", gameTitle = (string)null, page = 1, area = "" },
                namespaces: _defNamespace
            );
            routes.MapRoute(
                name: null,
                url: "news/page{page}",
                defaults: new { controller = "news", action = "List", gameTitle = (string)null, page = 1, area = "" },
                namespaces: _defNamespace
            );
            routes.MapRoute(
                name: null,
                url: "news/{gameTitle}",
                defaults: new { controller = "news", action = "List", page = 1, area = "" },
                namespaces: _defNamespace
            );
            routes.MapRoute(
                name: null,
                url: "news/{gameTitle}/page{page}",
                defaults: new { controller = "news", action = "List", page = 1, area = "" },
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

            #region humor
            routes.MapRoute(
                name: null,
                url: "humor",
                defaults: new { controller = "Humor", action = "List", page = 1, area = "" },
                namespaces: _defNamespace
            );
            routes.MapRoute(
                name: null,
                url: "humor/page{page}",
                defaults: new { controller = "Humor", action = "List", page = 1, area = "" },
                namespaces: _defNamespace
            );

            routes.MapRoute(
                name: null,
                url: "humor/{year}/{month}/{day}/{titleUrl}",
                defaults: new { controller = "Humor", action = "Details", area = "" },
                constraints: new { year = @"\d{4}" },
                namespaces: _defNamespace
            );
            #endregion

            routes.MapRoute(
                name: null,
                url: "Home/{gameTitle}",
                defaults: new { controller = "Home", action = "Index", gameTitle = UrlParameter.Optional, area = "" },
                namespaces: _defNamespace
            );
        }
    }
}
