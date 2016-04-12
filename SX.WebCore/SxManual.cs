using SX.WebCore.Abstract;
using System.ComponentModel.DataAnnotations.Schema;

namespace SX.WebCore
{
    [Table("D_MANUAL")]
    public class SxManual : SxMaterial
    {
        public virtual SxManualGroup Group { get; set; }
        public string GroupId { get; set; }
    }
}
