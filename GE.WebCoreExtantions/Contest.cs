using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GE.WebCoreExtantions
{
    [Table("D_CONTEST")]
    public class Contest : SX.WebCore.Abstract.Material
    {
        [Column("DATE_START")]
        public DateTime DateStart { get; set; }
    }
}
