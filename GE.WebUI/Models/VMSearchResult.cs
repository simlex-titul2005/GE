using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GE.WebUI.Models
{
    public sealed class VMSearchResult
    {
        public string TitleUrl { get; set; }
        public SX.WebCore.Enums.ModelCoreType ModelCoreType { get; set; }
        public DateTime DateCreate { get; set; }
        public string Title { get; set; }
        public string Foreword { get; set; }
    }
}