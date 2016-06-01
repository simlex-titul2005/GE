using GE.WebCoreExtantions;
using SX.WebCore;
using SX.WebCore.Repositories;
using SX.WebCore.Resources;
using System;
using System.Collections.Generic;
using System.Runtime.Caching;

namespace GE.WebUI
{
    public static class SiteSettings
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

        private static Dictionary<string, SxSiteSetting> getSettings()
        {
            var data = new RepoSiteSetting<DbContext>().GetByKeys(
                    Settings.siteName,
                    Settings.siteLogoPath,
                    Settings.siteBgPath,
                    Settings.emptyGameIconPath,
                    Settings.emptyGameGoodImagePath,
                    Settings.emptyGameBadImagePath
                );

            return data;
        }

        public static SxSiteSetting Get(string keySetting)
        {
            var settings = (Dictionary<string, SxSiteSetting>)MvcApplication.AppCache["CACHE_SITE_SETTINGS"];
            if (settings == null)
            {
                settings = getSettings();
                MvcApplication.AppCache.Add("CACHE_SITE_SETTINGS", settings, _defaultPolicy);
            }

            return settings.ContainsKey(keySetting) ? settings[keySetting] : null;
        }
    }
}
