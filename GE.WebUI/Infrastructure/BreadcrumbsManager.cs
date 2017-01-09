using GE.WebUI.ViewModels;
using SX.WebCore.MvcControllers.Abstract;
using SX.WebCore.ViewModels;
using System.Collections.Generic;

namespace GE.WebUI.Infrastructure
{
    public static class BreadcrumbsManager
    {
        public static void WriteBreadcrumbs(SxBaseController controller, HashSet<SxVMBreadcrumb> breadcrumbs)
        {
            if (controller.ControllerContext.IsChildAction) return;

            var routes = controller.ControllerContext.RequestContext.RouteData.Values;
            var gameName = !string.IsNullOrEmpty(routes["game"]?.ToString()) ? routes["game"].ToString() : null;

            breadcrumbs.Add(new SxVMBreadcrumb { Title = "Главная", Url = "/" });

            switch (controller.SxControllerName)
            {
                case "aphorisms":
                    breadcrumbs.Add(new SxVMBreadcrumb { Title = "Афоризмы", Url = controller.Url.Action("List", "Aphorisms") });

                    var category = (SxVMMaterialCategory)controller.ViewBag.Category;
                    if (category != null)
                        breadcrumbs.Add(new SxVMBreadcrumb { Title = category.Title, Url = controller.Url.Action("List", "Aphorisms", new { categoryId = category.Id }) });

                    var author = (ViewModels.VMAuthorAphorism)controller.ViewBag.Author;
                    if (author != null)
                        breadcrumbs.Add(new SxVMBreadcrumb { Title = author.Name, Url = controller.Url.Action("List", "Aphorisms", new { author = author.TitleUrl }) });

                    if (controller.SxActionName == "details")
                    {
                        var model = ((ViewModels.VMDetailAphorism)controller.SxModel)?.Aphorism;
                        if (model != null)
                            breadcrumbs.Add(new SxVMBreadcrumb { Title = model.Title, Url = model.GetUrl(controller.Url) });
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
                case "infographics":
                    breadcrumbs.Add(new SxVMBreadcrumb { Title = "Инфографики", Url = "#" });
                    if (controller.SxActionName == "details")
                    {
                        var model = (VMInfographic)controller.SxModel;
                        breadcrumbs.Add(new SxVMBreadcrumb { Title = model.Caption, Url = controller.Url.Action("Details", "Infographics", new { id = model.PictureId }) });
                    }
                    break;
                case "humor":
                    breadcrumbs.Add(new SxVMBreadcrumb { Title = "Юмор", Url = controller.Url.Action("List", "Humor") });
                    if (Equals("details", controller.SxActionName))
                    {
                        var model = (ViewModels.VMHumor)controller.SxModel;
                        breadcrumbs.Add(new SxVMBreadcrumb { Title = model.Title, Url = model.GetUrl(controller.Url) });
                    }
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
                    breadcrumbs.Add(new SxVMBreadcrumb { Title = "Тесты", Url = controller.Url.Action("List", "SiteTests") });
                    if (controller.SxActionName == "details")
                    {
                        var model = (ViewModels.VMSiteTestAnswer)controller.SxModel;
                        var test = model?.Question?.Test;
                        if (test != null)
                            breadcrumbs.Add(new SxVMBreadcrumb { Title = test.Title, Url = controller.Url.Action("Details", "SiteTests", new { titleUrl = test.TitleUrl }) });
                    }
                    break;
                case "youtubevideos":
                    if (controller.SxActionName != "details")
                        breadcrumbs.Add(new SxVMBreadcrumb { Title = "Поулярное видео", Url = controller.Url.Action("List", "YoutubeVideos", new { amount = 9, cat = 20 }) });
                    else
                    {
                        breadcrumbs.Add(new SxVMBreadcrumb { Title = "Видео", Url = "javascript:void(0)" });
                        var model = (SX.WebCore.ViewModels.Youtube.SxVMYoutubeVideo)controller.SxModel;
                        breadcrumbs.Add(new SxVMBreadcrumb { Title = model.Snippet?.Title ?? model.Id, Url = controller.Url.Action(controller.SxActionName, controller.SxActionName, new { videoId = model.Id }) });
                    }
                    break;
            }
        }
    }
}