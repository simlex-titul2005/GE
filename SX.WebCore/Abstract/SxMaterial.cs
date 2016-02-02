using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SX.WebCore.Abstract
{
    [Table("DV_MATERIAL")]
    public abstract class SxMaterial : SxDbUpdatedModel<int>, ISxHasHtml, ISxHasFrontPicture
    {
        [Column("TITLE"), MaxLength(400), Required]
        public string Title { get; set; }

        [Column("HTML")]
        public string Html { get; set; }

        [Column("CORE_TYPE")]
        public Enums.ModelCoreType ModelCoreType { get; set; }

        [Column("SHOW")]
        public bool Show { get; set; }

        [Column("FRONT_PICTURE_ID")]
        public Guid? FrontPictureId { get; set; }
        public virtual SxPicture FrontPicture { get; set; }
    }
}
