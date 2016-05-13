using SX.WebCore.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GE.WebCoreExtantions
{
    [Table("D_APHORISM")]
    public class Aphorism : SxMaterial
    {
        [MaxLength(50), Index]
        public string Author { get; set; }
    }
}
