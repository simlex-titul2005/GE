using SX.WebCore;
using SX.WebCore.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GE.WebCoreExtantions
{
    [Table("D_GAME")]
    public class Game : SX.WebCore.Abstract.SxDbUpdatedModel<int>, ISxHasFrontPicture
    {
        [Column("TITLE"), MaxLength(100), Required]
        public string Title { get; set; }

        [Column("TITLE_ABBR"), MaxLength(100),]
        public string TitleAbbr { get; set; }

        [Column("SHOW")]
        public bool Show { get; set; }

        [Column("DESCRIPTION"), MaxLength(255)]
        public string Description { get; set; }

        [Column("FRONT_PICTURE_ID")]
        public Guid? FrontPictureId { get; set; }
        public virtual SxPicture FrontPicture { get; set; }

        [Column("GOOD_PICTURE_ID")]
        public Guid? GoodPictureId { get; set; }
        public virtual SxPicture GoodPicture { get; set; }

        [Column("BAD_PICTURE_ID")]
        public Guid? BadPictureId { get; set; }
        public virtual SxPicture BadPicture { get; set; }
    }
}
