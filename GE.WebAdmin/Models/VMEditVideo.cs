using System;
using System.ComponentModel.DataAnnotations;

namespace GE.WebAdmin.Models
{
    public sealed class VMEditVideo
    {
        public Guid Id { get; set; }

        [Required, MaxLength(255), Display(Name ="Название видео")]
        public string Title { get; set; }

        [Required, MaxLength(255), DataType(DataType.Url), Display(Name ="Адрес видео")]
        public string Url { get; set; }

        [Display(Name ="Изображение"), UIHint("EditImage")]
        public Guid? PictureId { get; set; }
    }
}