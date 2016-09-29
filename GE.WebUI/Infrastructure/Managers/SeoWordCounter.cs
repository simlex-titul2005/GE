using GE.WebUI.ViewModels;

namespace GE.WebUI.Infrastructure.Managers
{
    public class SeoWordCounter
    {
        public int GetWordCount(VMSeoPhrase phrases)
        {
            string[] res = phrases.Text.Trim().Split(' ');
            return res.Length;
        }
    }
}