using System;

namespace GE.WebAdmin.Models
{
    public sealed class VMMenuItem
    {
        public int Id { get; set; }
        public int MenuId { get; set; }
        public VMRoute Route { get; set; }
        public DateTime DateCreate { get; set; }
        public string Caption { get; set; }
        public string Url { get; set; }
    }
}