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
                    return urlHelper.Action(MVC.News.Details(DateCreate.Year, DateCreate.Month.ToString("00"), DateCreate.Day.ToString("00"), TitleUrl));
                case SX.WebCore.Enums.ModelCoreType.Article:
                    return urlHelper.Action(MVC.Articles.Details(DateCreate.Year, DateCreate.Month.ToString("00"), DateCreate.Day.ToString("00"), TitleUrl));
                default:
                    return "#";
            }
        }
    }
}