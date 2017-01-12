var Games = (function () {
    function Games(dataUrl, linkedUrl, addSteamUrl, delSteamApps, grid) {
        var _this = this;
        this._modalSteamApps = $("#modal-steam-apps");
        this._modalSteamAppsLinked = $("#modal-linked-steam-apps");
        this._modalSteamAppsInputGame = this._modalSteamApps.find("input[name=\"GameId\"]");
        this._modalSteamLinkedAppsInputGame = this._modalSteamAppsLinked.find("input[name=\"GameId\"]");
        this._modalSteamAppsBody = $("#modal-steam-apps-body");
        this._modalSteamAppsLinkedBody = $("#modal-linked-steam-apps-body");
        this._modalSteamAppsGrid = new SxGridView(this._modalSteamAppsBody, null, this.steamAppsGridCheckboxCallback);
        this._modalSteamAppsGridLinked = new SxGridView(this._modalSteamAppsLinkedBody, null, this.steamAppsGridLinkedCheckboxCallback);
        this._grid = grid;
        this._modalSteamApps.on("show.bs.modal", function (e) {
            var btn = $(e.relatedTarget);
            var gameId = parseInt(btn.closest("tr").attr("data-row-id"));
            _this._modalSteamAppsInputGame.val(gameId);
            $.ajax({
                method: "post",
                url: dataUrl,
                beforeSend: function () {
                    $("#game-steam-app-add-btn").attr("disabled", "disabled");
                    _this._modalSteamAppsGrid.clearSelectedRows();
                },
                success: function (data, status, xhr) {
                    _this._modalSteamAppsBody.html(data);
                }
            });
        });
        this._modalSteamAppsLinked.on("show.bs.modal", function (e) {
            var btn = $(e.relatedTarget);
            var gameId = parseInt(btn.closest("tr").attr("data-row-id"));
            _this._modalSteamLinkedAppsInputGame.val(gameId);
            $.ajax({
                method: "post",
                url: linkedUrl,
                data: { gameId: gameId },
                beforeSend: function () {
                    $("#game-del-steam-app-add-btn").attr("disabled", "disabled");
                    _this._modalSteamAppsGridLinked.clearSelectedRows();
                },
                success: function (data, status, xhr) {
                    _this._modalSteamAppsLinkedBody.html(data);
                }
            });
        });
        $("#game-steam-app-add-btn").on("click", function (e) {
            var gameId = _this._modalSteamApps.find("input[name=\"GameId\"]").val();
            var steamAppIds = _this._modalSteamAppsGrid.selectedRows();
            var aft = _this._modalSteamApps.find("input[name=\"__RequestVerificationToken\"]").val();
            $.ajax({
                method: "post",
                url: addSteamUrl,
                data: { gameId: gameId, steamAppIds: steamAppIds, __RequestVerificationToken: aft },
                success: function (data, status, xhr) {
                    var s = data.MessageType;
                    if (s === 2) {
                        console.log(data.Message);
                        return;
                    }
                    _this._modalSteamApps.modal("hide");
                    _this._grid.getData({ page: 1 });
                }
            });
        });
        $("#game-del-steam-app-add-btn").on("click", function (e) {
            var gameId = _this._modalSteamAppsLinked.find("input[name=\"GameId\"]").val();
            var steamAppIds = _this._modalSteamAppsGridLinked.selectedRows();
            var aft = _this._modalSteamAppsLinked.find("input[name=\"__RequestVerificationToken\"]").val();
            $.ajax({
                method: "post",
                url: delSteamApps,
                data: { gameId: gameId, steamAppIds: steamAppIds, __RequestVerificationToken: aft },
                success: function (data, status, xhr) {
                    var s = data.MessageType;
                    if (s === 2) {
                        console.log(data.Message);
                        return;
                    }
                    _this._modalSteamAppsLinked.modal("hide");
                    _this._grid.getData({ page: 1 });
                }
            });
        });
        $("#game-del-all-steam-app-add-btn").on("click", function (e) {
            var gameId = _this._modalSteamAppsLinked.find("input[name=\"GameId\"]").val();
            var aft = _this._modalSteamAppsLinked.find("input[name=\"__RequestVerificationToken\"]").val();
            $.ajax({
                method: "post",
                url: delSteamApps,
                data: { gameId: gameId, __RequestVerificationToken: aft },
                success: function (data, status, xhr) {
                    var s = data.MessageType;
                    if (s === 2) {
                        console.log(data.Message);
                        return;
                    }
                    _this._modalSteamAppsLinked.modal("hide");
                    _this._grid.getData({ page: 1 });
                }
            });
        });
    }
    Games.prototype.steamAppsGridCheckboxCallback = function () {
        var grid = this;
        if (grid.getSelectedRowsCount() > 0)
            $("#game-steam-app-add-btn").removeAttr("disabled");
        else
            $("#game-steam-app-add-btn").attr("disabled", "disabled");
    };
    Games.prototype.steamAppsGridLinkedCheckboxCallback = function () {
        var grid = this;
        if (grid.getSelectedRowsCount() > 0)
            $("#game-del-steam-app-add-btn").removeAttr("disabled");
        else
            $("#game-del-steam-app-add-btn").attr("disabled", "disabled");
    };
    return Games;
}());
