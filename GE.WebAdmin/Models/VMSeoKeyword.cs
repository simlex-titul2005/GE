using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GE.WebAdmin.Models
{
    public sealed class VMSeoKeyword
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public int SeoInfoId { get; set; }
    }
}