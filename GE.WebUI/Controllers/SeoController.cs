using System.Configuration;
using System.Data.SqlClient;
using System.Text;
using System.Web.Mvc;
using System.Xml;
using Dapper;
using System.Linq;
using static SX.WebCore.Enums;
using System;
using SX.WebCore;
using System.Xml.Linq;

namespace GE.WebUI.Controllers
{
    public partial class SeoController : BaseController
    {
        [OutputCache(Duration = 900)]
        public virtual ContentResult Robotstxt()
        {
            var fileContent = SiteSettings.Get(SX.WebCore.Resources.Settings.robotsFileSetting);
            if (fileContent != null)
                return Content(fileContent.Value, "text/plain", Encoding.UTF8);
            else return null;
        }

        [OutputCache(Duration = 3600)]
        public virtual ContentResult Sitemap()
        {
            XNamespace ns = "http://www.sitemaps.org/schemas/sitemap/0.9";
            var root = new XElement(ns + "urlset", new XAttribute("xmlns", ns.NamespaceName));

            var query = @"SELECT
	dm.TitleUrl,
	dm.DateCreate,
	dm.DateUpdate,
	dm.ModelCoreType
FROM DV_MATERIAL AS dm
ORDER BY dm.DateUpdate DESC";
            
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DbContext"].ConnectionString))
            {
                var data = connection.Query<dynamic>(query)
                    .Select(x => new SxSiteMapUrl(getSiteMapLoc(Url, x)) {
                        LasMod=x.DateCreate
                    }).ToArray();

                for (int i = 0; i < data.Length; i++)
                {
                    var url = data[i];
                    root.Add(new XElement(ns+"url", 
                        new XElement(ns + "loc", url.Loc),
                        new XElement(ns + "lastmod", url.LasMod.ToString("yyyy-MM-dd"))
                        ));
                }
            }

            XDocument xml = new XDocument(new XDeclaration("1.0", "utf-8", null), root);
            return Content(xml.ToString(), "text/xml");
        }

        private static string getSiteMapLoc(UrlHelper helper, dynamic model)
        {
            var au = helper.RequestContext.HttpContext.Request.Url.AbsoluteUri;
            var vu = helper.RequestContext.HttpContext.Request.RawUrl;
            var hu = au.Substring(0, au.Length - vu.Length);
            var mct = (ModelCoreType)model.ModelCoreType;
            switch(mct)
            {
                case ModelCoreType.Article:
                    return hu + helper.Action(MVC.Articles.Details(
                        (int)model.DateCreate.Year,
                        (string)model.DateCreate.Month.ToString("00"),
                        (string)model.DateCreate.Day.ToString("00"),
                        (string)model.TitleUrl
                        ));
                case ModelCoreType.News:
                    return hu + helper.Action(MVC.News.Details(
                        (int)model.DateCreate.Year,
                        (string)model.DateCreate.Month.ToString("00"),
                        (string)model.DateCreate.Day.ToString("00"),
                        (string)model.TitleUrl
                        ));
                default: return null;
            }
        }
    }
}