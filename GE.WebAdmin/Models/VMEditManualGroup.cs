using System.ComponentModel.DataAnnotations;

namespace GE.WebAdmin.Models
{
    public sealed class VMEditManualGroup
    {
        [Required, RegularExpression(@"^[A-Za-z0-9]([-]*[A-Za-z0-9])*$", ErrorMessage = "Поле должно содержать только буквы латинского алфавита и тире"), Display(Name ="Идентификатор"), MaxLength(128)]
        public string Id { get; set; }

        [Required, Display(Name = "Заголовок"), MaxLength(100)]
        public string Title { get; set; }

        public VMManualGroup ParentGroup { get; set; }
        public string ParentGroupId { get; set; }
    }
}