using SX.WebCore.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace GE.WebAdmin.Models
{
    public sealed class VMEditMaterialCategory : SxVMEditMaterialCategory
    {
        public VMGame Game { get; set; }
        [Display(Name ="Игра"), UIHint("GameLookupGrid")]
        public int? GameId { get; set; }
    }
}