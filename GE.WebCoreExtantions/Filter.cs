using SX.WebCore.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
