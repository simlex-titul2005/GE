using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GE.WebAdmin.Models
{
    public sealed class VMSeoKeywordList
    {
        public VMSeoKeywordList()
        {
            Keywords = new VMSeoKeyword[0];
        }

        public int SeoInfoId { get; set; }
        public VMSeoKeyword[] Keywords { get; set; }
    }
}