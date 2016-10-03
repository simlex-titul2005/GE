using SX.WebCore.ViewModels;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GE.WebUI.ViewModels
{
    public sealed class VMAuthorAphorism
    {
        public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(SX.WebCore.Resources.Messages), ErrorMessageResourceName = "RequiredField")]
        [MaxLength(110, ErrorMessageResourceType = typeof(SX.WebCore.Resources.Messages), ErrorMessageResourceName = "MaxLengthField")]
        [Display(Name= "Строковый ключ")]
        public string TitleUrl { get; set; }

        [Required(ErrorMessageResourceType = typeof(SX.WebCore.Resources.Messages), ErrorMessageResourceName = "RequiredField")]
        [MaxLength(100, ErrorMessageResourceType = typeof(SX.WebCore.Resources.Messages), ErrorMessageResourceName = "MaxLengthField")]
        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Required(ErrorMessageResourceType = typeof(SX.WebCore.Resources.Messages), ErrorMessageResourceName = "RequiredField")]
        [MaxLength(400, ErrorMessageResourceType = typeof(SX.WebCore.Resources.Messages), ErrorMessageResourceName = "MaxLengthField")]
        [Display(Name = "Краткое описание"), DataType(DataType.MultilineText)]
        public string Foreword { get; set; }

        [Display(Name = "Описание"), AllowHtml]
        public string Description { get; set; }

        public SxVMPicture Picture { get; set; }
        [Display(Name = "Фото"), UIHint("_PicturesLookupGrid")]
        public Guid? PictureId { get; set; }
    }
}