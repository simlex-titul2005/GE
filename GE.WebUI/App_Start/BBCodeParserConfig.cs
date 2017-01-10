using GE.WebUI.ViewModels.Abstracts;
using SX.WebCore.ViewModels;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using SX.WebCore;

namespace GE.WebUI
{
    public static class BBCodeParserConfig
    {
        public static void ReplaceInfographics(UrlHelper urlHelper, SxVMMaterial model)
        {
            if (string.IsNullOrEmpty(model.Html)) return;

            var matches = Regex.Matches(model.Html, @"\[ig\]([^\]]+)\[/ig\]", RegexOptions.IgnoreCase);

            Guid pictureId = Guid.Empty;
            foreach (Match match in matches)
            {
                var strValue = match.Groups[1].Value;
                if (!Guid.TryParse(strValue, out pictureId)) continue;

                var picture = (model as VMMaterial).Infographics.FirstOrDefault(x => x.PictureId== pictureId);
                if (picture == null) continue;

                var url= urlHelper.ContentAbsUrl($"/infographics/{strValue.ToLower()}");
                var share = SX.WebCore.HtmlHelpers.SxExtantions.GetShareButtonTemplate(settings: new SX.WebCore.HtmlHelpers.SxExtantions.SxShareSettings {
                    ItemTemplate = b => $"<li data-type=\"{b.Net.Code}\" class=\"goodshare\" data-url=\"{url}\" data-title=\"{picture.Caption}\" data-text=\"{model.Title}\" data-image=\"{urlHelper.ContentAbsUrl(urlHelper.Action("Picture", "Pictures", new { id= picture.PictureId }))}\"><i class=\"{b.Net.LogoCssClass}\" style=\"background-color:{b.Net.Color}\"></i></li>"
                });
                model.Html = model.Html.Replace("[IG]" + strValue + "[/IG]", $"<div class=\"infographic-page-item\"><a href=\"{url}\"><img alt=\"{HttpUtility.HtmlEncode(picture.Caption)}\" src=\"/pictures/picture/{strValue.ToLower()}\" style=\"max-width:100%;\" /></a>{share}</div>");
            }
        }
    }
}