/// <reference path="../typings/jquery.d.ts" />
var GeRndAphorisms = (function () {
    function GeRndAphorisms(block, btnNextId, dataUrl, callback) {
        this._flag = true;
        this._block = $(block);
        this._progress = this._block.find(".rnd-aphorism__progress");
        this._btnNextId = btnNextId;
        this._dataUrl = dataUrl;
        this._callback = callback;
    }
    GeRndAphorisms.prototype.initialize = function () {
        var _this = this;
        this._block.on("click", this._btnNextId, function (e) {
            e.preventDefault();
            var id = _this._block.find(".rnd-aphorism").attr("data-id");
            $.ajax({
                method: "get",
                url: _this._dataUrl,
                data: { id: id },
                beforeSend: function () {
                    $("<i></i>").addClass("fa fa-spinner fa-spin").attr("aria-hidden", "true").prependTo(_this._btnNextId);
                },
                success: function (data, status, xhr) {
                    _this._block.html(data);
                    if (_this._callback) {
                        _this._callback();
                    }
                    _this._progress = _this._block.find(".rnd-aphorism__progress");
                    var seconds = parseInt(_this._block.find("[data-seconds]").attr("data-seconds"));
                    _this.initializeTimer(seconds);
                }
            });
        });
        this._block.on("mouseenter", function (e) {
            _this._flag = false;
        });
        this._block.on("mouseleave", function (e) {
            _this._flag = true;
        });
        var seconds = parseInt(this._block.find("[data-seconds]").attr("data-seconds"));
        this.initializeTimer(seconds);
    };
    ;
    GeRndAphorisms.prototype.initializeTimer = function (seconds) {
        var _this = this;
        clearInterval(this._timer);
        this._seconds = seconds;
        this._timerCounter = this._seconds;
        this._timer = setInterval(function () { _this.reloadProgress(); }, 1000);
    };
    ;
    GeRndAphorisms.prototype.reloadProgress = function () {
        if (!this._flag)
            return;
        if (this._timerCounter > 0) {
            this._timerCounter--;
            var width = this._timerCounter * 100 / this._seconds;
            this._progress.css("width", width + "%");
        }
        else {
            $(this._btnNextId).trigger("click");
        }
    };
    ;
    return GeRndAphorisms;
}());
