using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GE.WebAdmin.Models
{
    public sealed class VMEditAuthorAphorism
    {
        public int Id { get; set; }

        [MaxLength(110), Display(Name="Строковый ключ"), RegularExpression(@"^[A-Za-z0-9]([-]*[A-Za-z0-9])*$", ErrorMessage = "Поле должно содержать только буквы латинского алфавита и тире")]
        public string TitleUrl { get; set; }

        [Required, MaxLength(100), Display(Name = "Имя")]
        public string Name { get; set; }

        [Required, MaxLength(400), Display(Name="Краткое описание"), DataType(DataType.MultilineText)]
        public string Foreword { get; set; }

        [AllowHtml, DataType(DataType.MultilineText), Display(Name="Описание")]
        public string Description { get; set; }

        [Display(Name = "Изображение"), UIHint("PicturesLookupGrid")]
        public Guid? PictureId { get; set; }
    }
}