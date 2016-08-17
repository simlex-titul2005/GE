using SX.WebCore.ViewModels;
using System.Web.Mvc;

namespace GE.WebUI.Models
{
    public class VMMaterial : SxVMMaterial
    {
        public override string Url(UrlHelper url)
        {
            switch(ModelCoreType)
            {
                case SX.WebCore.Enums.ModelCoreType.Article:
                    return url.Action("Details", "Articles", new { year = DateCreate.Year, month = DateCreate.Month.ToString("00"), day = DateCreate.Day.ToString("00"), titleUrl = TitleUrl });
                case SX.WebCore.Enums.ModelCoreType.News:
                    return url.Action("Details", "News", new { year = DateCreate.Year, month = DateCreate.Month.ToString("00"), day = DateCreate.Day.ToString("00"), titleUrl = TitleUrl });
                default:
                    return "#";
            }
        }

        public int? GameId { get; set; }
        public VMGame Game { get; set; }
    }
}