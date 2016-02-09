using SX.WebCore.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SX.WebCore
{
    /// <summary>
    /// Детализированные фото (по длине)
    /// </summary>
    [Table("D_PICTURE_DETAIL")]
    public class SxPictureDetail : SxDbUpdatedModel<Guid>
    {
        [Column("WIDTH")]
        public int Width { get; set; }

        [Column("HEIGHT")]
        public int Height { get; set; }

        public virtual SxPicture Picture { get; set; }
        [Column("PICTURE_ID")]
        public Guid PictureId { get; set; }
    }
}
