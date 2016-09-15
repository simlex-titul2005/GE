using SX.WebCore.MvcControllers;
using SX.WebCore.ViewModels;
using System.Collections.Generic;

namespace GE.WebUI.Infrastructure
{
    public static class BreadcrumbsManager
    {
        public static void WriteBreadcrumbs(SxBaseController<DbContext> controller)
        {
            if (controller.ControllerContext.IsChildAction) return;

            var routes = controller.ControllerContext.RequestContext.RouteData.Values;
            var gameName = routes["game"] != null && !string.IsNullOrEmpty(routes["game"].ToString()) ? routes["game"].ToString() : null;

            var breadcrumbs = new List<SxVMBreadcrumb>();
            breadcrumbs.Add(new SxVMBreadcrumb { Title = "Главная", Url = "/" });

            switch (controller.SxControllerName)
            {
                case "aphorisms":
                    if (controller.SxActionName == "list" || controller.SxActionName == "details")
                    {
                        breadcrumbs.Add(new SxVMBreadcrumb { Title = "Афоризмы", Url = controller.Url.Action("List", "Aphorisms") });
                    }
                    break;
                case "articles":
                    if (controller.SxActionName == "list")
                    {
                        breadcrumbs.Add(new SxVMBreadcrumb { Title = "Статьи", Url = controller.Url.Action("List", "Articles") });
                        if (gameName != null)
                            breadcrumbs.Add(new SxVMBreadcrumb { Title = gameName });
                    }
                    else if (controller.SxActionName == "details")
                    {
                        breadcrumbs.Add(new SxVMBreadcrumb { Title = "Статьи", Url = controller.Url.Action("List", "Articles") });
                    }
                    break;
                case "authoraphorisms":
                    if (controller.SxActionName == "details")
                    {
                        breadcrumbs.Add(new SxVMBreadcrumb { Title = "Афоризмы", Url = controller.Url.Action("List", "Aphorisms") });
                    }
                    break;
                case "humor":
                    breadcrumbs.Add(new SxVMBreadcrumb { Title = "Юмор", Url = controller.Url.Action("List", "Humor") });
                    break;
                case "news":
                    if (controller.SxActionName == "list")
                    {
                        breadcrumbs.Add(new SxVMBreadcrumb { Title = "Новости", Url = controller.Url.Action("List", "News") });
                        if (gameName != null)
                            breadcrumbs.Add(new SxVMBreadcrumb { Title = gameName });
                    }
                    else if (controller.SxActionName == "details")
                    {
                        breadcrumbs.Add(new SxVMBreadcrumb { Title = "Новости", Url = controller.Url.Action("List", "News") });
                    }
                    break;
                case "sitetests":
                    if (controller.SxActionName == "list" || controller.SxActionName == "details")
                        breadcrumbs.Add(new SxVMBreadcrumb { Title = "Тесты", Url = controller.Url.Action("List", "SiteTests") });
                    break;
            }

            controller.ViewBag.Breadcrumbs = breadcrumbs.ToArray();
        }
    }
}