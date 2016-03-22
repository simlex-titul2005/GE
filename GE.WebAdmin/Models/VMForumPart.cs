using SX.WebCore.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GE.WebAdmin.Models
{
    public sealed class VMForumPart : ISxViewModel<int>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Html { get; set; }
    }
}