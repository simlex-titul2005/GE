using GE.WebUI.Models.Abstract;
using System.Web.Mvc;

namespace GE.WebUI.Models
{
    public sealed class VMDetailGameMaterial : VMDetailMaterial
    {
        public string GetUrl(UrlHelper urlHelper)
        {
            switch (ModelCoreType)
            {
                case SX.WebCore.Enums.ModelCoreType.News:
                    return urlHelper.Action("details", new { controller = "news", year = DateCreate.Year, month = DateCreate.Month.ToString("00"), day = DateCreate.Day.ToString("00"), titleUrl = TitleUrl });
                case SX.WebCore.Enums.ModelCoreType.Article:
                    return urlHelper.Action("details", new { controller = "articles", year = DateCreate.Year, month = DateCreate.Month.ToString("00"), day = DateCreate.Day.ToString("00"), titleUrl = TitleUrl });
                case SX.WebCore.Enums.ModelCoreType.Aphorism:
                    return urlHelper.Action("details", new { categoryId = CategoryId, titleUrl = TitleUrl});
                default:
                    return "#";
            }
        }

        public int GameId { get; set; }
    }
}