using System;

namespace GE.WebUI.ViewModels
{
    public sealed class VMFGBGame
    {
        public VMFGBGame()
        {
            MaterialCategories = new VMMaterialCategory[0];
        }
        public int Id { get; set; }
        public Guid FrontPictureId { get; set; }
        public string Title { get; set; }
        public string TitleUrl { get; set; }
        public string Description { get; set; }

        public VMMaterialCategory[] MaterialCategories { get; set; }
        public int MaterialCategoryLength { get { return MaterialCategories.Length; } }
        public bool HasMaterialCategories { get { return MaterialCategories.Length != 0; } }
    }
}