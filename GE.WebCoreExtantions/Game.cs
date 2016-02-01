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
    public class Game : SX.WebCore.Abstract.SxDbUpdatedModel<int>
    {
        [Column("TITLE"), MaxLength(100), Required]
        public string Title { get; set; }

        [Column("SHOW")]
        public bool Show { get; set; }

        [Column("DESCRIPTION"), MaxLength(255)]
        public string Description { get; set; }
    }
}
