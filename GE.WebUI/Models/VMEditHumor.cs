using System.ComponentModel.DataAnnotations;

namespace GE.WebUI.Models
{
    public sealed class VMEditHumor
    {
        [Display(Name="Категория юмора"), Required]
        public string CategoryId { get; set; }

        [Display(Name ="Представьтесь пожалуйста"), MaxLength(100), Required(ErrorMessage ="Введите Ваше имя")]
        public string UserName { get; set; }

        [Display(Name = "Заголовок"), MaxLength(255), Required(ErrorMessage = "Введите заголовок")]
        public string Title { get; set; }

        [Display(Name ="Текст"), Required(ErrorMessage ="Текст не должен быть пустым"), DataType(DataType.MultilineText)]
        public string Html { get; set; }
    }
}