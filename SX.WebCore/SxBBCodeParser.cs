using System.Text.RegularExpressions;

namespace SX.WebCore
{
    public static class SxBBCodeParser
    {
        public static string GetHtml(string inputHtml)
        {
            //currency
            Regex re = new Regex(@"\[usd\](.*?)\[\/usd\]");
            var res = re.Replace(inputHtml, "<a href=\"javascript:void(0)\" data-currency=\"true\" data-name=\"dollar\" onclick=\"getCurrency(this)\">$1 $</a>");
            re = new Regex(@"\[eur\](.*?)\[\/eur\]");
            res = re.Replace(res, "<a href=\"javascript:void(0)\" data-currency=\"true\" data-name=\"euro\" onclick=\"getCurrency(this)\">$1 &euro;</a>");

            return res;
        }
    }
}
