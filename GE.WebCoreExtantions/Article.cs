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
        [Column("GAME_ID")]
        public int? GameId { get; set; }

        public virtual ArticleType ArticleType { get; set; }
        [Column("ARTICLE_TYPE_NAME")]
        public string ArticleTypeName { get; set; }
        [Column("ARTICLE_TYPE_GAME_ID")]
        public int? ArticleTypeGameId { get; set; } 
    }
}
