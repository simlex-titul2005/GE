using GE.WebCoreExtantions.Repositories;
using SX.WebCore;
using System;
using System.Linq;
using System.Runtime.Caching;

namespace GE.WebUI
{
    public static class SiteBanners
    {
        private static readonly string _cacheBannersKey = "CACHE_BANNER_LIST";
        private static MemoryCache _cache;
        static SiteBanners()
        {
            if(_cache==null)
                _cache = new MemoryCache("CACHE_SITE_BANNERS");
        }

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

        public static SxBanner[] Banners()
        {
            var cacheBanners = _cache.Get(_cacheBannersKey);
            if(cacheBanners==null)
            {
                var repo = new RepoBanner();
                var banners = repo.All.ToArray();
                _cache.Add(new CacheItem(_cacheBannersKey, banners), _defaultPolicy);
                return banners;
            }
            else
            {
                return (SxBanner[])_cache[_cacheBannersKey];
            }
        }
    }
}
