using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GE.WebUI.Models
{
    public sealed class VMPreviewInfo
    {
        public DateTime? DateCreate { get; set; }
        public int? ViewsCount { get; set; }
        public int? CommentsCount { get; set; }
    }
}