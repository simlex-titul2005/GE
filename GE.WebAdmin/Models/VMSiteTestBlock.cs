using System;

namespace GE.WebAdmin.Models
{
    public sealed class VMSiteTestBlock
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public VMSiteTest Test { get; set; }
        public int TestId { get; set; }
        public string TestTitle
        {
            get
            {
                return Test?.Title;
            }
            set { }
        }

        public DateTime DateCreate { get; set; }

        public string Description { get; set; }
    }
}