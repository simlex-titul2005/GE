using GE.WebUI.ViewModels;

namespace GE.WebUI.Infrastructure.Managers
{
    public class SeoWordCounter
    {
        public int GetWordCount(VMSeoPhrase phrases)
        {
            var res = phrases.Text.Trim().Split(' ');
            return res.Length;
        }
    }
}