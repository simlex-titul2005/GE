using System;

namespace GE.WebUI.Models
{
    public class VMLGBNews
    {
        public DateTime DateOfPublication { get; set; }
        public DateTime DateCreate { get; set; }
        public string Title { get; set; }
        public string TitleUrl { get; set; }
        public int? GameId { get; set; }
        public Guid? FrontPictureId { get; set; }
    }
}