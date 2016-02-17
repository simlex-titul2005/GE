using SX.WebCore.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GE.WebAdmin.Models
{
    public sealed class VMEditPicture : ISxViewModel<Guid>
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