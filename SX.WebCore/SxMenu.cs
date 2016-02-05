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
    [Table("D_MENU")]
    public class SxMenu : SxDbUpdatedModel<int>
    {
        [Required, MaxLength(100), Column("NAME")]
        public string Name { get; set; }

        public virtual ICollection<SxMenuItem> Items { get; set; }
    }
}
