using System;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;

namespace SX.WebCore
{
    public static class SxBBCodeParser
    {
        public static string GetHtml(string inputHtml)
        {
            //currency
            Regex re = new Regex(@"\[USD\](.*?)\[\/USD\]");
            var res = re.Replace(inputHtml, "<a title=\"Узнать курс в рублях\" class=\"currency\" href=\"javascript:void(0)\" data-value=\"$1\" data-currency-cc=\"USD\">$1 <i class=\"fa fa-usd\"></i></a>");
            re = new Regex(@"\[EUR\](.*?)\[\/EUR\]");
            res = re.Replace(res, "<a title=\"Узнать курс в рублях\" class=\"currency\" href=\"javascript:void(0)\" data-value=\"$1\" data-currency-cc=\"EUR\">$1 <i class=\"fa fa-eur\"></i></a>");

            return res;
        }

        public static string ReplaceBanners(string inputHtml, SxBanner[] banners, Func<SxBanner, string> template)
        {
            Regex re = new Regex(@"\[BANNER\](.*?)\[\/BANNER\]");
            var matches = Regex.Matches(inputHtml, re.ToString());
            var list = new List<Guid>();
            foreach (Match match in matches)
            {
                var id = Guid.Parse(match.Groups[1].Value);
                list.Add(id);
            }

            var ban = banners.Where(x => list.Contains(x.Id)).ToArray();
            for (int i = 0; i < ban.Length; i++)
            {
                var banner = ban[i];
                inputHtml=inputHtml.Replace(string.Format("[BANNER]{0}[/BANNER]", banner.Id), template(banner));
            }

            return inputHtml;
        }
    }
}
