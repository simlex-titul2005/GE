namespace GE.WebUI.Models
{
    public sealed class VMSiteTestBlock
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public int TestId { get; set; }
        public VMSiteTest Test { get; set; }
    }
}