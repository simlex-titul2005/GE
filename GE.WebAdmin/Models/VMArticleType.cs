namespace GE.WebAdmin.Models
{
    public sealed class VMArticleType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int GameId { get; set; }
        public string GameTitle { get; set; }
    }
}