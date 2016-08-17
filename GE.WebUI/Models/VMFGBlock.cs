using SX.WebCore.ViewModels;
using System;

namespace GE.WebUI.Models
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

    public sealed class VMFGBGame
    {
        public VMFGBGame()
        {
            MaterialCategories = new SxVMMaterialCategory[0];
        }
        public int Id { get; set; }
        public Guid FrontPictureId { get; set; }
        public string Title { get; set; }
        public string TitleUrl { get; set; }
        public string Description { get; set; }

        public SxVMMaterialCategory[] MaterialCategories { get; set; }
        public int MaterialCategoryLength { get { return MaterialCategories.Length; } }
        public bool HasMaterialCategories { get { return MaterialCategories.Length != 0; } }
    }
}