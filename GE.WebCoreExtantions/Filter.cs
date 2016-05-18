using SX.WebCore.Abstract;
using static SX.WebCore.Enums;

namespace GE.WebCoreExtantions
{
    public sealed class Filter : SxFilter
    {
        public Filter() : base() { }

        public Filter(int page, int pageSize)
            :base(page, pageSize)
        {
            
        }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public int? Day { get; set; }
        public string GameTitle { get; set; }
        public string TitleUrl { get; set; }
    }
}
