using System;
using System.ComponentModel.DataAnnotations;

namespace GE.WebAdmin.Models
{
    public sealed class VMEditPicture
    {
        public Guid Id { get; set; }

        [Display(Name="Название")]
        public string Caption { get; set; }

        [Display(Name = "Описание"), DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        public string ImgFormat { get; set; }
    }
}