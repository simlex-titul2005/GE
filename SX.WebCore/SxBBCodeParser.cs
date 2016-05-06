using System;
using System.Text.RegularExpressions;
using System.Linq;
using System.Web.Mvc;

namespace SX.WebCore
{
    public static class SxBBCodeParser
    {
        public static string GetHtml(string inputHtml)
        {
            //usd
            Regex re = new Regex(@"\[USD\](.*?)\[\/USD\]");
            var res = re.Replace(inputHtml, "<a title=\"Узнать курс в рублях\" class=\"currency\" href=\"javascript:void(0)\" data-value=\"$1\" data-currency-cc=\"USD\">$1 <i class=\"fa fa-usd\"></i></a>");

            //eur
            re = new Regex(@"\[EUR\](.*?)\[\/EUR\]");
            res = re.Replace(res, "<a title=\"Узнать курс в рублях\" class=\"currency\" href=\"javascript:void(0)\" data-value=\"$1\" data-currency-cc=\"EUR\">$1 <i class=\"fa fa-eur\"></i></a>");

            return res;
        }

        public static string ReplaceBanners(
            string inputHtml,
            SxBannerCollection banners,
            Func<SxBanner, string> bannerPictureUrl,
            Func<SxBanner, Func<SxBanner, string>, string> bannerTemplate = null,
            Func<SxBannerGroup, Func<SxBanner, string>, string> bannerGroupTemplate = null
            )
        {
            var reBanner = new Regex(@"\[BANNER\](.*?)\[\/BANNER\]");
            var reBannerGroup = new Regex(@"\[BANNERG\](.*?)\[\/BANNERG\]");
            var matchesBanners = Regex.Matches(inputHtml, reBanner.ToString());
            var matchesBannerGroups = Regex.Matches(inputHtml, reBannerGroup.ToString());

            //replace banners
            foreach (Match matchBanner in matchesBanners)
            {
                var id = Guid.Parse(matchBanner.Groups[1].Value);
                var banner = banners.Banners.SingleOrDefault(x => x.Id == id);
                if (banner != null)
                    inputHtml = inputHtml.Replace(string.Format("[BANNER]{0}[/BANNER]", banner.Id), bannerTemplate != null ? bannerTemplate(banner, bannerPictureUrl) : getBannerTemplate(banner, bannerPictureUrl));
                else
                    inputHtml = inputHtml.Replace(string.Format("[BANNER]{0}[/BANNER]", id), null);
            }

            //replace banner groups
            foreach (Match matchBannerGroup in matchesBannerGroups)
            {
                var id = Guid.Parse(matchBannerGroup.Groups[1].Value);
                var bannerGroup = banners.BannerGroups.SingleOrDefault(x => x.Id == id);
                if (bannerGroup != null && bannerGroup.BannerLinks.Any())
                {
                    inputHtml = inputHtml.Replace(string.Format("[BANNERG]{0}[/BANNERG]", id), bannerGroupTemplate != null ? bannerGroupTemplate(bannerGroup, bannerPictureUrl) : getBannerGroupTemplate(bannerGroup, bannerPictureUrl));
                }
                else
                    inputHtml = inputHtml.Replace(string.Format("[BANNERG]{0}[/BANNERG]", id), null);
            }

            return inputHtml;
        }

        public static string ReplaceVideo(string inputHtml, SxVideo[] videos, Func<SxVideo, string> videoImgSrc, Func<SxVideo, string> videoTemplate=null)
        {
            var reVideo = new Regex(@"\[VIDEO\](.*?)\[\/VIDEO\]");
            var matchesVideo = Regex.Matches(inputHtml, reVideo.ToString());
            foreach (Match matchVideo in matchesVideo)
            {
                var id = Guid.Parse(matchVideo.Groups[1].Value);
                var video = videos.SingleOrDefault(x => x.Id == id);
                if(video != null)
                {
                    inputHtml = inputHtml.Replace(string.Format("[VIDEO]{0}[/VIDEO]", id), videoTemplate != null ? videoTemplate(video) : getVideoTemplate(video, videoImgSrc));
                }
                else
                {
                    inputHtml = inputHtml.Replace(string.Format("[VIDEO]{0}[/VIDEO]", id), null);
                }
            }

            return inputHtml;
        }

