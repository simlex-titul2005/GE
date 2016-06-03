namespace GE.WebUI.Models
{
    public sealed class VMSiteTestBlock
    {
        public VMSiteTestBlock()
        {
            Questions = new VMSiteTestQuestion[0];
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public VMSiteTestQuestion[] Questions { get; set; }
    }
}