using GE.WebCoreExtantions.Abstract;
using SX.WebCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace GE.WebCoreExtantions
{
    [Table("D_ARTICLE")]
    public class Article : SxArticle, IHasGame
    {
        public virtual Game Game { get; set; }
        public int? GameId { get; set; }
    }
}
