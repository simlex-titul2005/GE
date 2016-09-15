using SX.WebCore.ViewModels;
using System;

namespace GE.WebUI.ViewModels
{
    public sealed class VMLGNBGame
    {
        public VMLGNBGame()
        {
            News = new VMLGNBNews[0];
            Tags = new SxVMMaterialTag[0];
            Videos = new VMLGNBVideo[0];
        }

        public int Id { get; set; }
        public string TitleUrl { get; set; }
        public string Title { get; set; }
        public VMLGNBNews[] News { get; set; }
        public SxVMMaterialTag[] Tags { get; set; }
        public VMLGNBVideo[] Videos { get; set; }
        public Guid? FrontPictureId { get; set; }
    }
}