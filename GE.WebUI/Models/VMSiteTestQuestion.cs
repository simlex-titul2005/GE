using System;

namespace GE.WebUI.Models
{
    public sealed class VMSiteTestQuestion
    {
        public int Id { get; set; }
        public DateTime DateAnswer { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
    }
}