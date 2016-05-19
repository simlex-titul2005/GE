using SX.WebCore.Attrubutes;
using System.ComponentModel.DataAnnotations;
using static SX.WebCore.Enums;

namespace GE.WebAdmin.Models
{
    public sealed class VMEditSeoInfo
    {
        public VMEditSeoInfo()
        {
            Keywords = new VMSeoKeyword[0];
        }

        public int Id { get; set; }

        [MaxLength(255), Display(Name="Адрес")]
        public string RawUrl { get; set; }

        [MaxLength(255), Required, Display(Name = "Заголовок страницы"), MaxWordsCount(15), MinWordsCount(7)]
        public string SeoTitle { get; set; }

        [MaxLength(700), MinLength(150) Display(Name = "Описание страницы"), DataType(DataType.MultilineText)]
        public string SeoDescription { get; set; }

        public VMSeoKeyword[] Keywords { get; set; }

        [MaxLength(80), Display(Name = "Тег h1"), MaxWordsCount(6), MinWordsCount(2)]
        public string H1 { get; set; }

        [MaxLength(20), Display(Name = "Css стиль тега h1")]
        public string H1CssClass { get; set; }

        public int? MaterialId { get; set; }
        public ModelCoreType? ModelCoreType { get; set; }
    }
}