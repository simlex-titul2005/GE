/// <reference path="../jquery.d.ts" />

window.onload = () => {
    var share: SxShare = new SxShare();
};

class SxShare {
    MetaOGDescription: string;
    MetaOGImage: string;
    Location: string;
    Title: string;

    constructor() {
        this.MetaOGDescription = $("meta[property=\"og:description\"]").attr("content") || "";
        this.MetaOGImage = $("meta[property=\"og:image\"]").attr("ontent") || "";
        this.Location = location.href;
        this.Title = document.title;

        $(".goodshare[data-type]").on("click", (event: JQueryEventObject) => this.Popup(event));
        $("[data-counter]").each((index: number, counter: Element) => {
            this.GetCounter(counter);
        });
    }

    Popup(element: JQueryEventObject): void {
        let url: string = this.GetUrl(element.target.parentElement);
        if (url === "#") { return; };

        window.open(url,
            "",
            "left=" + (screen.width - 630) / 2 +
            ",top=" + (screen.height - 440) / 2 +
            ",toolbar=0,status=0,scrollbars=0,menubar=0,location=0,width=630,height=440"
        );
    }

    GetUrl(element: Element): string {
        let dataType: string = $(element).attr("data-type");
        let shareNetType: SxShareNetType = SxShareNetType[dataType];
        let description: string = $(element).attr("data-description") || this.MetaOGDescription;
        let image: string = $(element).attr("data-img-src") || this.MetaOGImage;
        let url: string = $(element).attr("data-url") || this.Location;
        let title: string = $(element).attr("data-title") || this.Title;

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
    }

    GetCounter(counter: Element): void {
        let dataType: SxShareNetType = SxShareNetType[$(counter).attr("data-counter")];
        let url: string = counter.getAttribute("data-url") || this.Location;

        switch (dataType) {
            default: return;

            case SxShareNetType.vk:
                $.getJSON("https://vk.com/share.php?act=count&index=1&url=" + encodeURIComponent(url) + "&callback=?",
                    function (response: any): void {
                        console.log(response);
                    });
                if (!window.VK) VK = {};
                VK.Share = {
                    count: function (index, count) {
                        $('[data-counter="vk"]').text(roundCount(count));
                    }
                };
        }
    }
}

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