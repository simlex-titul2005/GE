using System.ComponentModel.DataAnnotations;

namespace GE.WebAdmin.Models
{
    public sealed class VMEditForumPart
    {
        public int Id { get; set; }

        [MaxLength(255), Required, Display(Name ="Наименование раздела")]
        public string Title { get; set; }

        [Required, Display(Name = "Описание раздела"), DataType(DataType.MultilineText)]
        public string Html { get; set; }
    }
}