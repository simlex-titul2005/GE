namespace GE.WebUI.Models
{
    public sealed class VMAphorism
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Html { get; set; }
        public string TitleUrl { get; set; }
        public VMMaterialCategory Category { get; set; }
        public string CategoryId { get; set; }
    }
}