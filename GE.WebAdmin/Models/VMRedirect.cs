using System;

namespace GE.WebAdmin.Models
{
    public sealed class VMRedirect
    {
        public Guid Id { get; set; }
        public string OldUrl { get; set; }
        public string NewUrl { get; set; }
    }
}