        private static string getBannerTemplate(SxBanner banner, Func<SxBanner, string> bannerPictureUrl)
        {
            var figure = new TagBuilder("figure");
            figure.AddCssClass("banner");

            var a = new TagBuilder("a");
            a.MergeAttribute("href", banner.Url);

            var img = new TagBuilder("img");
            img.MergeAttribute("alt", banner.Title);
            img.MergeAttribute("src", bannerPictureUrl(banner));
            a.InnerHtml += img;
            figure.InnerHtml += a;


            var figcaption = new TagBuilder("figcaption");
            figcaption.InnerHtml += banner.Title;
            figure.InnerHtml += figcaption;

            return figure.ToString();
        }

        private static string getBannerGroupTemplate(SxBannerGroup banner, Func<SxBanner, string> bannerPictureUrl)
        {
            var id = Guid.NewGuid().ToString().ToLowerInvariant();
            var bannerLinks = banner.BannerLinks.ToArray();

            var div = new TagBuilder("div");
            div.AddCssClass("carousel slide");
            div.MergeAttribute("data-ride", "carousel");
            div.MergeAttribute("id", id);

            //var ol = new TagBuilder("ol");
            //ol.AddCssClass("carousel-indicators");

            var wrapper = new TagBuilder("div");
            wrapper.AddCssClass("carousel-inner");
            wrapper.MergeAttribute("role", "listbox");
            
            for (int i = 0; i < bannerLinks.Length; i++)
            {
                var b = bannerLinks[i].Banner;

                //var li = new TagBuilder("li");
                //li.MergeAttribute("data-target", id);
                //li.MergeAttribute("data-slide-to", i.ToString());
                //if (i == 0)
                //    li.AddCssClass("active");
                //ol.InnerHtml += li;


                //slider
                var item = new TagBuilder("div");
                item.AddCssClass("item");
                if (i == 0)
                    item.AddCssClass("active");

                var a = new TagBuilder("a");
                a.MergeAttribute("href", b.Url);

                var img = new TagBuilder("img");
                img.MergeAttribute("alt", b.Title);
                img.MergeAttribute("src", bannerPictureUrl(b));
                a.InnerHtml += img;
                item.InnerHtml += a;

                var caption = new TagBuilder("div");
                caption.AddCssClass("carousel-caption");
                caption.InnerHtml += b.Title;
                item.InnerHtml += caption;

                wrapper.InnerHtml += item;
            }

            //div.InnerHtml += ol;
            div.InnerHtml += wrapper;

            return div.ToString();
        }

        private static string getVideoTemplate(SxVideo video, Func<SxVideo, string> videoImgSrc)
        {
            var div = new TagBuilder("div");
            div.AddCssClass("video");
            div.MergeAttribute("id", video.Id.ToString().ToLowerInvariant());

            var wrapper = new TagBuilder("div");
            wrapper.AddCssClass("wrapper");

            var figure = new TagBuilder("figure");

            var buttonWrapper = new TagBuilder("div");
            buttonWrapper.AddCssClass("button-wrapper");

            var buttonContainer = new TagBuilder("div");
            buttonContainer.AddCssClass("button-container");

            var buttonCell = new TagBuilder("div");
            buttonCell.AddCssClass("button-cell");

            var a = new TagBuilder("a");
            a.MergeAttribute("href", "javascript:void(0)");
            a.MergeAttribute("data-src", video.Url);
            a.MergeAttribute("onclick", "playVideo(this)");
            a.AddCssClass("button");

            var i = new TagBuilder("i");
            i.AddCssClass("fa fa-youtube-play");
            a.InnerHtml += i;

            buttonCell.InnerHtml += a;
            buttonContainer.InnerHtml += buttonCell;
            buttonWrapper.InnerHtml += buttonContainer;

            figure.InnerHtml += buttonWrapper;

            var img = new TagBuilder("img");
            img.MergeAttribute("alt", video.Title);
            img.MergeAttribute("src", videoImgSrc(video));
            figure.InnerHtml += img;

            var figcaption = new TagBuilder("figcaption");
            figcaption.InnerHtml += video.Title;
            if(video.ViewsCount!=0)
            {
                var count = new TagBuilder("div");
                count.AddCssClass("count");
                count.InnerHtml += "Просмотров: <b>" + video.ViewsCount+"</b>";
                figcaption.InnerHtml += count;
            }
            if(video.SourceUrl!=null)
            {
                var source = new TagBuilder("div");
                source.AddCssClass("source");
                var aSource = new TagBuilder("a");
                aSource.MergeAttribute("href", video.SourceUrl);
                aSource.InnerHtml += video.SourceUrl;
                source.InnerHtml += "Источник: " + aSource;
                figcaption.InnerHtml += source;
            }
            figure.InnerHtml += figcaption;

            wrapper.InnerHtml += figure;

            div.InnerHtml += wrapper;

            return div.ToString();
        }
    }
}
