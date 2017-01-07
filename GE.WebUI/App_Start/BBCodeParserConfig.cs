using GE.WebUI.ViewModels.Abstracts;
using SX.WebCore.ViewModels;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace GE.WebUI
{
    public static class BBCodeParserConfig
    {
        public static void ReplaceInfographics(SxVMMaterial model)
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

                model.Html = model.Html.Replace("[IG]" + strValue + "[/IG]", $"<a href=\"/infographics/{strValue.ToLower()}\"><img alt=\"{HttpUtility.HtmlEncode(picture.Caption)}\" src=\"/pictures/picture/{strValue.ToLower()}\" style=\"max-width:100%;\" /></a>");
            }
        }
    }
}