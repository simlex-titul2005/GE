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
    [Table("D_SEO_KEYWORD")]
    public class SeoKeyWord : DbUpdatedModel<int>
    {
        [Column("VALUE"), MaxLength(50), Required]
        string Value { get; set; }

        public virtual SeoInfo SeoInfo { get; set; }
        [Column("SEO_INFO_ID")]
        public int SeoInfoId { get; set; }
    }
}
