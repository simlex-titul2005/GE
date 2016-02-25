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
        [MaxLength(50), Required]
        public string ImgFormat { get; set; }

        [Required]
        public byte[] OriginalContent { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        [Required, MaxLength(100)]
        public string Caption { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }
    }
}
