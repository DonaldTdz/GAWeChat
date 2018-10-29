export class PageModel implements IPageModel {
    pageIndex: number = 1;
    pageSize: number = 10;
    isLast: boolean = false;

    constructor(data?: IPageModel) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.pageIndex = data["pageIndex"];
            this.pageSize = data["pageSize"];
            this.isLast = data["isLast"];
        }
    }

    static fromJS(data: any): PageModel {
        let result = new PageModel();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["pageIndex"] = this.pageIndex;
        data["pageSize"] = this.pageSize;
        data["isLast"] = this.isLast;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new PageModel();
        result.init(json);
        return result;
    }
}
export interface IPageModel {
    pageIndex: number;
    pageSize: number;
    isLast: boolean;
}