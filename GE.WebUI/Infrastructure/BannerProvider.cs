using SX.WebCore.Providers;

namespace GE.WebUI.Infrastructure
{
    public static class BannerProvider
    {
        private static SxBannerProvider _provider;

        static BannerProvider()
        {
            _provider = new SxBannerProvider(() => SiteBanners.Collection.Banners);
        }

        public static SxBannerProvider Provider
        {
            get
            {
                return _provider ;
            }
        }
    }
}