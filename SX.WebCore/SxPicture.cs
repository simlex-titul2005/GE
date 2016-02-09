using SX.WebCore.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SX.WebCore
{
    /// <summary>
    /// Оригинальное фото
    /// </summary>
    [Table("D_PICTURE")]
    public class SxPicture : SxDbUpdatedModel<Guid>
    {
        [Column("IMG_TYPE")]
        public Enums.ImgType ImgType { get; set; }

        [Column("ORIGINAL_CONTENT"), MaxLength(1024)]
        public byte[] OriginalContent { get; set; }

        [Column("WIDTH")]
        public int Width { get; set; }

        [Column("HEIGHT")]
        public int Height { get; set; }

        public virtual ICollection<SxPictureDetail> Pictures { get; set; }
    }
}
