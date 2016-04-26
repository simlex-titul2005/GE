using System.ComponentModel.DataAnnotations;

namespace GE.WebAdmin.Models
{
    public sealed class VMEditBannedUrl
    {
        [Display(Name = "Адрес")]
        public int Id { get; set; }

        [Required, MaxLength(255), Display(Name = "Адрес"), DataType(DataType.Url)]
        public string Url { get; set; }

        [Required, MaxLength(255), Display(Name = "Причина бана"), DataType(DataType.MultilineText)]
        public string Couse { get; set; }
    }
}