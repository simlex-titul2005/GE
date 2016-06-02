using System;

namespace GE.WebAdmin.Models
{
    public sealed class VMSiteTestQuestion
    {
        public int Id { get; set; }

        public VMSiteTest Test { get; set; }
        public string TestTitle
        {
            get
            {
                return Block?.Test?.Title;
            }
            set { }
        }

        public VMSiteTestBlock Block { get; set; }
        public string BlockTitle
        {
            get
            {
                return Block?.Title;
            }
            set { }
        }

        public string Text { get; set; }

        public bool IsCorrect { get; set; }

        public DateTime DateCreate { get; set; }
    }
}