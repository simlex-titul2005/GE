using GE.WebCoreExtantions.Abstract;
using System.ComponentModel.DataAnnotations.Schema;

namespace GE.WebCoreExtantions
{
    [Table("D_ARTICLE")]
    public class Article : SX.WebCore.SxArticle, IHasGame
    {
        public virtual Game Game { get; set; }
        public int? GameId { get; set; }
    }
}
