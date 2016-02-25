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
    [Table("D_ROUTE_VALUE")]
    public class SxRouteValue : SxDbUpdatedModel<Guid>
    {
        [Required, MaxLength(50)]
        public string Name { get; set; }

        [Required, MaxLength(100)]
        public string Value { get; set; }

        public virtual SxRoute Route { get; set; }
        
        public Guid RouteId { get; set; }
    }
}
