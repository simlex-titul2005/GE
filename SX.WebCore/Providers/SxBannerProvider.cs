using System;
using System.Collections.Generic;
using System.Linq;

namespace SX.WebCore.Providers
{
    public class SxBannerProvider
    {
        private static Func<SxBanner[]> _banners;
        public SxBannerProvider(Func<SxBanner[]> banners)
        {
            _banners = banners;
        }

        public SxBanner GetBanner(SxBanner.BannerPlace place, string controllerName=null, string actionName=null)
        {
            SxBanner banner = null;
            if (controllerName == null && actionName == null)
                banner = getPlaceBanner(place);
            else if (controllerName != null && actionName == null)
                banner = getControllerBanner(place, controllerName);
            else if (controllerName != null && actionName != null)
                banner = getActionBanner(place, controllerName, actionName);

            return banner;
        }

        private static SxBanner getPlaceBanner(SxBanner.BannerPlace place)
        {
            var data = _banners().SingleOrDefault(x => x.Place == place && x.ControllerName == null && x.ActionName == null);
            return data;
        }

        private static SxBanner getControllerBanner(SxBanner.BannerPlace place, string controllerName)
        {
            var data = _banners().SingleOrDefault(x => x.Place == place && x.ControllerName == controllerName && x.ActionName == null);
            return data ?? getPlaceBanner(place);
        }

        private static SxBanner getActionBanner(SxBanner.BannerPlace place, string controllerName, string actionName)
        {
            var data = _banners().SingleOrDefault(x => x.Place == place && x.ControllerName == controllerName && x.ActionName == actionName);
            data = data ?? getControllerBanner(place, controllerName);
            return data ?? getPlaceBanner(place);
        }

        public SxBanner[] GetPageBanners(string controllerName, string actionName)
        {
            var list = new List<SxBanner>();
            foreach(var p in Enum.GetValues(typeof(SxBanner.BannerPlace)))
            {
                var place = (SxBanner.BannerPlace)p;
                var banner = GetBanner(place, controllerName, actionName);
                if (banner != null)
                    list.Add(banner);
            }

            return list.ToArray();
        }
    }
}
