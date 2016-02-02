using GE.WebCoreExtantions;
using SX.WebCore.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GE.WebAdmin.Models
{
    public sealed class VMEditArticle : SxMaterial, ISxViewModel<int>
    {
        public int? GameId { get; set; }
        public int? ArticleTypeId { get; set; }
    }
}