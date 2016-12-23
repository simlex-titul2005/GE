/// <reference path="../typings/jquery.d.ts" />
var Videos = (function () {
    function Videos(url, block, blockItem, itemWrapper, cat, amount, alterCount) {
        this._colCount = 3;
        this._counter = 0;
        this._cat = cat;
        this._amount = amount;
        this._url = url;
        this._block = $(block);
        this._blockItem = blockItem;
        this._itemWrapper = itemWrapper;
        this._data = [];
        this._alterCount = alterCount;
    }
    Videos.prototype.initialize = function () {
        this.getVideos();
    };
    Videos.prototype.getVideos = function () {
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
    Videos.prototype.fillVideos = function () {
        var _this = this;
        this._block.find(this._blockItem).each(function (i, e) {
            _this._counter++;
            if (_this._counter === _this._alterCount) {
                _this.generateVideoRow(e);
                _this._counter = 0;
            }
        });
    };
    Videos.prototype.generateVideoRow = function (e) {
        var counter = this._colCount;
        var itemWrapper = $(this._itemWrapper);
        var interVideoBlock = $("<div></div>").addClass("inter-videos");
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
        interVideoBlock.append(row);
        itemWrapper.append(interVideoBlock);
        itemWrapper.insertAfter(e);
    };
    Videos.prototype.removeVideosBlock = function () {
        this._data.splice(0, this._colCount);
    };
    Videos.prototype.generateVideoItem = function (video, row) {
        var item = $("<div class=\"col-md-4\"><div class=\"inter-videos-item\"><figure class=\"video\"" +
            "style=\"background-image:url('http://img.youtube.com/vi/"
            + video.VideoId + "/mqdefault.jpg');\" id=\"" + video.Id + "\"><div class=\"v-p-wr\">" +
            "<a href=\"" + video.VideoUrl + "\" target=\"_blank\">" +
            "<i class=\"fa fa-youtube-play\"></i></a></div></figure></div></div>");
        row.append(item);
    };
    return Videos;
}());
