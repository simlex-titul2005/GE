using GE.WebUI.ViewModels.Abstracts;
using System.ComponentModel.DataAnnotations;

namespace GE.WebUI.ViewModels
{
    [MetadataType(typeof(VMArticleMetadata))]
    public sealed class VMArticle : VMMaterial
    {
        [MaxLength(100, ErrorMessageResourceType = typeof(SX.WebCore.Resources.Messages), ErrorMessageResourceName = "MaxLengthField")]
        public string GameVersion { get; set; }
    }

    public sealed class VMArticleMetadata : VMMaterialMetadata
    {
        [Display(Name = "Игра"), UIHint("_GameLookupGrid")]
        public int? GameId { get; set; }

        [Display(Name = "Версия игры")]
        public string GameVersion { get; set; }

        [Display(Name = "Показывать")]
        public new bool Show { get; set; }

        [Display(Name = "Показывать в избранных")]
        public new bool IsTop { get; set; }
    }
}