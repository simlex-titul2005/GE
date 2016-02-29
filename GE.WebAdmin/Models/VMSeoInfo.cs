using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GE.WebAdmin.Models
{
    public sealed class VMSeoInfo
    {
        public int Id { get; set; }
        public string RawUrl { get; set; }
        public string SeoTitle { get; set; }
        public string SeoDescription { get; set; }
    }
}