using SX.WebCore.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SX.WebCore
{
    [Table("D_MANUAL_GROUP")]
    public class SxManualGroup : SxDbModel<string>
    {
        [Required, MaxLength(100)]
        public string Title { get; set; }

        public virtual SxManualGroup ParentGroup { get; set; }
        public string ParentGroupId { get; set; }
    }
}
