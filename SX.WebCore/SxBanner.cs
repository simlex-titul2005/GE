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

            Top=1,

            Right=2,

            Bottom=3,

            Left=4
        }

        public int ClicksCount { get; set; }
    }
}
