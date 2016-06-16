using GE.WebCoreExtantions;
using GE.WebUI.Models;
using SX.WebCore.MvcControllers;
using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using System.Web.Mvc;

namespace GE.WebUI.Extantions.Controllers
{
    public static class BaseControllerExt
    {
        private static CacheItemPolicy _defaultPolicy
        {
            get
            {
                return new CacheItemPolicy
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(15)
                };
            }
        }

        public static void WriteBreadcrumbs(this SxBaseController<DbContext> controller)
        {
            var routes = controller.ControllerContext.RequestContext.RouteData.Values;
            var gameName = routes["game"] != null && !string.IsNullOrEmpty(routes["game"].ToString()) ? routes["game"].ToString() : null;

            var breadcrumbs = new List<VMBreadcrumb>();
            breadcrumbs.Add(new VMBreadcrumb { Title = "Главная", Url = "/" });
            if(controller.SxControllerName=="aphorisms")
            {
                if (controller.SxActionName == "list" || controller.SxActionName == "details")
                {
                    breadcrumbs.Add(new VMBreadcrumb { Title = "Афоризмы", Url = controller.Url.Action(MVC.Aphorisms.List()) });
                }
            }
            else if (controller.SxControllerName == "articles")
            {
                if (controller.SxActionName == "list")
                {
                    breadcrumbs.Add(new VMBreadcrumb { Title = "Статьи", Url = controller.Url.Action(MVC.Articles.List()) });
                    if (gameName != null)
                        breadcrumbs.Add(new VMBreadcrumb { Title = gameName });
                }
                else if (controller.SxActionName == "details")
                {
                    breadcrumbs.Add(new VMBreadcrumb { Title = "Статьи", Url = controller.Url.Action(MVC.Articles.List()) });
                }
            }
            else if (controller.SxControllerName == "news")
            {
                if (controller.SxActionName == "list")
                {
                    breadcrumbs.Add(new VMBreadcrumb { Title = "Новости", Url = controller.Url.Action(MVC.News.List()) });
                    if (gameName != null)
                        breadcrumbs.Add(new VMBreadcrumb { Title = gameName });
                }
                else if (controller.SxActionName == "details")
                {
                    breadcrumbs.Add(new VMBreadcrumb { Title = "Новости", Url = controller.Url.Action(MVC.News.List()) });
                }
            }
            else if (controller.SxControllerName == "forum")
            {
                if (controller.SxActionName == "list")
                {
                    breadcrumbs.Add(new VMBreadcrumb { Title = "Форум", Url = controller.Url.Action(MVC.Forum.List()) });
                    if (gameName != null)
                        breadcrumbs.Add(new VMBreadcrumb { Title = gameName });
                }
            }

            controller.ViewBag.Breadcrumbs = breadcrumbs.ToArray();
        }
    }
}