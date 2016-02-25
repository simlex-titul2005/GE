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
        [MaxLength(400), Required]
        public string Title { get; set; }

        public string Html { get; set; }

        public Enums.ModelCoreType ModelCoreType { get; set; }

        public bool Show { get; set; }

        public Guid? FrontPictureId { get; set; }
        public virtual SxPicture FrontPicture { get; set; }

        public int ViewsCount { get; set; }

        public int CommentsCount { get; set; }
    }
}
