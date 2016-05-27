using System;
using System.Linq;

namespace SX.WebCore.Providers
{
    public class SxBannerProvider
    {
        private SxBanner[] _banners;
        public SxBannerProvider(Func<SxBanner[]> banners)
        {
            _banners = new SxBanner[0];
            _banners = banners();
        }

        public SxBanner GetPlaceBanner(SxBanner.BannerPlace place, string controllerName = null, string actionName = null)
        {
            if (!_banners.Any()) return null;

            if (controllerName == null && actionName == null)
                return getPlaceBanner(place);
            else if (controllerName != null && actionName == null)
                return getControllerBanner(place, controllerName);
            else if (controllerName != null && actionName != null)
                return getActionBanner(place, controllerName, actionName);
            else
                return null;
        }

        private SxBanner getPlaceBanner(SxBanner.BannerPlace place)
        {
            return _banners.SingleOrDefault(x => x.Place == place);
        }

        private SxBanner getControllerBanner(SxBanner.BannerPlace place, string controllerName)
        {
            var data = _banners.SingleOrDefault(x => x.Place == place && x.ControllerName == controllerName);
            return data ?? getPlaceBanner(place);
        }

        private SxBanner getActionBanner(SxBanner.BannerPlace place, string controllerName, string actionName)
        {
            var data = _banners.SingleOrDefault(x => x.Place == place && x.ControllerName == controllerName && x.ActionName == actionName);
            return data ?? getControllerBanner(place, controllerName);
        }
    }
}
