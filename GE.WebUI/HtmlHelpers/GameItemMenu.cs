using GE.WebUI.ViewModels;
using System.Text;
using System.Web.Mvc;

namespace GE.WebUI.HtmlHelpers
{
    public static class HtmlHelpersExtantions
    {
        public static MvcHtmlString GameItemMenu(this HtmlHelper html, VMGame game)
        {
            var sb = new StringBuilder();

            sb.Append("<div class=\"btn-group\">");

            sb.Append("<button type=\"button\" class=\"btn btn-sm btn-default dropdown-toggle\" data-toggle=\"dropdown\" aria-haspopup=\"true\" aria-expanded=\"false\">");
            sb.Append(" Действия <span class=\"caret\"></span>");
            sb.Append("</button>");

            sb.Append("<ul class=\"dropdown-menu\">");

            sb.Append("<li class=\"dropdown-header\">Привязки Steam</li>");
            sb.Append("<li><li><a href=\"#\" data-toggle=\"modal\" data-target=\"#modal-steam-apps\">Привязать SteamApp</a></li>");
            if (game.SteamAppsCount > 0)
            {
                sb.Append($"<li><a href=\"#\" data-toggle=\"modal\" data-target=\"#modal-linked-steam-apps\">Управление SteamApp <button class=\"btn btn-xs btn-primary\">{(game.SteamAppsCount)}</button></a></li>");
                sb.Append("<li class=\"dropdown-header\">Новости Steam</li>");
                sb.Append("<li><li><a href=\"#\" data-toggle=\"modal\" data-target=\"#modal-steam-app-news\">Парсить новости Steam</a></li>");
                sb.Append("<li role=\"separator\" class=\"divider\"></li>");
            }

            sb.Append("</ul>");

            sb.Append("</div>");

            return MvcHtmlString.Create(sb.ToString());
        }
    }
}