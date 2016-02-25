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
        [MaxLength(100), Required]
        public string Title { get; set; }

        [MaxLength(100),]
        public string TitleAbbr { get; set; }

        public bool Show { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }

        public Guid? FrontPictureId { get; set; }
        public virtual SxPicture FrontPicture { get; set; }

        public Guid? GoodPictureId { get; set; }
        public virtual SxPicture GoodPicture { get; set; }

        public Guid? BadPictureId { get; set; }
        public virtual SxPicture BadPicture { get; set; }
    }
}
