using SX.WebCore.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GE.WebCoreExtantions
{
    [Table("D_APHORISM")]
    public class Aphorism : SxDbUpdatedModel<int>, ISxHasHtml
    {
        [Required, MaxLength(600)]
        public string Html { get; set; }

        [MaxLength(100), Index]
        public string Author { get; set; }

        [MaxLength(60), Index]
        public string Category { get; set; }
    }
}
