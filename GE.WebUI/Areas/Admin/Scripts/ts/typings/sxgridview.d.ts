declare class SxGridView {
    constructor(element: any, callback?: any, checkboxCallback?: any);

    public clearSelectedRows(): any;

    public selectCheckboxes(): any;

    public selectedRows(): any[];

    public getData(query: any): any;

    public getCurrentPage(): number;

    public setCheckboxCallback(callback: any): void;

    public getSelectedRowsCount(): number
}