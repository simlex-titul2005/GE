/// <reference path="../typings/bootstrap.d.ts" />
/// <reference path="../typings/sxgridview.d.ts" />
/// <reference path="../typings/jquery.d.ts" />

class EditMaterialPage {
    private _mid: number;
    private _mct: number;

    private _inpographicTab: JQuery;
    private _infographics: JQuery;
    private _infographicsModal: JQuery;
    private _infographicsModalAddBtn: JQuery;
    private _infographicsAddForm: JQuery;
    private _infographicsUrl: string = "/admin/infographics";

    constructor(mid: number, mct: number) {
        var g1: SxGridView = new SxGridView("#infographics");
        var g2: SxGridView = new SxGridView("#modal-infographics-not-linked-block");

        this._mid = mid;
        this._mct = mct;
        this._inpographicTab = $("a[href=\"#mm-infographics\"]");
        this._infographics = $("#infographics");
        this._infographicsModal = $("#modal-infographics-not-linked");
        this._infographicsModalAddBtn = $("#modal-infographics-not-linked-add-btn");
        this._infographicsAddForm = this._infographicsModal.find("form");

        this._inpographicTab.on("show.bs.tab", (): void => {
            $.ajax({
                method: "get",
                url: this._infographicsUrl,
                data: { mid: this._mid, mct: this._mct },
                beforeSend: (): void => {
                    $("<i></i>").addClass("fa fa-spinner fa-spin").attr("aria-hidden", "true").prependTo(this._inpographicTab);
                },
                success: (data: any, status: string, xhr: JQueryXHR): void => {
                    this._infographics.html(data);
                    this._inpographicTab.find(".fa-spinner").remove();
                }
            });
        });

        this._infographics.on("click", ".sx-gv__create-btn", (e: JQueryEventObject): void => {
            e.preventDefault();
            this._infographicsModal.modal("show");
        });

        this._infographicsModal.on("show.bs.modal", (): void => {
            $.ajax({
                method: "get",
                url: this._infographicsUrl,
                data: { mid: this._mid, mct: this._mct, linked: false },
                beforeSend: (): void => {
                    g2.clearSelectedRows();
                },
                success: (data: any, status: string, xhr: JQueryXHR): void => {
                    this._infographicsModal.find(".modal-body > #modal-infographics-not-linked-block").html(data);
                }
            });
        });

        this._infographicsModal.on("hide.bs.modal", (): void => {
            this._infographicsModal.find(".modal-body > #modal-infographics-not-linked-block").html("");
        });

        this._infographicsModalAddBtn.on("click", (e: JQueryEventObject): void => {
            e.preventDefault();
            var aft: string = this._infographicsAddForm.find("[name=\"__RequestVerificationToken\"]").val();
            var ids: any[] = g2.selectedRows();
            if (ids.length === 0) { return; }

            $.ajax({
                method: "post",
                url: this._infographicsAddForm.attr("action"),
                data: { mid: this._mid, mct: this._mct, __RequestVerificationToken: aft, ids: ids },
                success: (data: any, status: string, xhr: JQueryXHR): void => {
                    this._infographicsModal.modal("hide");
                    this._infographics.html(data);
                }
            });
            return;
        });
    }
}