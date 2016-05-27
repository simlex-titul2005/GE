using SX.WebCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace GE.WebAdmin.Models
{
    public sealed class VMEditBanner
    {
        public Guid Id { get; set; }

        [MaxLength(100), Display(Name ="Заголовок")]
        public string Title { get; set; }

        public VMPicture Picture { get; set; }
        [Required, Display(Name ="Картинка"), UIHint("EditImage")]
        public Guid? PictureId { get; set; }

        [Required, MaxLength(255), Display(Name = "Ссылка"), DataType(DataType.Url)]
        public string Url { get; set; }

        [Display(Name = "Место на странице")]
        public SxBanner.BannerPlace? Place { get; set; }

        [MaxLength(50), Display(Name ="Конроллер")]
        public string ControllerName { get; set; }

        [MaxLength(50), Display(Name = "Действие")]
        public string ActionName { get; set; }
    }
}