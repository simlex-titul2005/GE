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
    [Table("D_ROUTE")]
    public class SxRoute : SxDbUpdatedModel<Guid>
    {
        [Required, MaxLength(100), Column("NAME")]
        public string Name { get; set; }

        [Required, MaxLength(100), Column("CONTROLLER")]
        public string Controller { get; set; }

        [Required, MaxLength(100), Column("ACTION")]
        public string Action { get; set; }

        public virtual ICollection<SxRouteValue> Values { get; set; }
    }
}
