declare class SxGridView {
    constructor(element: any, callback?: any);

    public clearSelectedRows(): any;

    public selectCheckboxes(): any;

    public selectedRows(): any[];

    public getData(query: any): any;

    public getCurrentPage(): number;
}