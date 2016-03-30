using System.ComponentModel.DataAnnotations;

namespace GE.WebAdmin.Models
{
    public sealed class VMEditSeoKeyword
    {
        public int Id { get; set; }

        public VMSeoInfo SeoInfo { get; set; }

        [Required]
        public int SeoInfoId { get; set; }

        [MaxLength(50), Required, Display(Name="Значение ключевого слова")]
        public string Value { get; set; }
    }
}