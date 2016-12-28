/// <reference path="../typings/jquery.d.ts" />

class GeRndAphorisms {
    private _block: JQuery;
    private _progress: JQuery;
    private _btnNextId: string;
    private _dataUrl: string;
    private _timer: number;
    private _seconds: number;
    private _timerCounter: number;
    private _flag: boolean = true;
    private _callback: any;

    constructor(block: any, btnNextId: string, dataUrl: string, callback?: any) {
        this._block = $(block);
        this._progress = this._block.find(".rnd-aphorism__progress");
        this._btnNextId = btnNextId;
        this._dataUrl = dataUrl;
        this._callback = callback;
    }

    public initialize(): void {
        this._block.on("click", this._btnNextId, (e: JQueryInputEventObject): void => {
            e.preventDefault();
            var id: any = this._block.find(".rnd-aphorism").attr("data-id");
            $.ajax({
                method: "get",
                url: this._dataUrl,
                data: { id: id },
                beforeSend: () => {
                    $("<i></i>").addClass("fa fa-spinner fa-spin").attr("aria-hidden", "true").prependTo(this._btnNextId);
                },
                success: (data: any, status: string, xhr: JQueryXHR): void => {
                    this._block.html(data);
                    if (this._callback)
                    {
                        this._callback();
                    }
                    this._progress = this._block.find(".rnd-aphorism__progress");
                    var seconds: number = parseInt(this._block.find("[data-seconds]").attr("data-seconds"));
                    this.initializeTimer(seconds);
                }
            });
        });

        this._block.on("mouseenter", (e: JQueryInputEventObject): void => {
            this._flag = false;
        });

        this._block.on("mouseleave", (e: JQueryInputEventObject): void => {
            this._flag = true;
        });

        var seconds: number = parseInt(this._block.find("[data-seconds]").attr("data-seconds"));
        this.initializeTimer(seconds);
    };

    private initializeTimer(seconds: number): void {
        clearInterval(this._timer);
        this._seconds = seconds;
        this._timerCounter = this._seconds;
        this._timer = setInterval(() => { this.reloadProgress() }, 1000);
    };

    private reloadProgress(): void {
        if (!this._flag) return;

        if (this._timerCounter > 0) {
            this._timerCounter--;
            var width: number = this._timerCounter * 100 / this._seconds;
            this._progress.css("width", width + "%");
        }
        else {
            $(this._btnNextId).trigger("click");
        }
    };
}