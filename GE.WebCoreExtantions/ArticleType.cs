using SX.WebCore.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GE.WebCoreExtantions
{
    [Table("D_ARTICLE_TYPE")]
    public class ArticleType : SxDbModel<byte>
    {
        [Column("NAME")]
        public string Name { get; set; }

        public virtual Game Game { get; set; }
        [Column("GAME_ID")]
        public int GameId { get; set; }
    }
}
