using SX.WebCore.ViewModels;
using System;

namespace GE.WebUI.Models.Abstract
{
    public class VMDetailMaterial : VMLastMaterial
    {
        public DateTime DateUpdate { get; set; }
        public string GameTitleUrl { get; set; }
        public string CategoryId { get; set; }
        public bool ShowFrontPictureOnDetailPage { get; set; }
        public string Html { get; set; }
        public int LikeUpCount { get; set; }
        public int LikeDownCount { get; set; }
        public string UserNikName { get; set; }
        public VMMateriallnfo Info
        {
            get
            {
                return new VMMateriallnfo
                {
                    DateOfPublication = DateOfPublication,
                    CommentsCount = CommentsCount,
                    ViewsCount = ViewsCount,
                    LikeUpCount = LikeUpCount,
                    LikeDownCount = LikeDownCount
                };
            }
        }
        public SxVMSeoTags SeoTags { get; set; }
    }
}