using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GE.WebAdmin.Models
{
    public sealed class VMEditAphorism
    {
        public int Id { get; set; }

        [Display(Name ="Заголовок"), Required, MaxLength(150)]
        public string Title { get; set; }

        [Display(Name = "Содержание"), Required, DataType(DataType.MultilineText)]
        public string Html { get; set; }

        public VMMaterialCategory Category { get; set; }

        [Required]
        public string CategoryId { get; set; }

        [Required, MaxLength(255)]
        public string TitleUrl { get; set; }

        [MaxLength(50), Display(Name ="Автор")]
        public string Author { get; set; }
    }
}