using SX.WebCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GE.WebUI.Models.Abstract
{
    public class VMDetailMaterial
    {
        public int Id { get; set; }
        public string GameTitleUrl { get; set; }
        public Enums.ModelCoreType ModelCoreType { get; set; }
        public Guid? FrontPictureId { get; set; }
        public string Title { get; set; }
        public string Foreword { get; set; }
        public string Html { get; set; }
        public DateTime DateOfPublication { get; set; }
        public int CommentsCount { get; set; }
        public int ViewsCount { get; set; }
        public int LikeUpCount { get; set; }
        public int LikeDownCount { get; set; }
        public VMMateriallnfo Info
        {
            get
            {
                return new VMMateriallnfo
                {
                    DateOfPublication = this.DateOfPublication,
                    CommentsCount = this.CommentsCount,
                    ViewsCount = this.ViewsCount,
                    LikeUpCount = this.LikeUpCount,
                    LikeDownCount = this.LikeDownCount
                };
            }
        }
    }
}