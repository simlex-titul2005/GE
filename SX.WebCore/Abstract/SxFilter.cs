using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SX.WebCore.Abstract
{
    public abstract class SxFilter
    {
        public int? SkipCount { get; set; }
        public int? PageSize { get; set; }
        public object[] Additional { get; set; }
    }
}
