var EditMaterialPage = (function () {
    function EditMaterialPage(mid, mct) {
        var _this = this;
        this._infographicsUrl = "/admin/infographics";
        var g1 = new SxGridView("#infographics");
        var g2 = new SxGridView("#modal-infographics-not-linked-block");
        this._mid = mid;
        this._mct = mct;
        this._inpographicTab = $("a[href=\"#mm-infographics\"]");
        this._infographics = $("#infographics");
        this._infographicsModal = $("#modal-infographics-not-linked");
        this._infographicsModalAddBtn = $("#modal-infographics-not-linked-add-btn");
        this._infographicsAddForm = this._infographicsModal.find("form");
        this._inpographicTab.on("show.bs.tab", function () {
            $.ajax({
                method: "get",
                url: _this._infographicsUrl,
                data: { mid: _this._mid, mct: _this._mct },
                beforeSend: function () {
                    $("<i></i>").addClass("fa fa-spinner fa-spin").attr("aria-hidden", "true").prependTo(_this._inpographicTab);
                },
                success: function (data, status, xhr) {
                    _this._infographics.html(data);
                    _this._inpographicTab.find(".fa-spinner").remove();
                }
            });
        });
        this._infographics.on("click", ".sx-gv__create-btn", function (e) {
            e.preventDefault();
            _this._infographicsModal.modal("show");
        });
        this._infographicsModal.on("show.bs.modal", function () {
            $.ajax({
                method: "get",
                url: _this._infographicsUrl,
                data: { mid: _this._mid, mct: _this._mct, linked: false },
                beforeSend: function () {
                    g2.clearSelectedRows();
                },
                success: function (data, status, xhr) {
                    _this._infographicsModal.find(".modal-body > #modal-infographics-not-linked-block").html(data);
                }
            });
        });
        this._infographicsModal.on("hide.bs.modal", function () {
            _this._infographicsModal.find(".modal-body > #modal-infographics-not-linked-block").html("");
        });
        this._infographicsModalAddBtn.on("click", function (e) {
            e.preventDefault();
            var aft = _this._infographicsAddForm.find("[name=\"__RequestVerificationToken\"]").val();
            var ids = g2.selectedRows();
            if (ids.length === 0) {
                return;
            }
            $.ajax({
                method: "post",
                url: _this._infographicsAddForm.attr("action"),
                data: { mid: _this._mid, mct: _this._mct, __RequestVerificationToken: aft, ids: ids },
                success: function (data, status, xhr) {
                    _this._infographicsModal.modal("hide");
                    _this._infographics.html(data);
                }
            });
            return;
        });
    }
    return EditMaterialPage;
}());
