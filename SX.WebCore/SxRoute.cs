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

        [MaxLength(100), Column("DOMAIN")]
        public string Domain { get; set; }

        [Required, MaxLength(100), Column("CONTROLLER")]
        public string Controller { get; set; }

        [Required, MaxLength(100), Column("ACTION")]
        public string Action { get; set; }

        public virtual ICollection<SxRouteValue> Values { get; set; }

        [NotMapped]
        public string Url
        {
            get
            {
                var url = string.Format("{0}{1}/{2}",
                    !string.IsNullOrEmpty(Domain) ? Domain + "/" : null,
                    !string.IsNullOrEmpty(Domain) ? Controller : "/" + Controller,
                    Action
                    );
                return url;
            }
        }
    }
}
