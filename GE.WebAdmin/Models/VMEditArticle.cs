using GE.WebCoreExtantions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GE.WebAdmin.Models
{
    public sealed class VMEditArticle : SX.WebCore.Abstract.SxMaterial, SX.WebCore.Abstract.ISxViewModel
    {
        public int? GameId { get; set; }
        public int? ArticleTypeId { get; set; }
    }
}