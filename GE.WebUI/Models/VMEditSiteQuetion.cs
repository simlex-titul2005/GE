using System.ComponentModel.DataAnnotations;

namespace GE.WebUI.Models
{
    public sealed class VMEditSiteQuetion
    {
        public int Id { get; set; }

        [Display(Name ="Как к Вам обращаться?*"), Required, MaxLength(50)]
        public string UserName { get; set; }

        [Display(Name = "Email*"), Required, MaxLength(50), DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Ваш вопрос, предложение или претензия*"), Required, MaxLength(400), DataType(DataType.MultilineText)]
        public string Text { get; set; }
    }
}