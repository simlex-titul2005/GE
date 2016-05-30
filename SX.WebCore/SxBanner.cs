using SX.WebCore.Abstract;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SX.WebCore
{
    [Table("D_BANNER")]
    public class SxBanner : SxDbUpdatedModel<Guid>
    {
        [MaxLength(100)]
        public string Title { get; set; }

        public SxPicture Picture { get; set; }
        public Guid PictureId { get; set; }

        [Required, MaxLength(255)]
        public string Url { get; set; }

        [MaxLength(50)]
        public string ControllerName { get; set; }

        [MaxLength(50)]
        public string ActionName { get; set; }

        public BannerPlace Place { get; set; }

        public enum BannerPlace : byte
        {
            Unknown=0,

            /// <summary>
            /// 320 x 90
            /// </summary>
            Brand = 1,

            /// <summary>
            /// 1270 x90
            /// </summary>
            Top=2,

            /// <summary>
            /// 320 x 90
            /// </summary>
            TopR = 3,

            /// <summary>
            /// 1903 x 90
            /// </summary>
            Bottom=10,

            /// <summary>
            /// 1140 x 90
            /// </summary>
            T=11,

            R=12,

            B=13,

            L=14
        }
    }
}
