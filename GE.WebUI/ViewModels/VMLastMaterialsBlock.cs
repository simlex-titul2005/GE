using GE.WebUI.ViewModels.Abstracts;
using SX.WebCore.ViewModels;
using System;

namespace GE.WebUI.ViewModels
{
    public sealed class VMLastMaterialsBlock
    {
        public VMLastMaterialsBlock()
        {
            Materials = new VMMaterial[0];
            Games = new VMGame[0];
            GameMaterials = new VMMaterial[0];
        }

        public VMMaterial[] Materials { get; set; }
        public VMGame[] Games { get; set; }
        public VMMaterial[] GameMaterials { get; set; }
        public Tuple<int, SxVMMaterialTag> GameTags { get; set; }
    }
}