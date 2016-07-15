using SX.WebCore.Abstract;
using SX.WebCore.Attrubutes;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using static SX.WebCore.Enums;

namespace GE.WebAdmin.Models
{
    [MetadataType(typeof(VMEditHumorMetadata))]
    public sealed class VMEditHumor : SxMaterial
    {
        [MaxLength(255)]
        public string OldTitleUrl { get; set; }
    }

    sealed class VMEditHumorMetadata
    {
        [Display(Name = "Название статьи"), MaxLength(255), Required, MaxWordsCount(15), MinWordsCount(1)]
        public string Title { get; set; }

        [Display(Name = "Контент"), Required, DataType(DataType.MultilineText), AllowHtml]
        public string Html { get; set; }

        [Display(Name = "Вступление"), DataType(DataType.MultilineText)]
        public string Foreword { get; set; }

        [Display(Name = "Показывать"), Required]
        public bool Show { get; set; }

        [Display(Name = "Изображение"), UIHint("PicturesLookupGrid")]
        public Guid? FrontPictureId { get; set; }

        [Display(Name = "Показывать картинку на странице")]
        public bool ShowFrontPictureOnDetailPage { get; set; }

        [Display(Name = "Строковый ключ"), MaxLength(255), Required]
        public string TitleUrl { get; set; }

        [Display(Name = "Дата публикации"), UIHint("EditDate")]
        public DateTime DateOfPublication { get; set; }

        [Display(Name = "Категория"), UIHint("MaterialCategoryLookupGrid"), AdditionalMetadata("mct", ModelCoreType.Humor)]
        public string CategoryId { get; set; }

        [Display(Name = "Показывать в избранных")]
        public bool IsTop { get; set; }
    }
}