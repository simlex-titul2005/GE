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
        [Index]
        public DateTime DateOfPublication { get; set; }

        [MaxLength(255), Required]
        public string Title { get; set; }

        [MaxLength(255), Required, Index]
        public string TitleUrl { get; set; }

        public string Html { get; set; }

        [MaxLength(400)]
        public string Foreword { get; set; }

        public Enums.ModelCoreType ModelCoreType { get; set; }

        public bool Show { get; set; }

        public Guid? FrontPictureId { get; set; }
        public virtual SxPicture FrontPicture { get; set; }

        public int ViewsCount { get; set; }

        public int CommentsCount { get; set; }

        public virtual SxAppUser User { get; set; }
        [MaxLength(128)]
        public string UserId { get; set; }

        public ICollection<SxSeoInfo> SeoInfo { get; set; }
    }
}
