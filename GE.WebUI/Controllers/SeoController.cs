using GE.WebUI.Infrastructure;
using SX.WebCore.MvcControllers;

namespace GE.WebUI.Controllers
{
    public sealed class SeoController : SxSeoController<DbContext>
    {
//        private static ISxSiteMapProvider _smProvider;
//        public SeoController()
//        {
//            if (_smProvider == null)
//                _smProvider = SiteMapProvider.Create();
//        }

//#if !DEBUG
//        [OutputCache(Duration = 86400)]
//#endif
//        [AllowAnonymous]
//        public async Task<ContentResult> Sitemap()
//        {
//            return await Task.Run(() =>
//            {
//                SxSiteMapUrl[] data = null;
//                using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DbContext"].ConnectionString))
//                {
//                    data = connection.Query<dynamic>("dbo.get_site_map")
//                        .Select(x => new SxSiteMapUrl(getSiteMapLoc(Url, x))
//                        {
//                            LasMod = x.DateCreate
//                        }).ToArray();
//                }

//                return Content(_smProvider.GenerateSiteMap(data), "text/xml");
//            });
//        }

//        private static string getSiteMapLoc(UrlHelper helper, dynamic model)
//        {
//            var au = helper.RequestContext.HttpContext.Request.Url.AbsoluteUri;
//            var vu = helper.RequestContext.HttpContext.Request.RawUrl;
//            var hu = au.Substring(0, au.Length - vu.Length);
//            var mct = (ModelCoreType)model.ModelCoreType;
//            switch (mct)
//            {
//                case ModelCoreType.Article:
//                    return hu + helper.Action("Details", "Articles", new { year = model.DateCreate.Year, month = model.DateCreate.Month.ToString("00"), day = model.DateCreate.Day.ToString("00"), titleUrl = model.TitleUrl });
//                case ModelCoreType.News:
//                    return hu + helper.Action("Details", "News", new { year = model.DateCreate.Year, month = model.DateCreate.Month.ToString("00"), day = model.DateCreate.Day.ToString("00"), titleUrl = model.TitleUrl });
//                case ModelCoreType.Aphorism:
//                    return hu + helper.Action("Details", "Aphorisms", new { categoryId = model.CategoryId, titleUrl = model.TitleUrl });
//                default: return null;
//            }
//        }
    }
}