export class PurchaseRecord implements IPurchaseRecord {
    id: string;
    productId: string;
    specification: string;
    quantity: number;
    shopId: string;
    shopName: string;
    openId: string;
    tenantId: number;
    integral: number;
    remark: string;
    creationTime: Date;
    operatorOpenId: string;
    operatorName: string;
    isEvaluation: boolean;
    weChatName: string;
    phone: string;
    favouriteSpecification: string;
    price: number;
    constructor(data?: IPurchaseRecord) {
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
            this.productId = data["productId"];
            this.specification = data["specification"];
            this.quantity = data["quantity"];
            this.shopId = data["shopId"];
            this.shopName = data["shopName"];
            this.openId = data["openId"];
            this.tenantId = data["tenantId"];
            this.integral = data["integral"];
            this.remark = data["remark"];
            this.creationTime = data["creationTime"];
            this.operatorOpenId = data["operatorOpenId"];
            this.operatorName = data["operatorName"];
            this.isEvaluation = data["isEvaluation"];
            this.weChatName = data["weChatName"];
            this.phone = data["phone"];
            this.favouriteSpecification = data["favouriteSpecification"];
            this.price = data["price"];
        }
    }

    static fromJS(data: any): PurchaseRecord {
        let result = new PurchaseRecord();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id;
        data["productId"] = this.productId;
        data["specification"] = this.specification;
        data["quantity"] = this.quantity;
        data["shopId"] = this.shopId;
        data["shopName"] = this.shopName;
        data["openId"] = this.openId;
        data["tenantId"] = this.tenantId;
        data["integral"] = this.integral;
        data["remark"] = this.remark;
        data["creationTime"] = this.creationTime;
        data["operatorOpenId"] = this.operatorOpenId;
        data["operatorName"] = this.operatorName;
        data["isEvaluation"] = this.isEvaluation;
        data["weChatName"] = this.weChatName;
        data["phone"] = this.phone;
        data["favouriteSpecification"] = this.favouriteSpecification;
        data["price"] = this.price;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new PurchaseRecord();
        result.init(json);
        return result;
    }
}
export interface IPurchaseRecord {
    id: string;
    productId: string;
    specification: string;
    quantity: number;
    shopId: string;
    shopName: string;
    openId: string;
    tenantId: number;
    integral: number;
    remark: string;
    creationTime: Date;
    operatorOpenId: string;
    operatorName: string;
    isEvaluation: boolean;
    weChatName: string;
    phone: string;
    favouriteSpecification: string;
    price: number;
}