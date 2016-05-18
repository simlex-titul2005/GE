using System;

namespace GE.WebUI.Models
{
    public sealed class VMComment
    {
        public int Id { get; set; }
        public DateTime DateCreate { get; set; }
        public string Html { get; set; }
        public VMUser User { get; set; }
    }
}