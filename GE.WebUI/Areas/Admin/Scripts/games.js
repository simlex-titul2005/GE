var Games = (function () {
    function Games(dataUrl, addSteamUrl, delSteamApp, grid, dataUrlPars) {
        var _this = this;
        this._modalSteamApps = $("#modal-steam-apps");
        this._modalSteamAppsInputGame = this._modalSteamApps.find("input[name=\"GameId\"]");
        this._modalSteamAppsBody = $("#modal-steam-apps-body");
        this._dataUrl = dataUrl;
        this._dataUrlPars = dataUrlPars;
        this._modalSteamAppsGrid = new SxGridView(this._modalSteamAppsBody);
        this._grid = grid;
        this._modalSteamApps.on("show.bs.modal", function (e) {
            var btn = $(e.relatedTarget);
            var gameId = parseInt(btn.closest("tr").attr("data-row-id"));
            _this._modalSteamAppsInputGame.val(gameId);
            $.ajax({
                method: "post",
                url: _this._dataUrl,
                data: _this._dataUrlPars,
                success: function (data, status, xhr) {
                    _this._modalSteamAppsBody.html(data);
                }
            });
        });
        this._modalSteamApps.on("click", ".free-steam-app-btn", function (e) {
            var btn = $(e.currentTarget);
            var gameId = _this._modalSteamAppsInputGame.val();
            var steamAppId = btn.closest("tr").attr("data-row-id");
            var rvt = _this._modalSteamApps.find("input[name=\"__RequestVerificationToken\"]").val();
            $.ajax({
                method: "post",
                url: addSteamUrl,
                data: { gameId: gameId, steamAppId: steamAppId, __RequestVerificationToken: rvt },
                success: function (data, status, xhr) {
                    var s = parseInt(data.MessageType);
                    var message = data.Message;
                    if (s === 2) {
                        console.log(message);
                        return;
                    }
                    _this._modalSteamApps.modal("hide");
                    _this._grid.getData({ page: _this._grid.getCurrentPage() });
                }
            });
        });
        $("#grid-games").on("click", ".del-game-steam-app-btn", function (e) {
            var gameId = $(e.currentTarget).closest("tr").attr("data-row-id");
            var rvt = _this._modalSteamApps.find("input[name=\"__RequestVerificationToken\"]").val();
            $.ajax({
                method: "post",
                url: delSteamApp,
                data: { gameId: gameId, __RequestVerificationToken: rvt },
                success: function (data, status, xhr) {
                    var s = parseInt(data.MessageType);
                    var message = data.Message;
                    if (s === 2) {
                        console.log(message);
                        return;
                    }
                    _this._grid.getData({ page: _this._grid.getCurrentPage() });
                }
            });
        });
    }
    return Games;
}());
