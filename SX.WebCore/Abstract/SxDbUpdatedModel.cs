using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SX.WebCore.Abstract
{
    public abstract class DbUpdatedModel<TKey> : DBModel<TKey>
    {
        [Column("DATE_UPDATE"), DataType(DataType.DateTime)]
        public DateTime DateUpdate { get; set; }
    }
}
