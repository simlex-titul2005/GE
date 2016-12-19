/// <reference path="../typings/jquery.d.ts" />

class ApVideos {
    private _url: string;
    private _cat: number;
    private _amount: number;
    private _block: JQuery;
    private _data: Array<any>;
    private _itemsCount: number = 5;
    private _colCount: number = 3;
    private _counter: number = 0;

    constructor(url: string, cat: number, amount: number) {
        this._cat = cat;
        this._amount = amount;
        this._url = url;
        this._block = $(".ap-list ul");
        this._data = [];
    }

    public initialize(): void {
        this.getVideos();
    }

    private getVideos(): void {
        $.ajax({
            method: "get",
            url: this._url,
            data: { cat: this._cat, amount: this._amount },
            success: (data: any, status: string, xhr: JQueryXHR): void => {
                this._data = data;
                this.fillVideos();
            }
        });
    }

    private fillVideos(): void {
        this._block.find(".ap-list__item").each((i: number, e: Element) => {
            this._counter++;
            if (this._counter === this._itemsCount) {
                this.generateVideoRow(e);
                this._counter = 0;
            }
        });
    }

    private generateVideoRow(e: Element): void {
        var counter: number = this._colCount;
        var li: JQuery = $("<li></li>");
        var row: JQuery = $("<div class=\"row\"></div>");
        for (var video of this._data) {
            if (counter <= 0) {
                this.removeVideosBlock();
                break;
            }

            this.generateVideoItem(video, row);
            counter--;
        }
        li.append(row);
        li.insertAfter(e);
    }

    private removeVideosBlock(): void {
        this._data.splice(0, this._colCount);
    }

    private generateVideoItem(video: any, row: JQuery): void {
        var item: JQuery = $("<div class=\"col-md-4\"><div class=\"ap-list__video\"><figure class=\"video\"" +
            "style=\"background-image:url('http://img.youtube.com/vi/"
            + video.VideoId + "/mqdefault.jpg');\" id=\"" + video.Id + "\"><div class=\"v-p-wr\">" +
            "<a href=\"" + video.VideoUrl + "\" target=\"_blank\">" +
            "<i class=\"fa fa-youtube-play\"></i></a></div></figure></div></div>");
        row.append(item);
    }
}