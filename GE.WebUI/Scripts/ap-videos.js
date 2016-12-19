/// <reference path="../typings/jquery.d.ts" />
var ApVideos = (function () {
    function ApVideos(url, cat, amount) {
        this._itemsCount = 5;
        this._colCount = 3;
        this._counter = 0;
        this._cat = cat;
        this._amount = amount;
        this._url = url;
        this._block = $(".ap-list ul");
        this._data = [];
    }
    ApVideos.prototype.initialize = function () {
        this.getVideos();
    };
    ApVideos.prototype.getVideos = function () {
        var _this = this;
        $.ajax({
            method: "get",
            url: this._url,
            data: { cat: this._cat, amount: this._amount },
            success: function (data, status, xhr) {
                _this._data = data;
                _this.fillVideos();
            }
        });
    };
    ApVideos.prototype.fillVideos = function () {
        var _this = this;
        this._block.find(".ap-list__item").each(function (i, e) {
            _this._counter++;
            if (_this._counter === _this._itemsCount) {
                _this.generateVideoRow(e);
                _this._counter = 0;
            }
        });
    };
    ApVideos.prototype.generateVideoRow = function (e) {
        var counter = this._colCount;
        var li = $("<li></li>");
        var row = $("<div class=\"row\"></div>");
        for (var _i = 0, _a = this._data; _i < _a.length; _i++) {
            var video = _a[_i];
            if (counter <= 0) {
                this.removeVideosBlock();
                break;
            }
            this.generateVideoItem(video, row);
            counter--;
        }
        li.append(row);
        li.insertAfter(e);
    };
    ApVideos.prototype.removeVideosBlock = function () {
        this._data.splice(0, this._colCount);
    };
    ApVideos.prototype.generateVideoItem = function (video, row) {
        var item = $("<div class=\"col-md-4\"><div class=\"ap-list__video\"><figure class=\"video\"" +
            "style=\"background-image:url('http://img.youtube.com/vi/"
            + video.VideoId + "/mqdefault.jpg');\" id=\"" + video.Id + "\"><div class=\"v-p-wr\">" +
            "<a href=\"" + video.VideoUrl + "\" target=\"_blank\">" +
            "<i class=\"fa fa-youtube-play\"></i></a></div></figure></div></div>");
        row.append(item);
    };
    return ApVideos;
}());
