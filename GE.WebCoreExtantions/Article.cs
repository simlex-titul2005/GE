using GE.WebCoreExtantions.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GE.WebCoreExtantions
{
    [Table("D_ARTICLE")]
    public class Article : SX.WebCore.SxArticle, IHasGame
    {
        public virtual Game Game { get; set; }
        public int? GameId { get; set; }

        public virtual ArticleType ArticleType { get; set; }
        public string ArticleTypeName { get; set; }

        public int? ArticleTypeGameId { get; set; }
    }
}
