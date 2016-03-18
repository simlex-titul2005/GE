using SX.WebCore.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static SX.WebCore.Enums;

namespace SX.WebCore
{
    [Table("D_VOTE")]
    public class SxVote : SxDbModel<int>
    {
        public virtual SxMaterial Material { get; set; }
        public ModelCoreType ModelCoreType { get; set; }
        public int MaterialId { get; set; }

        public virtual SxAppUser User { get; set; }
        [MaxLength(128)]
        public string UserId { get; set; }

        public byte IsUp { get; set; }
    }
}
