using System.ComponentModel.DataAnnotations;

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

        public VMAuthorAphorism Author { get; set; }
        [Display(Name = "Автор"), UIHint("EditAuthorAphorism")]
        public int AuthorId { get; set; }
    }
}