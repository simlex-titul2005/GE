using System;
using System.ComponentModel.DataAnnotations;

namespace GE.WebAdmin.Models
{
    public sealed class VMEditBannerGroup
    {
        public VMEditBannerGroup()
        {
            Banners = new VMBanner[0];
        }

        public Guid Id { get; set; }

        [Required, MaxLength(100), Display(Name = "Заголовок")]
        public string Title { get; set; }

        [MaxLength(400), Display(Name = "Описание"), DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "Привязанные баннеры"), UIHint("AddBanner")]
        public VMBanner[] Banners { get; set; }
    }
}