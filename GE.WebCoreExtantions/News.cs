using GE.WebCoreExtantions.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GE.WebCoreExtantions
{
    [Table("D_NEWS")]
    public class News : SX.WebCore.SxNews, IHasGame
    {
        public virtual Game Game { get; set; }
        public int? GameId { get; set; }
    }
}
