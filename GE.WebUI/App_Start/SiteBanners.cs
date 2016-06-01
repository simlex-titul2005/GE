using GE.WebCoreExtantions;
using GE.WebCoreExtantions.Repositories;
using SX.WebCore;
using SX.WebCore.Repositories;
using System;
using System.Linq;
using System.Runtime.Caching;

namespace GE.WebUI
{
    public static class SiteBanners
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

        public static SxBannerCollection Collection
        {
            get
            {
                var cacheBanners = (SxBannerCollection)MvcApplication.AppCache["CACHE_SITE_BANNERS"];
                if (cacheBanners == null)
                {
                    cacheBanners = new SxBannerCollection();
                    cacheBanners.Banners = new RepoBanner<DbContext>().All.ToArray();
                    cacheBanners.BannerGroups = new RepoBannerGroup().All.ToArray();

                    MvcApplication.AppCache.Add("CACHE_SITE_BANNERS", cacheBanners, _defaultPolicy);
                }

                return cacheBanners;
            }
        }
    }
}
