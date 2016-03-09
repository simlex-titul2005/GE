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
    /// <summary>
    /// Вид статистики кликов
    /// </summary>
    [Table("D_CLICK_TYPE")]
    public class SxClickType : SxDbUpdatedModel<int>
    {
        [Required, MaxLength(255)]
        public string Name { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }
    }
}
