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
    [Table("D_SEO_INFO")]
    public class SeoInfo : DbUpdatedModel<int>
    {
        [Column("SEO_TITLE"), MaxLength(255), Required]
        public string SeoTitle { get; set; }

        [Column("SEO_DESCRIPTION"), MaxLength(1000)]
        public string SeoDescription { get; set; }

        public virtual ICollection<SeoKeyWord> SeoKeyWords { get; set; }

        public virtual Material Material { get; set; }
        [Column("MATERIAL_ID")]
        public int MeterialId { get; set; }

        [Column("MATERIAL_CORE_TYPE")]
        public Enums.ModelType MaterialCoreType { get; set; }
    }
}
