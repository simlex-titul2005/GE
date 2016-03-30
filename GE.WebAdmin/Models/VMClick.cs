using System;

namespace GE.WebAdmin.Models
{
    public sealed class VMClick
    {
        public Guid Id { get; set; }
        public DateTime DateCreate { get; set; }
        public string UrlRef { get; set; }
        public string RawUrl { get; set; }
        public string LinkTarget { get; set; }
        public string ClickTypeName { get; set; }
    }
}