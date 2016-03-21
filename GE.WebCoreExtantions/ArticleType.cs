using SX.WebCore.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GE.WebCoreExtantions
{
    [Table("D_ARTICLE_TYPE")]
    public class ArticleType : SxDbModel<int>
    {
        [MaxLength(150), Required]
        public string Name { get; set; }

        [MaxLength(255), Required]
        public string Description { get; set; }

        public virtual Game Game { get; set; }
        public int GameId { get; set; }

        [MaxLength(100)]
        public string Color { get; set; }
    }
}
