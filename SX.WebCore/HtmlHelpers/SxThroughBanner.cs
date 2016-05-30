using System;
using System.Web.Mvc;

namespace SX.WebCore.HtmlHelpers
{
    public static partial class SxExtantions
    {
        public static MvcHtmlString SxThroughBanner(this HtmlHelper htmlHelper, SxBanner banner, Func<SxBanner, string> FuncBannerImgUrl)
        {
            if (banner == null) return null;
            var figure = new TagBuilder("figure");
            figure.AddCssClass(string.Concat("th-banner ", banner.Place.ToString().ToLower()));

            var a = new TagBuilder("a");
            a.MergeAttribute("href", banner.Url);
            a.MergeAttribute("target", "_blank");

            var img = new TagBuilder("img");
            img.MergeAttribute("src", FuncBannerImgUrl(banner));
            img.MergeAttribute("alt", banner.Title);
            a.InnerHtml += img;

            figure.InnerHtml += a;

            return MvcHtmlString.Create(figure.ToString());
        }
    }
}
