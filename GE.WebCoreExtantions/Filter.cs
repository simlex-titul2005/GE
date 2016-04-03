using SX.WebCore.Abstract;
using static SX.WebCore.Enums;

namespace GE.WebCoreExtantions
{
    public sealed class Filter : SxFilter
    {
        public Filter()
        {
            Page = 1;
        }

        public int? Year { get; set; }
        public int? Month { get; set; }
        public int? Day { get; set; }
        public string GameTitle { get; set; }
        public string TitleUrl { get; set; }
        public int Page { get; set; }
        public int? MaterialId { get; set; }
        public ModelCoreType ModelCoreType { get; set; }
        public string Tag { get; set; }
    }
}
