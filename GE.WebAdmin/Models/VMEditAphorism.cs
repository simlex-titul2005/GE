using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GE.WebAdmin.Models
{
    public sealed class VMEditAphorism
    {
        public int Id { get; set; }

        [Display(Name ="Заголовок"), Required, MaxLength(150)]
        public string Title { get; set; }

        [Display(Name = "Показывать на сайте"), Required]
        public bool Show { get; set; }

        [Display(Name = "Вступление"), Required, DataType(DataType.MultilineText)]
        public string Foreword { get; set; }

        [Display(Name = "Содержание"), Required, DataType(DataType.MultilineText), AllowHtml]
        public string Html { get; set; }

        public VMMaterialCategory Category { get; set; }

        [Required]
        public string CategoryId { get; set; }

        [Required, MaxLength(255)]
        public string TitleUrl { get; set; }

        public VMAuthorAphorism Author { get; set; }
        [Display(Name = "Автор"), UIHint("AuthorAphorismLookupGrid")]
        public int? AuthorId { get; set; }

        [Display(Name ="Источник"), MaxLength(255), DataType(DataType.Url)]
        public string SourceUrl { get; set; }
    }
}