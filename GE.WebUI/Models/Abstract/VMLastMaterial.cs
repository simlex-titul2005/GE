using System;
using static SX.WebCore.Enums;

namespace GE.WebUI.Models.Abstract
{
    public abstract class VMLastMaterial
    {
        public DateTime DateOfPublication { get; set; }
        public ModelCoreType ModelCoreType { get; set; }
        public DateTime DateCreate { get; set; }
        public string Title { get; set; }
        public string TitleUrl { get; set; }
        public Guid? FrontPictureId { get; set; }
    }
}