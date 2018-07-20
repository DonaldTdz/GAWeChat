export class ExhibitionShop implements IExhibitionShop {
    id: string;
    createTime: Date;
    retailerId: string;
    shopName: string;
    shopAddress: string;
    picPath: string;
    votes: number;
    status: number;
    shopId: string;
    auditTime: Date;
    custCode: string;
    custName: string;
    area: string;
    fansNum: number;
    phone: string;
    noId: number;
    constructor(data?: IExhibitionShop) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.id = data["id"];
            this.createTime = data["createTime"];
            this.retailerId = data["retailerId"];
            this.shopName = data["shopName"];
            this.shopAddress = data["shopAddress"];
            this.picPath = data["picPath"];
            this.votes = data["votes"];
            this.status = data["status"];
            this.shopId = data["shopId"];
            this.auditTime = data["auditTime"];
            this.custCode = data["custCode"];
            this.custName = data["custName"];
            this.area = data["area"];
            this.fansNum = data["fansNum"];
            this.phone = data["phone"];
            this.noId = data["noId"];
        }
    }

    static fromJS(data: any): ExhibitionShop {
        let result = new ExhibitionShop();
        result.init(data);
        return result;
    }

    static fromJSArray(dataArray: any[]): ExhibitionShop[] {
        let array = [];
        dataArray.forEach(result => {
            let item = new ExhibitionShop();
            item.init(result);
            array.push(item);
        });
        return array;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id;
        data["createTime"] = this.createTime;
        data["retailerId"] = this.retailerId;
        data["shopName"] = this.shopName;
        data["shopAddress"] = this.shopAddress;
        data["picPath"] = this.picPath;
        data["votes"] = this.votes;
        data["status"] = this.status;
        data["shopId"] = this.shopId;
        data["auditTime"] = this.auditTime;
        data["custCode"] = this.custCode;
        data["custName"] = this.custName;
        data["area"] = this.area;
        data["fansNum"] = this.fansNum;
        data["phone"] = this.phone;
        data["noId"] = this.noId;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new ExhibitionShop();
        result.init(json);
        return result;
    }
}
export interface IExhibitionShop {
    id: string;
    createTime: Date;
    retailerId: string;
    shopName: string;
    shopAddress: string;
    picPath: string;
    votes: number;
    status: number;
    shopId: string;
    auditTime: Date;
    custCode: string;
    custName: string;
    area: string;
    fansNum: number;
    phone: string;
    noId: number;
}

