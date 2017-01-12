/// <reference path="../typings/sxgridview.d.ts" />
/// <reference path="../typings/bootstrap.d.ts" />
/// <reference path="../typings/jquery.d.ts" />

class Games {
    private _modalSteamApps: JQuery;
    private _modalSteamAppsLinked: JQuery;

    private _modalSteamAppsInputGame: JQuery;
    private _modalSteamLinkedAppsInputGame: JQuery;

    private _modalSteamAppsBody: JQuery;
    private _modalSteamAppsLinkedBody: JQuery;

    private _modalSteamAppsGrid: SxGridView;
    private _modalSteamAppsGridLinked: SxGridView;

    private _grid: SxGridView;

    constructor(dataUrl: string, linkedUrl: string, addSteamUrl: string, delSteamApps: string, grid: SxGridView) {
        this._modalSteamApps = $("#modal-steam-apps");
        this._modalSteamAppsLinked = $("#modal-linked-steam-apps");

        this._modalSteamAppsInputGame = this._modalSteamApps.find("input[name=\"GameId\"]");
        this._modalSteamLinkedAppsInputGame = this._modalSteamAppsLinked.find("input[name=\"GameId\"]");

        this._modalSteamAppsBody = $("#modal-steam-apps-body");
        this._modalSteamAppsLinkedBody = $("#modal-linked-steam-apps-body");
        
        this._modalSteamAppsGrid = new SxGridView(this._modalSteamAppsBody, null, this.steamAppsGridCheckboxCallback);
        this._modalSteamAppsGridLinked = new SxGridView(this._modalSteamAppsLinkedBody, null, this.steamAppsGridLinkedCheckboxCallback);

        this._grid = grid;
        

        this._modalSteamApps.on("show.bs.modal", (e: JQueryEventObject): void => {
            var btn: JQuery = $(e.relatedTarget);
            var gameId: number = parseInt(btn.closest("tr").attr("data-row-id"));
            this._modalSteamAppsInputGame.val(gameId);

            $.ajax({
                method: "post",
                url: dataUrl,
                beforeSend: (): void => {
                    $("#game-steam-app-add-btn").attr("disabled", "disabled");
                    this._modalSteamAppsGrid.clearSelectedRows();
                },
                success: (data: any, status: string, xhr: JQueryXHR): void => {
                    this._modalSteamAppsBody.html(data);
                }
            });
        });

        this._modalSteamAppsLinked.on("show.bs.modal", (e: JQueryEventObject): void => {
            var btn: JQuery = $(e.relatedTarget);
            var gameId: number = parseInt(btn.closest("tr").attr("data-row-id"));
            this._modalSteamLinkedAppsInputGame.val(gameId);

            $.ajax({
                method: "post",
                url: linkedUrl,
                data: { gameId: gameId },
                beforeSend: (): void => {
                    $("#game-del-steam-app-add-btn").attr("disabled", "disabled");
                    this._modalSteamAppsGridLinked.clearSelectedRows();
                },
                success: (data: any, status: string, xhr: JQueryXHR): void => {
                    this._modalSteamAppsLinkedBody.html(data);
                }
            });
        });

        $("#game-steam-app-add-btn").on("click", (e: JQueryEventObject): void => {
            var gameId: any = this._modalSteamApps.find("input[name=\"GameId\"]").val();
            var steamAppIds: any = this._modalSteamAppsGrid.selectedRows();
            var aft: any = this._modalSteamApps.find("input[name=\"__RequestVerificationToken\"]").val();
            $.ajax({
                method: "post",
                url: addSteamUrl,
                data: { gameId: gameId, steamAppIds: steamAppIds, __RequestVerificationToken: aft },
                success: (data: any, status: string, xhr: JQueryXHR): void => {
                    var s: any = data.MessageType;
                    if (s === 2) {
                        console.log(data.Message);
                        return;
                    }

                    this._modalSteamApps.modal("hide");
                    this._grid.getData({ page: 1 });
                }
            });
        });

        $("#game-del-steam-app-add-btn").on("click", (e: JQueryEventObject): void => {
            var gameId: any = this._modalSteamAppsLinked.find("input[name=\"GameId\"]").val();
            var steamAppIds: any = this._modalSteamAppsGridLinked.selectedRows();
            var aft: any = this._modalSteamAppsLinked.find("input[name=\"__RequestVerificationToken\"]").val();
            $.ajax({
                method: "post",
                url: delSteamApps,
                data: { gameId: gameId, steamAppIds: steamAppIds, __RequestVerificationToken: aft },
                success: (data: any, status: string, xhr: JQueryXHR): void => {
                    var s: any = data.MessageType;
                    if (s === 2) {
                        console.log(data.Message);
                        return;
                    }

                    this._modalSteamAppsLinked.modal("hide");
                    this._grid.getData({ page: 1 });
                }
            });
        });

        $("#game-del-all-steam-app-add-btn").on("click", (e: JQueryEventObject): void => {
            var gameId: any = this._modalSteamAppsLinked.find("input[name=\"GameId\"]").val();
            var aft: any = this._modalSteamAppsLinked.find("input[name=\"__RequestVerificationToken\"]").val();
            $.ajax({
                method: "post",
                url: delSteamApps,
                data: { gameId: gameId, __RequestVerificationToken: aft },
                success: (data: any, status: string, xhr: JQueryXHR): void => {
                    var s: any = data.MessageType;
                    if (s === 2) {
                        console.log(data.Message);
                        return;
                    }

                    this._modalSteamAppsLinked.modal("hide");
                    this._grid.getData({ page: 1 });
                }
            });
        });
    }

    private steamAppsGridCheckboxCallback(): void {
        var grid: SxGridView = <SxGridView><Object>this;
        if (grid.getSelectedRowsCount() > 0)
            $("#game-steam-app-add-btn").removeAttr("disabled");
        else
            $("#game-steam-app-add-btn").attr("disabled", "disabled");
    }

    private steamAppsGridLinkedCheckboxCallback(): void {
        var grid: SxGridView = <SxGridView><Object>this;
        if (grid.getSelectedRowsCount() > 0)
            $("#game-del-steam-app-add-btn").removeAttr("disabled");
        else
            $("#game-del-steam-app-add-btn").attr("disabled", "disabled");
    }
}