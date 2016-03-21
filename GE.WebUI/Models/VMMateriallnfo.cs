using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GE.WebUI.Models
{
    public sealed class VMMateriallnfo
    {
        public DateTime? DateCreate { get; set; }
        public int? ViewsCount { get; set; }
        public int? CommentsCount { get; set; }
        public int VoteUpCount { get; set; }
        public int VoteDownCount { get; set; }
    }
}