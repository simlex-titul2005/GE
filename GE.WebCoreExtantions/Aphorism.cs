using SX.WebCore.Abstract;
using System.ComponentModel.DataAnnotations.Schema;

namespace GE.WebCoreExtantions
{
    [Table("D_APHORISM")]
    public class Aphorism : SxMaterial
    {
        public virtual AuthorAphorism Author { get; set; }
        public int? AuthorId { get; set; }
    }
}
