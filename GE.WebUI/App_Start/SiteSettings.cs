using GE.WebCoreExtantions.Repositories;
using SX.WebCore;
using System;
using System.Runtime.Caching;

namespace GE.WebUI
{
    public static class SiteSettings
    {
        private static MemoryCache _cache;
        static SiteSettings()
        {
            if(_cache==null)
                _cache = new MemoryCache("CACHE_SITE_SETTINGS");
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

        public static SxSiteSetting Get(string keySetting)
        {
            var setting = _cache.Get(keySetting);
            if (setting != null) return (SxSiteSetting)setting;

            var repo = new RepoSiteSetting();
            var value = repo.GetByKey(keySetting);
            var newItem = value == null || (value != null && value.Value == null) ? new SxSiteSetting { Id = keySetting, Value = null } : value;
            _cache.Add(new CacheItem(keySetting, newItem), _defaultPolicy);

            return (SxSiteSetting)_cache.Get(keySetting);
        }
    }
}
