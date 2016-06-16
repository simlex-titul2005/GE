using SX.WebCore;

namespace GE.WebUI.Infrastructure
{
    public static class SiteSettings
    {
        public static SxSiteSetting Get(string key)
        {
            return MvcApplication.SiteSettingsProvider.Get(key);
        }
    }
}