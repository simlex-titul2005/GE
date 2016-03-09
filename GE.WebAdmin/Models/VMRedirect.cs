using SX.WebCore.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GE.WebAdmin.Models
{
    public sealed class VMRedirect : ISxViewModel<Guid>
    {
        public Guid Id { get; set; }
        public string OldUrl { get; set; }
        public string NewUrl { get; set; }
    }
}