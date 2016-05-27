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
        private static readonly string _cacheBannersKey = "CACHE_BANNER_LIST";
        private static MemoryCache _cache;
        static SiteBanners()
        {
            if (_cache == null)
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

        public static SxBannerCollection Collection
        {
            get
            {
                var cacheBanners = _cache.Get(_cacheBannersKey);
                if (cacheBanners == null)
                {
                    var collection = new SxBannerCollection();
                    collection.Banners = new RepoBanner<DbContext>().All.ToArray();

                    collection.BannerGroups = new RepoBannerGroup().All.ToArray();

                    _cache.Add(new CacheItem(_cacheBannersKey, collection), _defaultPolicy);
                    return collection;
                }
                else
                {
                    return (SxBannerCollection)_cache[_cacheBannersKey];
                }
            }
        }
    }
}
