var SteamApi = (function () {
    function SteamApi(runBtn, getAppList) {
        var _this = this;
        this._runBtn = $(runBtn);
        this._appModal = $(this._runBtn.attr("data-target"));
        this._getAppList = getAppList;
        this._runBtn.on("click", function (e) {
            e.preventDefault();
            e.stopPropagation();
            _this._appModal.modal("show");
            _this._getAppList();
        });
    }
    return SteamApi;
}());
