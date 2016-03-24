using SX.WebCore.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static SX.WebCore.Enums;

namespace SX.WebCore
{
    [Table("D_LIKE")]
    public class SxLike : SxDbModel<int>
    {
        public virtual SxMaterial Material { get; set; }
        public ModelCoreType ModelCoreType { get; set; }
        public int MaterialId { get; set; }

        public virtual SxAppUser User { get; set; }
        [Index, MaxLength(128)]
        public string UserId { get; set; }

        [Index]
        public byte Direction { get; set; }

        [Index, Required, MaxLength(50)]
        public string SessionId { get; set; }

        /// <summary>
        /// Использовать для возврата общего кол-ва лайков
        /// </summary>
        [NotMapped]
        public int Count { get; set; }
    }
}
