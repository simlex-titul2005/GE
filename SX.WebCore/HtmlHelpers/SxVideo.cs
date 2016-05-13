using SX.WebCore.Providers;
using System.Web.Mvc;

namespace SX.WebCore.HtmlHelpers
{
    public static partial class SxExtantions
    {
        public static MvcHtmlString SxVideo(this HtmlHelper htmlHelper, SxVideo video)
        {
            return MvcHtmlString.Create(GetVideoTemplate(video));
        }

        public static string GetVideoTemplate(SxVideo video)
        {
            var figure = new TagBuilder("figure");
            figure.AddCssClass("video");
            figure.MergeAttribute("style", string.Concat("background-image:url(", VideoProvider.GetVideoImageUrl(video.VideoId), ");"));

            var playWrapper = new TagBuilder("div");
            playWrapper.AddCssClass("v-p-wr");

            var a = new TagBuilder("a");
            a.MergeAttribute("href", "javascript:void(0)");
            a.InnerHtml += "<i class=\"fa fa-youtube-play\"></i>";
            playWrapper.InnerHtml += a;

            return figure.ToString();
        }
    }
}
