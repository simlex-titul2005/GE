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
            figure.MergeAttribute("style", string.Concat("background-image:url(", FuncBannerImgUrl(banner), ")"));

            return MvcHtmlString.Create(figure.ToString());
        }
    }
}
