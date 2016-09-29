namespace GE.WebUI.ViewModels
{
    public sealed class VMSeoPhrase
    {
        public VMSeoPhrase(string phrase)
        {
            Text = phrase;
        }

        public string Text { get; set; }
        public int WordCount { get; set; }
    }
}