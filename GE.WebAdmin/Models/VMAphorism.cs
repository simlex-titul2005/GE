using System;

namespace GE.WebAdmin.Models
{
    public sealed class VMAphorism
    {
        public int Id { get; set; }
        public DateTime DateCreate { get; set; }
        public string Author { get; set; }
        public string Html { get; set; }
        public string Category { get; set; }
    }
}