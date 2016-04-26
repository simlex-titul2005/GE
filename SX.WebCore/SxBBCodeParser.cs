using System.Text.RegularExpressions;

namespace SX.WebCore
{
    public static class SxBBCodeParser
    {
        public static string GetHtml(string inputHtml)
        {
            //currency
            Regex re = new Regex(@"\[USD\](.*?)\[\/USD\]");
            var res = re.Replace(inputHtml, "<a href=\"javascript:void(0)\" data-currency-cc=\"USD\">$1 <i class=\"fa fa-usd\"></i></a>");
            re = new Regex(@"\[EUR\](.*?)\[\/EUR\]");
            res = re.Replace(res, "<a href=\"javascript:void(0)\" data-currency-cc=\"EUR\">$1 <i class=\"fa fa-eur\"></i></a>");

            return res;
        }
    }
}
