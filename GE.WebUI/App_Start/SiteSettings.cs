using GE.WebCoreExtantions.Repositories;
using SX.WebCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace GE.WebUI
{
    public static class SiteSettings
    {
        private static MemoryCache _cache;
        static SiteSettings()
        {
            _cache = new MemoryCache("SITE_SETTINGS");
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

        public static object Get(string keySetting)
        {
            var setting = _cache.Get(keySetting);
            if (setting != null) return setting;

            var repo = new RepoSiteSetting();
            var value = repo.GetByKey(keySetting);
            _cache.Add(new CacheItem(keySetting, value.Value), _defaultPolicy);
            return value.Value;
        }
    }
}
