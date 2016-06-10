using System;

namespace GE.WebAdmin.Models
{
    public sealed class VMSiteTest
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime DateCreate { get; set; }
        public string Description { get; set; }
        public SX.WebCore.SxSiteTest.SiteTestType TestType { get; set; }
    }
}