using SX.WebCore.ViewModels;

namespace GE.WebUI.ViewModels
{
    public sealed class VMMaterialCategory : SxVMMaterialCategory
    {
        public int? GameId { get; set; }
        public VMGame Game { get; set; }
    }
}