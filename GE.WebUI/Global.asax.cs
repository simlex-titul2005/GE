using GE.WebUI.Infrastructure;
using GE.WebUI.Models;
using GE.WebUI.ViewModels;
using SX.WebCore.MvcApplication;
using System;
using System.Web.Mvc;

namespace GE.WebUI
{
    public class MvcApplication : SxMvcApplication<DbContext>
    {
        private static readonly string[] _defaultControllerNamespaces = new string[] { "GE.WebUI.Controllers" };

        protected override void Application_Start(object sender, EventArgs e)
        {
            var args = new SxApplicationEventArgs
            {
                WebApiConfigRegister = WebApiConfig.Register,
                MapperConfigurationExpression = cfg =>
                {

                    //aphorism
                    cfg.CreateMap<Aphorism, VMAphorism>();
                    cfg.CreateMap<VMAphorism, Aphorism>();

                    //author aphorism
                    cfg.CreateMap<AuthorAphorism, VMAuthorAphorism>();
                    cfg.CreateMap<VMAuthorAphorism, AuthorAphorism>();

                    //article
                    //cfg.CreateMap<Article, VMArticle>();
                    //cfg.CreateMap<VMArticle, Article>();

                },

                //routes
                DefaultControllerNamespaces = _defaultControllerNamespaces,
                PostRouteAction = routes =>
                {
                    //humor
                    routes.MapRoute(
                        name: null,
                        url: "humor",
                        defaults: new { controller = "Humor", action = "List", page = 1, area = "" },
                        namespaces: _defaultControllerNamespaces
                    );

                    routes.MapRoute(
                        name: null,
                        url: "humor/page{page}",
                        defaults: new { controller = "Humor", action = "List", page = 1, area = "" },
                        namespaces: _defaultControllerNamespaces
                    );

                }
            };

            base.Application_Start(sender, args);
        }
    }
}
