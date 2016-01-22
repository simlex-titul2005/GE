using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SX.WebCore.Abstract
{
    public abstract class DBModel<TKey>
    {
        [Column("ID"), Required, Key]
        public TKey Id { get; set; }

        [Column("DATE_CREATE"), DataType(DataType.DateTime)]
        public DateTime DateCreate { get; set; }
    }
}
