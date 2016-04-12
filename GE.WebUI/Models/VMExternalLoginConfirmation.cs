using System.ComponentModel.DataAnnotations;

namespace GE.WebUI.Models
{
    public sealed class VMExternalLoginConfirmation
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}