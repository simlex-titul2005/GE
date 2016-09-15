using System;

namespace GE.WebUI.ViewModels
{
    public sealed class VMGameMenuGame
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string TitleUrl { get; set; }
        public string TitleAbbr { get; set; }
        public Guid FrontPictureId { get; set; }
        public Guid GoodPictureId { get; set; }
        public Guid BadPictureId { get; set; }
        public int ImgWidth { get; set; }
        public int IconHeight { get; set; }
    }
}