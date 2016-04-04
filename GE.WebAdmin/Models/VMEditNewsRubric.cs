using System.ComponentModel.DataAnnotations;

namespace GE.WebAdmin.Models
{
    public sealed class VMEditNewsRubric
    {
        [Required, MaxLength(128), Display(Name = "Значение рубрики"), RegularExpression(@"^[A-Za-z0-9]([-]*[A-Za-z0-9])*$", ErrorMessage = "Поле должно содержать только буквы латинского алфавита и тире")]
        public string Id { get; set; }

        [MaxLength(255), Display(Name ="Описание рубрики"), DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }
}