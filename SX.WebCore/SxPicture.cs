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
        [Column("IMG_FORMAT"), MaxLength(50), Required]
        public string ImgFormat { get; set; }

        [Column("ORIGINAL_CONTENT"), Required]
        public byte[] OriginalContent { get; set; }

        [Column("WIDTH")]
        public int Width { get; set; }

        [Column("HEIGHT")]
        public int Height { get; set; }

        [Column("CAPTION"), Required, MaxLength(100)]
        public string Caption { get; set; }

        [Column("DESCRIPTION"), MaxLength(255)]
        public string Description { get; set; }

        public virtual ICollection<SxPictureDetail> Pictures { get; set; }
    }
}
