/// <reference path="../typings/jquery.d.ts" />

class Videos {
    private _url: string;
    private _cat: number;
    private _amount: number;
    private _block: JQuery;
    private _blockItem: string;
    private _itemWrapper: string;
    private _data: Array<any>;
    private _alterCount: number;
    private _colCount: number = 3;
    private _counter: number = 0;

    constructor(url: string, block: any, blockItem: string, itemWrapper:string, cat: number, amount: number, alterCount:number) {
        this._cat = cat;
        this._amount = amount;
        this._url = url;
        this._block = $(block);
        this._blockItem = blockItem;
        this._itemWrapper = itemWrapper;
        this._data = [];
        this._alterCount = alterCount;
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
        this._block.find(this._blockItem).each((i: number, e: Element) => {
            this._counter++;
            if (this._counter === this._alterCount) {
                this.generateVideoRow(e);
                this._counter = 0;
            }
        });
    }

    private generateVideoRow(e: Element): void {
        var counter: number = this._colCount;
        var itemWrapper: JQuery = $(this._itemWrapper);
        var interVideoBlock: JQuery = $("<div></div>").addClass("inter-videos");
        var row: JQuery = $("<div class=\"row\"></div>");
        for (var video of this._data) {
            if (counter <= 0) {
                this.removeVideosBlock();
                break;
            }

            this.generateVideoItem(video, row);
            counter--;
        }
        interVideoBlock.append(row);
        itemWrapper.append(interVideoBlock);
        itemWrapper.insertAfter(e);
    }

    private removeVideosBlock(): void {
        this._data.splice(0, this._colCount);
    }

    private generateVideoItem(video: any, row: JQuery): void {
        var item: JQuery = $("<div class=\"col-md-4\"><div class=\"inter-videos-item\"><figure class=\"video\"" +
            "style=\"background-image:url('http://img.youtube.com/vi/"
            + video.VideoId + "/mqdefault.jpg');\" id=\"" + video.Id + "\"><div class=\"v-p-wr\">" +
            "<a href=\"" + video.VideoUrl + "\" target=\"_blank\">" +
            "<i class=\"fa fa-youtube-play\"></i></a></div></figure></div></div>");
        row.append(item);
    }
}