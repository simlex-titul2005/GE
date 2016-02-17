using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GE.WebUI.Models
{
    public sealed class VMGame
    {
        public string Title { get; set; }
        public string TitleAbbr { get; set; }
        public Guid FrontPictureId { get; set; }
        public Guid GoodPictureId { get; set; }
        public Guid BadPictureId { get; set; }
    }
}