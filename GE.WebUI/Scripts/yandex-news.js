/// <reference path="../typings/jquery.d.ts" />
var YandexNews = (function () {
    function YandexNews(rubric, block) {
        var _this = this;
        this.loadNews = function () {
            if (!_this._data || !_this._data.length) {
                return;
            }
            $("<div class=\"ya-news__title\"><a href=\"//news.yandex.ru/\" target=\"_blank\">Яндекс.Новости</a></div>").appendTo(_this._block);
            var ul = $("<div></div>").addClass("yandex-news__list");
            for (var i = 0; i < _this._data.length; i++) {
                var item = _this._data[i];
                $("<li class=\"yandex-news__item\"><span class=\"ya-news__date\">"
                    + item.date + "&nbsp;" + item.time + "</span><span class=\"ya-news__title\"><i class=\"fa fa-link\" aria-hiddent=\"true\" style=\"margin-right:15px; font-size:11px; top:-2px; position:relative\"></i><a href=\""
                    + item.url + "\" target=\"_blank\">" + item.title + "</a></span><div class=\"ya-news__description\">"
                    + item.descr + "</div></li>").appendTo(ul);
            }
            ul.appendTo(_this._block);
            $("<div class=\"ya-news__all\"><a href=\"//news.yandex.ru/\" target=\"_blank\">Все новости на "
                + _this.formatDate(_this._update_time_t) + "</a></div>").appendTo(_this._block);
        };
        this.formatDate = function (ts) {
            var d = new Date(ts * 1000);
            return d.getHours() + ':' + ('0' + d.getMinutes()).substr(-2);
        };
        this._block = $(block);
        var script = $.getScript("//news.yandex.ru/ru/games5.utf8.js").done(function () {
            _this._data = window[rubric];
            _this._update_time_t = window["update_time_t"];
            _this.loadNews();
        });
    }
    return YandexNews;
}());
var YandexRssNews = (function () {
    function YandexRssNews() {
    }
    return YandexRssNews;
}());
