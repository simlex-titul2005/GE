namespace GE.WebUI.ViewModels
{
    public sealed class VMSteamNews
    {
        public int SteamAppId { get; set; }

        public string Gid { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }

        public bool IsExternalUrl { get; set; }

        public string Author { get; set; }

        public string Contents { get; set; }

        public string FeedLabel { get; set; }

        public int Date { get; set; }

        public string FeedName { get; set; }
    }
}