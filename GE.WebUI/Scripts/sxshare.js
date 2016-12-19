/// <reference path="../typings/jquery.d.ts" />
window.onload = function () {
    var share = new SxShare();
    // share.FillCount();
    share.Init();
};
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
    SxShareNetType[SxShareNetType["ln"] = 30] = "ln"; // lINE
})(SxShareNetType || (SxShareNetType = {}));
var SxCounterResult = (function () {
    function SxCounterResult(dataType, url) {
        this.dataType = dataType;
        this.url = url;
    }
    return SxCounterResult;
}());
var SxShare = (function () {
    function SxShare() {
        this.elements = $(".goodshare[data-type]");
        this.counters = $(".goodshare [data-counter]");
        this.counterResults = new Array();
        this.pageUrl = location.href;
        this.pageImage = $("meta[property=\"og:image\"]").attr("content") || $("link[rel=\"shortcut icon\"]").attr("href") || "";
        this.pageTitle = document.title;
        this.pageDesc = $("meta[property=\"og:description\"]").attr("content") || $("meta[name =\"description\"]").attr("content") || "";
    }
    SxShare.prototype.Init = function () {
        var _this = this;
        this.elements.on("click", function (element) { return _this.Popup(element); });
    };
    SxShare.prototype.FillCount = function () {
        var _this = this;
        this.counters.each(function (index, elem) { return _this.GetCount(index, elem); });
    };
    SxShare.prototype.Popup = function (e) {
        var target = e.target.parentElement;
        var url = this.GetUrl(target);
        if (url === null) {
            return;
        }
        window.open(url, "", "left=" + (screen.width - 630) / 2
            + ",top=" + (screen.height - 440) / 2
            + ",toolbar=0,status=0,scrollbars=0,menubar=0,location=0,width=630,height=440");
        e.preventDefault();
        e.stopPropagation();
    };
    SxShare.prototype.GetUrl = function (target) {
        var dataType = SxShareNetType[target.getAttribute("data-type")];
        var dataUrl = target.getAttribute("data-url") || this.pageUrl;
        var dataImage = target.getAttribute("data-image") || this.pageImage;
        var dataTitle = target.getAttribute("data-title") || this.pageTitle;
        var dataDesc = target.getAttribute("data-desc") || this.pageDesc;
        switch (dataType) {
            default: return null;
            // vk
            case SxShareNetType.vk:
                return "http://vk.com/share.php?"
                    + "url=" + encodeURIComponent(dataUrl)
                    + "&title=" + encodeURIComponent(dataTitle)
                    + "&description=" + encodeURIComponent(dataDesc)
                    + "&image=" + encodeURIComponent(dataImage);
            // fb
            case SxShareNetType.fb:
                return "https://facebook.com/sharer/sharer.php?"
                    + "u=" + encodeURIComponent(dataUrl);
            // ok
            case SxShareNetType.ok:
                return "http://www.odnoklassniki.ru/dk?st.cmd=addShare&st.s=1"
                    + "&st.comments=" + encodeURIComponent(dataTitle)
                    + "&st._surl=" + encodeURIComponent(dataUrl);
            // gp
            case SxShareNetType.gp:
                return "https://plus.google.com/share?"
                    + "url=" + encodeURIComponent(dataUrl);
            // tw
            case SxShareNetType.tw:
                return "http://twitter.com/share?"
                    + "url=" + encodeURIComponent(dataUrl)
                    + "&text=" + encodeURIComponent(dataTitle);
        }
    };
    SxShare.prototype.GetCount = function (index, elem) {
        var dataType = SxShareNetType[elem.getAttribute("data-counter")];
        var dataUrl = encodeURIComponent($(elem).closest(".goodshare").attr("data-url") || this.pageUrl);
        var exist = this.counterResults.some(function (x) { return x.url === dataUrl && x.dataType === dataType; })[0];
        if (!exist) {
            var counter = new SxCounterResult(dataType, dataUrl);
            counter.count = this.GetAjaxCount(counter);
            $(elem).html(counter.count);
            this.counterResults.push(counter);
            return;
        }
        $(elem).html(exist.count);
    };
    SxShare.prototype.GetAjaxCount = function (counter) {
        var number;
        var result;
        switch (counter.dataType) {
            default: number = 0;
            // vk
            case SxShareNetType.vk:
                number = this.GetAjaxCount_vk(counter.url);
                break;
            // fb
            case SxShareNetType.fb:
                number = this.GetAjaxCount_fb(counter.url);
                break;
            // ok
            case SxShareNetType.ok:
                number = 3;
                break;
            // gp
            case SxShareNetType.gp:
                number = this.GetAjaxCount_gp(counter.url);
                break;
        }
        if (number > 999 && number <= 999999) {
            result = (number / 1000) + "k";
            return result;
        }
        if (number > 999999) {
            result = ">1M";
            return result;
        }
        result = number === undefined ? "0" : number.toString();
        return result;
    };
    SxShare.prototype.GetAjaxCount_fb = function (url) {
        $.getJSON("https://graph.facebook.com/?id=" + url + "&callback=?", function (data, textStatus, jqXHR) {
            if (data.share === undefined) {
                return 0;
            }
            return data.share.share_count;
        });
    };
    SxShare.prototype.GetAjaxCount_gp = function (url) {
        var number = 0;
        $.ajax({
            type: "POST",
            url: "https://clients6.google.com/rpc",
            processData: true,
            contentType: "application/json",
            data: JSON.stringify({
                method: "pos.plusones.get",
                id: url,
                params: {
                    nolog: true,
                    id: url,
                    source: "widget",
                    userId: "@viewer",
                    groupId: "@self"
                },
                jsonrpc: "2.0",
                key: "p",
                apiVersion: "v1"
            }),
            success: function (data, textStatus, jqXHR) {
                if (data !== undefined
                    && data.result !== undefined
                    && data.result.metadata !== undefined
                    && data.result.metadata.globalCounts !== undefined
                    && data.result.metadata.globalCounts.count !== undefined) {
                    number = data.result.metadata.globalCounts.count;
                }
            }
        });
        return number;
    };
    SxShare.prototype.GetAjaxCount_vk = function (url) {
        $.getJSON("https://vk.com/share.php?act=count&index=1&url=" + url + "&callback=?", function (data, textStatus, jqXHR) {
            console.log(textStatus);
            console.log(data);
        });
    };
    return SxShare;
}());
