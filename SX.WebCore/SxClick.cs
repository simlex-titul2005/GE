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
    /// Статистика кликов
    /// </summary>
    [Table("D_CLICK")]
    public class SxClick : SxDbModel<Guid>
    {
        /// <summary>
        /// Исходящий адрес
        /// </summary>
        [Required, MaxLength(255)]
        public string UrlRef { get; set; }

        /// <summary>
        /// Адрес назначения
        /// </summary>
        [Required, MaxLength(255)]
        public string RawUrl { get; set; }

        /// <summary>
        /// Тип клика: _traget, _parent, _self, _top
        /// </summary>
        [MaxLength(20)]
        public string LinkTarget { get; set; }

        public virtual SxClickType ClickType { get; set; }
        public int ClickTypeId { get; set; }
    }
}
