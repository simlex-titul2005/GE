/// <reference path="../typings/jquery.d.ts" />

window.onload = () => {
    var page = new pageNews();
};

class pageNews {
    private _grid: JQuery;

    constructor() {
        this._grid = $("#grid");

        this._grid.on("click", ".col-steam .btn", (e: JQueryEventObject): void => {
            alert("Здесь будет показана оригинальная новость. Пока не реализовано");
        });
    }
}