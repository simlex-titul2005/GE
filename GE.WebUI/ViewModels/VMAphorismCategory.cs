namespace GE.WebUI.ViewModels
{
    public sealed class VMAphorismCategory
    {
        public VMAphorismCategory()
        {
            Authors = new VMAphorismCategory[0];
        }

        public string Id { get; set; }
        public string Title { get; set; }
        public VMAphorismCategory[] Authors { get; set; }
        public bool IsCurrent { get; set; }
    }
}