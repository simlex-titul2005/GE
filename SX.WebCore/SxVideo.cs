using SX.WebCore.Abstract;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static SX.WebCore.Enums;

namespace SX.WebCore
{
    [Table("D_VIDEO")]
    public class SxVideo : SxDbUpdatedModel<Guid>
    {
        [Required, MaxLength(255)]
        public string Title { get; set; }

        [Required, MaxLength(255)]
        public string Url { get; set; }

        public virtual SxPicture Picture { get; set; }
        public Guid PictureId { get; set; }
    }
}
