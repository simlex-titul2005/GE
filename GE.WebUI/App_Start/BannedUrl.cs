using GE.WebCoreExtantions;
using SX.WebCore.Repositories;
using System;
using System.Runtime.Caching;

namespace GE.WebUI
{
    public static class BannedUrl
    {
        private static CacheItemPolicy _defaultPolicy
        {
            get
            {
                return new CacheItemPolicy
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(15)
                };
            }
        }

        public static string[] Collection
        {
            get
            {
                var list = (string[])MvcApplication.AppCache["CACHE_SITE_BANNED_URL"];
                if (list == null)
                {
                    list = new RepoBannedUrl<DbContext>().GetAllUrls();
                    MvcApplication.AppCache.Add("CACHE_SITE_BANNED_URL", list, _defaultPolicy);
                }

                return list;
            }
        }
    }
}