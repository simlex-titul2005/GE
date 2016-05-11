using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace GE.WebAdmin.Models
{
    public sealed class VMEditAphorism
    {
        public int Id { get; set; }

        [Display(Name="Автор"), MaxLength(100)]
        public string Author { get; set; }

        [Display(Name = "Контент"), MaxLength(600), DataType(DataType.MultilineText), AllowHtml, Required]
        public string Html { get; set; }

        [MaxLength, Display(Name = "Категория"), Required]
        public string Category { get; set; }
    }
}