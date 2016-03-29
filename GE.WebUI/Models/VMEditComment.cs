using System.ComponentModel.DataAnnotations;
using static SX.WebCore.Enums;

namespace GE.WebUI.Models
{
    public sealed class VMEditComment
    {
        public int MaterialId { get; set; }
        public ModelCoreType ModelCoreType { get; set; }

        [Display(Name ="Представьтесь, пожалуйста"), MaxLength(40), Required(ErrorMessage ="Поле обязательно для заполнения")]
        public string UserName { get; set; }

        [DataType(DataType.MultilineText), Display(Name = "Текст комментария"), Required(ErrorMessage = "Поле обязательно для заполнения")]
        public string Html { get; set; }
    }
}