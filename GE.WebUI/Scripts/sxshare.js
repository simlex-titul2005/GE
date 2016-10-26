window.onload = function () {
    var share = new SxShare();
};
var SxShare = (function () {
    function SxShare() {
        var _this = this;
        this.MetaOGDescription = $("meta[property=\"og:description\"]").attr("content") || "";
        this.MetaOGImage = $("meta[property=\"og:image\"]").attr("ontent") || "";
        this.Location = location.href;
        this.Title = document.title;
        $(".goodshare[data-type]").on("click", function (event) { return _this.Popup(event); });
        $("[data-counter]").each(function (index, counter) {
            _this.GetCounter(counter);
        });
    }
    SxShare.prototype.Popup = function (element) {
        var url = this.GetUrl(element.target.parentElement);
        if (url === "#") {
            return;
        }
        ;
        window.open(url, "", "left=" + (screen.width - 630) / 2 +
            ",top=" + (screen.height - 440) / 2 +
            ",toolbar=0,status=0,scrollbars=0,menubar=0,location=0,width=630,height=440");
    };
    SxShare.prototype.GetUrl = function (element) {
        var dataType = $(element).attr("data-type");
        var shareNetType = SxShareNetType[dataType];
        var description = $(element).attr("data-description") || this.MetaOGDescription;
        var image = $(element).attr("data-img-src") || this.MetaOGImage;
        var url = $(element).attr("data-url") || this.Location;
        var title = $(element).attr("data-title") || this.Title;
        switch (shareNetType) {
            case SxShareNetType.vk:
                return "http://vk.com/share.php?"
                    + "url=" + encodeURIComponent(url)
                    + "&title=" + encodeURIComponent(title)
                    + "&description=" + encodeURIComponent(description)
                    + "&image=" + encodeURIComponent(image);
            case SxShareNetType.ok:
                return "http://www.odnoklassniki.ru/dk?st.cmd=addShare&st.s=1"
                    + "&st.comments=" + encodeURIComponent(title)
                    + "&st._surl=" + encodeURIComponent(url);
            case SxShareNetType.fb:
                return "https://facebook.com/sharer/sharer.php?"
                    + "u=" + encodeURIComponent(url);
            case SxShareNetType.gp:
                return "https://plus.google.com/share?"
                    + "url=" + encodeURIComponent(url);
            case SxShareNetType.tw:
                return "http://twitter.com/share?"
                    + "url=" + encodeURIComponent(url)
                    + "&text=" + encodeURIComponent(title);
            default: return "#";
        }
    };
    SxShare.prototype.GetCounter = function (counter) {
        var dataType = SxShareNetType[$(counter).attr("data-counter")];
        var url = counter.getAttribute("data-url") || this.Location;
        switch (dataType) {
            default: return;
            case SxShareNetType.vk:
                $.getJSON("https://vk.com/share.php?act=count&index=1&url=" + encodeURIComponent(url) + "&callback=?", function (response) {
                    console.log(response);
                });
                if (!window.VK)
                    VK = {};
                VK.Share = {
                    count: function (index, count) {
                        $('[data-counter="vk"]').text(roundCount(count));
                    }
                };
        }
    };
    return SxShare;
}());
var SxShareNetType;
(function (SxShareNetType) {
    SxShareNetType[SxShareNetType["vk"] = 0] = "vk";
    SxShareNetType[SxShareNetType["ok"] = 1] = "ok";
    SxShareNetType[SxShareNetType["fb"] = 2] = "fb";
    SxShareNetType[SxShareNetType["lj"] = 3] = "lj";
    SxShareNetType[SxShareNetType["tw"] = 4] = "tw";
    SxShareNetType[SxShareNetType["gp"] = 5] = "gp";
    SxShareNetType[SxShareNetType["mr"] = 6] = "mr";
    SxShareNetType[SxShareNetType["li"] = 7] = "li";
    SxShareNetType[SxShareNetType["tm"] = 8] = "tm";
    SxShareNetType[SxShareNetType["bl"] = 9] = "bl";
    SxShareNetType[SxShareNetType["pt"] = 10] = "pt";
    SxShareNetType[SxShareNetType["en"] = 11] = "en";
    SxShareNetType[SxShareNetType["di"] = 12] = "di";
    SxShareNetType[SxShareNetType["rd"] = 13] = "rd";
    SxShareNetType[SxShareNetType["de"] = 14] = "de";
    SxShareNetType[SxShareNetType["su"] = 15] = "su";
    SxShareNetType[SxShareNetType["po"] = 16] = "po";
    SxShareNetType[SxShareNetType["sb"] = 17] = "sb";
    SxShareNetType[SxShareNetType["lr"] = 18] = "lr";
    SxShareNetType[SxShareNetType["bf"] = 19] = "bf";
    SxShareNetType[SxShareNetType["ip"] = 20] = "ip";
    SxShareNetType[SxShareNetType["ra"] = 21] = "ra";
    SxShareNetType[SxShareNetType["xi"] = 22] = "xi";
    SxShareNetType[SxShareNetType["wp"] = 23] = "wp";
    SxShareNetType[SxShareNetType["bd"] = 24] = "bd";
    SxShareNetType[SxShareNetType["rr"] = 25] = "rr";
    SxShareNetType[SxShareNetType["wb"] = 26] = "wb";
    SxShareNetType[SxShareNetType["tg"] = 27] = "tg";
    SxShareNetType[SxShareNetType["vi"] = 28] = "vi";
    SxShareNetType[SxShareNetType["wa"] = 29] = "wa";
    SxShareNetType[SxShareNetType["ln"] = 30] = "ln";
})(SxShareNetType || (SxShareNetType = {}));
