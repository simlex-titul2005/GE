/// <reference path="../typings/bootstrap.d.ts" />
/// <reference path="../typings/sxgridview.d.ts" />
/// <reference path="../typings/jquery.d.ts" />

class SteamApi {
    private _runBtn: JQuery;
    private _appModal: JQuery;
    private _getAppList: any;

    constructor(runBtn: any, getAppList: () => any) {
        this._runBtn = $(runBtn);
        this._appModal = $(this._runBtn.attr("data-target"));
        this._getAppList = getAppList;

        this._runBtn.on("click", (e: JQueryEventObject): void => {
            e.preventDefault();
            e.stopPropagation();
            this._appModal.modal("show");
            this._getAppList();
        });
    }
}