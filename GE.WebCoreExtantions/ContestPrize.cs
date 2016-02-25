using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GE.WebCoreExtantions
{
    [Table("D_CONTEST_PRIZE")]
    public class ContestPrize : SX.WebCore.Abstract.SxDbUpdatedModel<int>
    {
        [MaxLength(100), Required]
        public string Title { get; set; }

        public virtual Contest Contest { get; set; }
        public int ContestId { get; set; }

        public SX.WebCore.Enums.ModelCoreType MaterialCoreType { get; set; }
    }
}
