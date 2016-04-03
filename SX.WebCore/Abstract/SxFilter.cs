using System.Collections.Generic;
using static SX.WebCore.HtmlHelpers.SxExtantions;

namespace SX.WebCore.Abstract
{
    public abstract class SxFilter
    {
        public SxFilter()
        {
            PagerInfo = new SxPagerInfo(1, 10);
        }

        public SxFilter(int page, int pageSize)
        {
            PagerInfo = new SxPagerInfo(page,pageSize);
        }
        public SxPagerInfo PagerInfo { get; set; }
        public string Tag { get; set; }
        public dynamic WhereExpressionObject { get; set; }
        public IDictionary<string, SortDirection> Orders { get; set; }
    }
}
