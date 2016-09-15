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
