using System.Web.Mvc;
using SX.WebCore.ViewModels;

namespace GE.WebUI.Models
{
    public class VMPreviewArticle : SxVMMaterial
    {
        public int? GameId { get; set; }
        public VMGame Game { get; set; }
        public override string Url(UrlHelper url)
        {
            return url.Action("Details", "Articles", new { year = DateCreate.Year, month = DateCreate.Month.ToString("00"), day = DateCreate.Day.ToString("00"), titleUrl = TitleUrl });
        }
    }
}