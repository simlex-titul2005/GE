using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;
using Dapper;
using System.Linq;
using static SX.WebCore.Enums;
using SX.WebCore;
using SX.WebCore.Abstract;
using GE.WebUI.Infrastructure;
using SX.WebCore.MvcControllers;
using GE.WebCoreExtantions;

namespace GE.WebUI.Controllers
{
    public sealed class SeoController : SxSeoController<DbContext>
    {
        private static ISxSiteMapProvider _smProvider;
        public SeoController()
        {
            if(_smProvider==null)
                _smProvider = SiteMapProvider.Create();
        }

#if !DEBUG
        [OutputCache(Duration = 3600)]
#endif
        [AllowAnonymous]
        public ContentResult Sitemap()
        {
            var query = @"SELECT dm.TitleUrl,
       dm.DateCreate,
       dm.DateUpdate,
       dm.ModelCoreType
FROM   DV_MATERIAL  AS dm
       JOIN D_NEWS  AS dn
            ON  dn.Id = dm.Id
            AND dn.ModelCoreType = dm.ModelCoreType
WHERE  dm.Show = 1
       AND dm.DateOfPublication <= GETDATE()
UNION ALL
SELECT dm.TitleUrl,
       dm.DateCreate,
       dm.DateUpdate,
       dm.ModelCoreType
FROM   DV_MATERIAL     AS dm
       JOIN D_ARTICLE  AS da
            ON  da.Id = dm.Id
            AND da.ModelCoreType = dm.ModelCoreType
WHERE  dm.Show = 1
       AND dm.DateOfPublication <= GETDATE()
ORDER BY
       dm.DateUpdate DESC";

            SxSiteMapUrl[] data = null;
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DbContext"].ConnectionString))
            {
                data = connection.Query<dynamic>(query)
                    .Select(x => new SxSiteMapUrl(getSiteMapLoc(Url, x))
                    {
                        LasMod = x.DateCreate
                    }).ToArray();
            }

            return Content(_smProvider.GenerateSiteMap(data), "text/xml");
        }

        private static string getSiteMapLoc(UrlHelper helper, dynamic model)
        {
            var au = helper.RequestContext.HttpContext.Request.Url.AbsoluteUri;
            var vu = helper.RequestContext.HttpContext.Request.RawUrl;
            var hu = au.Substring(0, au.Length - vu.Length);
            var mct = (ModelCoreType)model.ModelCoreType;
            switch (mct)
            {
                case ModelCoreType.Article:
                    return hu + helper.Action("details", new {controller= "Articles", year= model.DateCreate.Year, month= model.DateCreate.Month.ToString("00"), day= model.DateCreate.Day.ToString("00"), titleUrl= model.TitleUrl } );
                case ModelCoreType.News:
                    return hu + helper.Action("details", new { controller = "News", year = model.DateCreate.Year, month = model.DateCreate.Month.ToString("00"), day = model.DateCreate.Day.ToString("00"), titleUrl = model.TitleUrl });
                default: return null;
            }
        }
    }
}