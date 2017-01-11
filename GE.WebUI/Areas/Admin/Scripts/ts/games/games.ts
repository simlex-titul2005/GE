/// <reference path="../typings/sxgridview.d.ts" />
/// <reference path="../typings/bootstrap.d.ts" />
/// <reference path="../typings/jquery.d.ts" />

class Games {
    private _modalSteamApps: JQuery;
    private _modalSteamAppsInputGame: JQuery;
    private _modalSteamAppsBody: JQuery;
    private _dataUrl: string;
    private _dataUrlPars: any;
    private _modalSteamAppsGrid: SxGridView;
    private _grid: SxGridView;

    constructor(dataUrl: string, addSteamUrl: string, delSteamApp: string, grid: SxGridView, dataUrlPars?: any) {
        this._modalSteamApps = $("#modal-steam-apps");
        this._modalSteamAppsInputGame = this._modalSteamApps.find("input[name=\"GameId\"]");
        this._modalSteamAppsBody = $("#modal-steam-apps-body");
        this._dataUrl = dataUrl;
        this._dataUrlPars = dataUrlPars;
        this._modalSteamAppsGrid = new SxGridView(this._modalSteamAppsBody);
        this._grid = grid;

        this._modalSteamApps.on("show.bs.modal", (e: JQueryEventObject): void => {
            var btn: JQuery = $(e.relatedTarget);
            var gameId: number = parseInt(btn.closest("tr").attr("data-row-id"));
            this._modalSteamAppsInputGame.val(gameId);

            $.ajax({
                method: "post",
                url: this._dataUrl,
                data: this._dataUrlPars,
                success: (data: any, status: string, xhr: JQueryXHR): void => {
                    this._modalSteamAppsBody.html(data);
                }
            });
        });

        this._modalSteamApps.on("click", ".free-steam-app-btn", (e: JQueryEventObject): void => {
            var btn: JQuery = $(e.currentTarget);
            var gameId: any = this._modalSteamAppsInputGame.val();
            var steamAppId: any = btn.closest("tr").attr("data-row-id");
            var rvt: any = this._modalSteamApps.find("input[name=\"__RequestVerificationToken\"]").val();
            $.ajax({
                method: "post",
                url: addSteamUrl,
                data: { gameId: gameId, steamAppId: steamAppId, __RequestVerificationToken: rvt },
                success: (data: any, status: string, xhr: JQueryXHR): void => {
                    var s: number = parseInt(data.MessageType);
                    var message: string = data.Message;
                    if (s === 2) {
                        console.log(message);
                        return;
                    }

                    this._modalSteamApps.modal("hide");
                    this._grid.getData({ page: this._grid.getCurrentPage() });
                }
            });
        });

        $("#grid-games").on("click", ".del-game-steam-app-btn", (e: JQueryEventObject): void => {
            var gameId: any = $(e.currentTarget).closest("tr").attr("data-row-id");
            var rvt: any = this._modalSteamApps.find("input[name=\"__RequestVerificationToken\"]").val();
            $.ajax({
                method: "post",
                url: delSteamApp,
                data: { gameId: gameId, __RequestVerificationToken: rvt },
                success: (data: any, status: string, xhr: JQueryXHR): void => {
                    var s: number = parseInt(data.MessageType);
                    var message: string = data.Message;
                    if (s === 2) {
                        console.log(message);
                        return;
                    }

                    this._grid.getData({ page: this._grid.getCurrentPage() });
                }
            });
        });
    }
}