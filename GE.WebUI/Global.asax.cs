using GE.WebUI.Infrastructure;
using GE.WebUI.Models;
using SX.WebCore.MvcApplication;
using System;
using System.Collections.Generic;

namespace GE.WebUI
{
    public class MvcApplication : SxMvcApplication
    {
        private static readonly string[] _defNamespace = new string[] { "GE.WebUI.Controllers" };

        protected override void Application_Start(object sender, EventArgs e)
        {
            var args = new SxApplicationEventArgs(
                    defaultSiteName: "game-exe.com",
                    getDbContextInstance: () => { return new DbContext(); },
                    getModelCoreTypeName: getModelCoreTypeName,
                    customModelCoreTypes: new Dictionary<string, byte>
                    {
                        [nameof(Aphorism)] = 6,
                        [nameof(Humor)] = 7
                    },
                    repoProvider: repoProvider
                )
            {
                WebApiConfigRegister = WebApiConfig.Register,
                MapperConfigurationExpression = AutoMapperConfig.Register,

                //routes
                DefaultControllerNamespaces = _defNamespace,
                PreRouteAction=RouteConfig.PreRouteAction,
                PostRouteAction = RouteConfig.PostRouteAction
            };

            base.Application_Start(sender, args);
        }

        private static string getModelCoreTypeName(byte key)
        {
            switch(key)
            {
                default:
                    return null;
                case 1:
                    return "Статья";
                case 2:
                    return "Новость";
                case 6:
                    return "Афоризм";
                case 7:
                    return "Юмор";
            }
        }

        private readonly RepoProvider repoProvider = new RepoProvider();
    }
}
