using GE.WebCoreExtantions;
using SX.WebCore.Repositories;
using System;
using System.Runtime.Caching;

namespace GE.WebUI
{
    public static class BannedUrl
    {
        private static readonly string _cacheBannedUrlListKey = "CACHE_BANNED_URL_LIST";
        private static MemoryCache _cache;
        static BannedUrl()
        {
            if (_cache == null)
                _cache = new MemoryCache("CACHE_SITE_BANNED_URLS");
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

        public static string[] Collection
        {
            get
            {
                var _list = _cache.Get(_cacheBannedUrlListKey);
                if (_list == null)
                {
                    var repo = new RepoBannedUrl<DbContext>();
                    var data = repo.GetAllUrls();
                    _cache.Add(new CacheItem(_cacheBannedUrlListKey, data), _defaultPolicy);
                    return data;
                }
                else
                    return (string[])_list;
            }
        }
    }
}