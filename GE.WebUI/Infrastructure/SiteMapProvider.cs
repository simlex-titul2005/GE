using SX.WebCore;
using SX.WebCore.Abstract;
using System.Xml.Linq;

namespace GE.WebUI.Infrastructure
{
    public class SiteMapProvider : ISxSiteMapProvider
    {
        public static SiteMapProvider Create()
        {
            return new SiteMapProvider();
        }

        public string GenerateSiteMap(SxSiteMapUrl[] data)
        {
            XNamespace ns = "http://www.sitemaps.org/schemas/sitemap/0.9";
            var root = new XElement(ns + "urlset", new XAttribute("xmlns", ns.NamespaceName));
            for (int i = 0; i < data.Length; i++)
            {
                var url = data[i];
                root.Add(new XElement(ns + "url",
                    new XElement(ns + "loc", url.Loc),
                    new XElement(ns + "lastmod", url.LasMod.ToString("yyyy-MM-dd"))
                    ));
            }
            var xml = new XDocument(new XDeclaration("1.0", "utf-8", null), root);
            return xml.ToString();
        }
    }
}