using System.Collections.Generic;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.Abstract
{
    public abstract class SxFilter
    {
        public int? SkipCount { get; set; }
        public int? PageSize { get; set; }
        public dynamic WhereExpressionObject { get; set; }
        public IDictionary<string, SortDirection> Orders { get; set; }
    }
}
