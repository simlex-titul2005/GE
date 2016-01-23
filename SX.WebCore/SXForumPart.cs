using SX.WebCore.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SX.WebCore
{
    [Table("D_FORUM_PART")]
    public class SXForumPart : SxDbUpdatedModel<int>, ISxHasHtml
    {
        [Column("TITLE"), MaxLength(255), Required]
        public string Title { get; set; }

        [Column("HTML"), Required]
        public string Html { get; set; }
    }
}
