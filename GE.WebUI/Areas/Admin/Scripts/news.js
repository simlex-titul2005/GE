window.onload = function () {
    var page = new pageNews();
};
var pageNews = (function () {
    function pageNews() {
        this._grid = $("#grid");
        this._grid.on("click", ".col-steam .btn", function (e) {
            alert("Здесь будет показана оригинальная новость. Пока не реализовано");
        });
    }
    return pageNews;
}());
