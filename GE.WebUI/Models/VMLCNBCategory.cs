namespace GE.WebUI.Models
{
    public sealed class VMLCNBCategory
    {
        public VMLCNBCategory()
        {
            News = new VMLCNBNews[0];
        }

        public string Id { get; set; }
        public string Title { get; set; }

        public VMLCNBNews[] News { get; set; }
    }
}