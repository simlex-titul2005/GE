/// <reference path="../jquery.d.ts" />
window.onload = () => {
    var share: SxShare = new SxShare();
    // share.FillCount();
    share.Init();
};

enum SxShareNetType {
    vk,// vkontakte
    ok,// odnoklassniki
    fb,// facebook
    lj,// liveJournal
    tw,// twitter
    gp,// google Plus
    mr,// my@Mail.Ru
    li,// linkedIn
    tm,// tumblr
    bl,// blogger
    pt,// pinterest
    en,// evernote
    di,// digg
    rd,// reddit
    de,// delicious
    su,// stumbleUpon
    po,// pocket
    sb,// surfingbird
    lr,// liveInternet,
    bf,// buffer
    ip,// instapaper
    ra,// readability
    xi,// xing
    wp,// wordPress
    bd,// baidu
    rr,// renren
    wb,// weibo
    tg,// telegram
    vi,// viber
    wa,// whatsApp
    ln// lINE
}

class SxCounterResult {
    dataType: SxShareNetType;
    url: string;
    count: string;

    constructor(dataType: SxShareNetType, url: string) {
        this.dataType = dataType;
        this.url = url;
    }
}

class SxShare {
    elements: JQuery;
    counters: JQuery;
    pageUrl: string;
    pageImage: string;
    pageTitle: string;
    pageDesc: string;
    counterResults: SxCounterResult[];

    constructor() {
        this.elements = $(".goodshare[data-type]");

        this.counters = $(".goodshare [data-counter]");
        this.counterResults = new Array<SxCounterResult>();

        this.pageUrl = location.href;
        this.pageImage = $("meta[property=\"og:image\"]").attr("content") || $("link[rel=\"shortcut icon\"]").attr("href") || "";
        this.pageTitle = document.title;
        this.pageDesc = $("meta[property=\"og:description\"]").attr("content") || $("meta[name =\"description\"]").attr("content") || "";
    }

    Init(): void {
        this.elements.on("click", (element: JQueryEventObject) => this.Popup(element));
    }

    FillCount(): void {
        this.counters.each((index: number, elem: Element) => this.GetCount(index, elem));
    }

    private Popup(element: JQueryEventObject): void {
        var target: Element = element.target.parentElement;
        var url: string = this.GetUrl(target);
        if (url === null) { return; }
        window.open(url, "", "left=" + (screen.width - 630) / 2
            + ",top=" + (screen.height - 440) / 2
            + ",toolbar=0,status=0,scrollbars=0,menubar=0,location=0,width=630,height=440");
    }

    private GetUrl(target: Element): string {
        var dataType: SxShareNetType = SxShareNetType[target.getAttribute("data-type")];
        var dataUrl: string = target.getAttribute("data-url") || this.pageUrl;
        var dataImage: string = target.getAttribute("data-image") || this.pageImage;
        var dataTitle: string = target.getAttribute("data-title") || this.pageTitle;
        var dataDesc: string = target.getAttribute("data-desc") || this.pageDesc;

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
    }

    private GetCount(index: number, elem: Element): void {
        var dataType: SxShareNetType = SxShareNetType[elem.getAttribute("data-counter")];
        var dataUrl: string = encodeURIComponent($(elem).closest(".goodshare").attr("data-url") || this.pageUrl);
        var exist: SxCounterResult = this.counterResults.some((x: SxCounterResult) => x.url === dataUrl && x.dataType === dataType)[0];
        if (!exist) {
            var counter: SxCounterResult = new SxCounterResult(dataType, dataUrl);
            counter.count = this.GetAjaxCount(counter);
            $(elem).html(counter.count);
            this.counterResults.push(counter);
            return;
        }

        $(elem).html(exist.count);
    }

    private GetAjaxCount(counter: SxCounterResult): string {
        var number: any;
        var result: string;

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
    }
    private GetAjaxCount_fb(url: string): any {
        $.getJSON("https://graph.facebook.com/?id=" + url + "&callback=?",
            (data: any, textStatus: string, jqXHR: JQueryXHR) => {
                if (data.share === undefined) {
                    return 0;
                }
                return data.share.share_count;
            });
    }
    private GetAjaxCount_gp(url: string): any {
        var number: any = 0;

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
                    groupId:"@self"
                },
                jsonrpc: "2.0",
                key: "p",
                apiVersion:"v1"
            }),
            success: (data: any, textStatus: string, jqXHR: JQueryXHR): void => {
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
    }
    private GetAjaxCount_vk(url: string): any {
        $.getJSON("https://vk.com/share.php?act=count&index=1&url=" + url + "&callback=?",
            (data: any, textStatus: string, jqXHR: JQueryXHR) => {
                console.log(textStatus);
                console.log(data);
            });
    }
}