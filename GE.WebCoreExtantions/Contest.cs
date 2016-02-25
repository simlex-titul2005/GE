using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GE.WebCoreExtantions
{
    [Table("D_CONTEST")]
    public class Contest : SX.WebCore.Abstract.SxMaterial
    {
        [DataType(DataType.DateTime)]
        public DateTime DateStart { get; set; }

        public virtual ICollection<ContestPrize> Prizes { get; set; }
    }
}
