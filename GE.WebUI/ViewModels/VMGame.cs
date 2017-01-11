using SX.WebCore.ViewModels;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GE.WebUI.ViewModels
{
    public sealed class VMGame
    {
        public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(SX.WebCore.Resources.Messages), ErrorMessageResourceName = "RequiredField")]
        [MaxLength(100, ErrorMessageResourceType = typeof(SX.WebCore.Resources.Messages), ErrorMessageResourceName = "MaxLengthField")]
        [Display(Name ="Наименование")]
        public string Title { get; set; }

        [Required(ErrorMessageResourceType = typeof(SX.WebCore.Resources.Messages), ErrorMessageResourceName = "RequiredField")]
        [MaxLength(100, ErrorMessageResourceType = typeof(SX.WebCore.Resources.Messages), ErrorMessageResourceName = "MaxLengthField")]
        [Display(Name = "Строковый ключ")]
        public string TitleUrl { get; set; }

        [MaxLength(100, ErrorMessageResourceType = typeof(SX.WebCore.Resources.Messages), ErrorMessageResourceName = "MaxLengthField")]
        [Display(Name = "Краткое наименование")]
        public string TitleAbbr { get; set; }

        [Display(Name = "Показывать")]
        public bool Show { get; set; }

        [MaxLength(255, ErrorMessageResourceType = typeof(SX.WebCore.Resources.Messages), ErrorMessageResourceName = "MaxLengthField")]
        [Display(Name = "Краткое описание")]
        public string Description { get; set; }

        [Display(Name = "Полное описание"), AllowHtml]
        public string FullDescription { get; set; }

        [Display(Name = "Иконка игры"), UIHint("_PicturesLookupGrid")]
        public Guid? FrontPictureId { get; set; }
        public SxVMPicture FrontPicture { get; set; }

        [Display(Name = "Картинка для новостей"), UIHint("_PicturesLookupGrid")]
        public Guid? GoodPictureId { get; set; }
        public SxVMPicture GoodPicture { get; set; }

        [Display(Name = "Картинка для статей"), UIHint("_PicturesLookupGrid")]
        public Guid? BadPictureId { get; set; }
        public SxVMPicture BadPicture { get; set; }

        public int? SteamAppId { get; set; }
        public VMSteamApp SteamApp { get; set; }
    }
}