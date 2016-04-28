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
    }
}
