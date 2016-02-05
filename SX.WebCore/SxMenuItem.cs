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
    [Table("D_MENU_ITEM")]
    public class SxMenuItem : SxDbUpdatedModel<int>
    {
        [Required, MaxLength(100), Column("CAPTION")]
        public string Caption { get; set; }

        public virtual SxMenu Menu { get; set; }
        [Column("MENU_ID")]
        public int MenuId { get; set; }

        public virtual SxRoute Route { get; set; }
        [Column("ROUTE_ID")]
        public Guid? RouteId { get; set; }
    }
}
