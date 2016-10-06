using SX.WebCore;
using SX.WebCore.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GE.WebUI.ViewModels.Abstracts
{
    [MetadataType(typeof(VMMaterialMetadata))]
    public class VMMaterial : SxVMMaterial
    {
        public int? GameId { get; set; }
        public VMGame Game { get; set; }

        public string GameVersion { get; set; }

        public sealed override string GetUrl(UrlHelper urlHelper)
        {
            switch (ModelCoreType)
            {
                case Enums.ModelCoreType.Article:
                    return urlHelper.Action("Details", "Articles", new { year = DateCreate.Year, month = DateCreate.Month.ToString("00"), day = DateCreate.Day.ToString("00"), titleUrl = TitleUrl });
                case Enums.ModelCoreType.News:
                    return urlHelper.Action("Details", "News", new { year = DateCreate.Year, month = DateCreate.Month.ToString("00"), day = DateCreate.Day.ToString("00"), titleUrl = TitleUrl });
                case Enums.ModelCoreType.Humor:
                    return urlHelper.Action("Details", "Humor", new { year = DateCreate.Year, month = DateCreate.Month.ToString("00"), day = DateCreate.Day.ToString("00"), titleUrl = TitleUrl });
                case Enums.ModelCoreType.Aphorism:
                    return urlHelper.Action("Details", "Aphorisms", new { categoryId=CategoryId, titleUrl =TitleUrl });
                default:
                    return "#";
            }
        }
    }

    public class VMMaterialMetadata : SxVMMaterialMetadata
    {
        [Display(Name = "Игра"), UIHint("_GameLookupGrid")]
        public int? GameId { get; set; }

        [MaxLength(100, ErrorMessageResourceType = typeof(SX.WebCore.Resources.Messages), ErrorMessageResourceName = "MaxLengthField")]
        [Display(Name = "Версия игры")]
        public string GameVersion { get; set; }

        [Display(Name = "Показывать")]
        public new bool Show { get; set; }

        [Display(Name = "Показывать в избранных")]
        public new bool IsTop { get; set; }
    }
}