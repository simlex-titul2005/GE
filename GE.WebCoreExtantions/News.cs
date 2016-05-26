using GE.WebCoreExtantions.Abstract;
using System.ComponentModel.DataAnnotations.Schema;

namespace GE.WebCoreExtantions
{
    [Table("D_NEWS")]
    public class News : SX.WebCore.SxNews, IHasGame
    {
        public virtual Game Game { get; set; }
        public int? GameId { get; set; }

        [NotMapped]
        public int CommentsCount { get; set; }
    }
}
