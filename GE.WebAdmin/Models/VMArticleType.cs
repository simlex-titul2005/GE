using GE.WebCoreExtantions;
using SX.WebCore.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GE.WebAdmin.Models
{
    public sealed class VMArticleType : ISxViewModel<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string GameTitle { get; set; }
    }
}