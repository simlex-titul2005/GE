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
    [Table("D_PICTURE")]
    public class SxPicture : DbUpdatedModel<Guid>
    {
        [Column("IMG_TYPE")]
        public Enums.ImgType ImgType { get; set; }

        [Column("CONTENT"), MaxLength(1024)]
        public byte[] Content { get; set; }

        [Column("WIDTH")]
        public int Width { get; set; }

        [Column("HEIGHT")]
        public int Height { get; set; }

        public SxMaterial Material { get; set; }
        [Column("MATERIAL_ID")]
        public int MaterialId { get; set; }

        [Column("MATERIAL_CORE_TYPE")]
        public Enums.ModelType MaterialCoreType { get; set; }
    }
}
