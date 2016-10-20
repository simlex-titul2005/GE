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
            var args = new SxApplicationEventArgs
            {
                DefaultSiteName="game-exe.com",
                GetDbContextInstance = () => { return new DbContext(); },
                WebApiConfigRegister = WebApiConfig.Register,
                MapperConfigurationExpression = AutoMapperConfig.Register,

                //routes
                DefaultControllerNamespaces = _defNamespace,
                PreRouteAction=RouteConfig.PreRouteAction,
                PostRouteAction = RouteConfig.PostRouteAction,

                CustomModelCoreTypes=new Dictionary<string, byte> {
                    [nameof(Aphorism)]=6,
                    [nameof(Humor)]=7
                },
                ModelCoreTypeNameFunc= getModelCoreTypeName
            };

            base.Application_Start(sender, args);
        }

        private static string getModelCoreTypeName(byte key)
        {
            switch(key)
            {
                default:
                    return null;
            }
        }
    }
}
