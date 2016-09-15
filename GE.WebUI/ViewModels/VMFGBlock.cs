using GE.WebUI.ViewModels.Abstracts;

namespace GE.WebUI.ViewModels
{
    public sealed class VMFGBlock
    {
        public VMFGBlock()
        {
            Games = new VMFGBGame[0];
        }

        public VMFGBGame[] Games { get; set; }
        public int GameLength { get { return Games.Length; } }
        public bool HasGames { get { return GameLength != 0; } }
        public VMMaterial[] Articles { get; set; }
        public string SelectedGameTitle { get; set; }
    }
}