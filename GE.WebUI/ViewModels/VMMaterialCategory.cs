using SX.WebCore.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace GE.WebUI.ViewModels
{
    public sealed class VMMaterialCategory : SxVMMaterialCategory
    {
        [Display(Name="Игра"), UIHint("_GameLookupGrid")]
        public int? GameId { get; set; }
        public VMGame Game { get; set; }
    }
}