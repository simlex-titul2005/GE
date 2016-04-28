using System;
using System.ComponentModel.DataAnnotations;

namespace GE.WebAdmin.Models
{
    public sealed class VMEditBanner
    {
        public Guid Id { get; set; }

        [MaxLength(100), Display(Name ="Заголовок")]
        public string Title { get; set; }

        [Required, Display(Name ="Картинка"), UIHint("EditImage")]
        public Guid? PictureId { get; set; }

        [Required, MaxLength(255), Display(Name = "Ссылка"), DataType(DataType.Url)]
        public string Url { get; set; }
    }
}