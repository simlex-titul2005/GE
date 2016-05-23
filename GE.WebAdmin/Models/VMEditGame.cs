using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using static SX.WebCore.Enums;

namespace GE.WebAdmin.Models
{
    public sealed class VMEditGame
    {
        public int Id { get; set; }

        [Required, Display(Name="Полное наименование игры")]
        public string Title { get; set; }

        [Required, Display(Name = "Ключ формирования ссылки"), RegularExpression(@"^[A-Za-z0-9]([-]*[A-Za-z0-9])*$", ErrorMessage="Поле должно содержать только буквы латинского алфавита и тире")]
        public string TitleUrl { get; set; }

        [Display(Name = "Сокращенное наименование игры")]
        public string TitleAbbr { get; set; }

        [DataType(DataType.MultilineText), Display(Name="Описание")]
        public string Description { get; set; }

        [AllowHtml, Display(Name = "Полное описание"), DataType(DataType.MultilineText)]
        public string FullDescription { get; set; }

        [Display(Name = "Показывать")]
        public bool Show { get; set; }

        [Display(Name="Иконка"), UIHint("EditImage"), Required]
        public Guid? FrontPictureId { get; set; }

        [Display(Name = "Картинка для новостей"), UIHint("EditImage"), Required]
        public Guid? GoodPictureId { get; set; }

        [Display(Name = "Картинка для статей"), UIHint("EditImage"), Required]
        public Guid? BadPictureId { get; set; }
    }
}