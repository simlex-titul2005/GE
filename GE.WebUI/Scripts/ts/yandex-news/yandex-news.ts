/// <reference path="../typings/jquery.d.ts" />

class YandexNews {
    private _data: YandexRssNews[];
    private _update_time_t: any;
    private _block: JQuery;

    constructor(rubric: string, block: any) {
        this._block = $(block);
        var script: any = $.getScript("//news.yandex.ru/ru/games5.utf8.js").done(() => {
            this._data = <YandexRssNews[]>window[rubric];
            this._update_time_t = window["update_time_t"];
            this.loadNews();
        });
    }

    private loadNews = () => {
        if (!this._data || !this._data.length) { return; }
        $("<div class=\"ya-news__title\"><a href=\"//news.yandex.ru/\" target=\"_blank\">Яндекс.Новости</a></div>").appendTo(this._block);

        var ul: JQuery = $("<div></div>").addClass("yandex-news__list");
        for (var i = 0; i < this._data.length; i++) {
            var item = this._data[i];
            $("<li class=\"yandex-news__item\"><span class=\"ya-news__date\">"
                + item.date + "&nbsp;" + item.time + "</span><span class=\"ya-news__title\"><i class=\"fa fa-link\" aria-hiddent=\"true\" style=\"margin-right:15px; font-size:11px; top:-2px; position:relative\"></i><a href=\""
                + item.url + "\" target=\"_blank\">" + item.title + "</a></span><div class=\"ya-news__description\">"
                + item.descr + "</div></li>").appendTo(ul);
        }
        ul.appendTo(this._block);

        $("<div class=\"ya-news__all\"><a href=\"//news.yandex.ru/\" target=\"_blank\">Все новости на "
            + this.formatDate(this._update_time_t) + "</a></div>").appendTo(this._block);
    };

    private formatDate = (ts: any) => {
        var d = new Date(ts * 1000);
        return d.getHours() + ':' + ('0' + d.getMinutes()).substr(-2);
    };
}

class YandexRssNews {
    time: string;
    date: string;
    ts: number;
    url: string;
    title: string;
    descr: string;
}