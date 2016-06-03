using SX.WebCore.Abstract;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SX.WebCore
{
    [Table("D_SITE_TEST")]
    public class SxSiteTest : SxDbUpdatedModel<int>
    {
        [Required, MaxLength(200)]
        public string Title { get; set; }

        [Required, MaxLength(1000)]
        public string Description { get; set; }

        public virtual ICollection<SxSiteTestBlock> Blocks { get; set; }
    }
}
