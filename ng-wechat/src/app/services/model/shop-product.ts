export class ShopProduct implements IShopProduct {
    id: string;
    specification: string;
    type: number;
    price: number;
    isRare: boolean;
    packageCode: string;
    barCode: string;
    searchCount: number;
    isAction: boolean;
    creationTime: Date;
    tenantId: number;
    creatorUserId: number;
    photoUrl: string;
    typeName: string;
    shopId: string;
    productId: string;
    desc: string;
    tags: string;
    constructor(data?: IShopProduct) {
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
            this.specification = data["specification"];
            this.type = data["type"];
            this.price = data["price"];
            this.isRare = data["isRare"];
            this.packageCode = data["packageCode"];
            this.barCode = data["barCode"];
            this.searchCount = data["searchCount"];
            this.isAction = data["isAction"];
            this.creationTime = data["creationTime"];
            this.tenantId = data["tenantId"];
            this.creatorUserId = data["creatorUserId"];
            this.photoUrl = data["photoUrl"];
            this.typeName = data["typeName"];
            this.shopId = data["shopId"];
            this.productId = data["productId"];
            this.desc = data["desc"];
            this.tags = data["tags"];
        }
    }

    static fromJS(data: any): ShopProduct {
        let result = new ShopProduct();
        result.init(data);
        return result;
    }

    static fromJSArray(dataArray: any[]): ShopProduct[] {
        let array = [];
        dataArray.forEach(result => {
            let item = new ShopProduct();
            item.init(result);
            array.push(item);
        });

        return array;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id;
        data["specification"] = this.specification;
        data["type"] = this.type;
        data["price"] = this.price;
        data["isRare"] = this.isRare;
        data["packageCode"] = this.packageCode;
        data["barCode"] = this.barCode;
        data["searchCount"] = this.searchCount;
        data["isAction"] = this.isAction;
        data["creationTime"] = this.creationTime;
        data["tenantId"] = this.tenantId;
        data["creatorUserId"] = this.creatorUserId;
        data["photoUrl"] = this.photoUrl;
        data["typeName"] = this.typeName;
        data["shopId"] = this.shopId;
        data["productId"] = this.productId;
        data["desc"] = this.desc;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new ShopProduct();
        result.init(json);
        return result;
    }
}
export interface IShopProduct {
    id: string;
    specification: string;
    type: number;
    price: number;
    isRare: boolean;
    packageCode: string;
    barCode: string;
    searchCount: number;
    isAction: boolean;
    creationTime: Date;
    tenantId: number;
    creatorUserId: number;
    photoUrl: string;
    typeName: string;
    shopId: string;
    productId: string;
    desc: string;
    tags: string;
}

export class ShopGoods implements IShopGoods {
    id: string;
    specification: string;
    price: number;
    packageCode: string;
    barCode: string;
    num: number;

    constructor(data?: IShopGoods) {
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
            this.specification = data["specification"];
            this.price = data["price"];
            this.packageCode = data["packageCode"];
            this.barCode = data["barCode"];
            this.num = data["num"];
        }
    }

    static fromJS(data: any): ShopGoods {
        let result = new ShopGoods();
        result.init(data);
        return result;
    }

    static fromJSArray(dataArray: any[]): ShopGoods[] {
        let array = [];
        dataArray.forEach(result => {
            let item = new ShopGoods();
            item.init(result);
            array.push(item);
        });

        return array;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id;
        data["specification"] = this.specification;
        data["price"] = this.price;
        data["packageCode"] = this.packageCode;
        data["barCode"] = this.barCode;
        data["num"] = this.num;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new ShopGoods();
        result.init(json);
        return result;
    }
}
export interface IShopGoods {
    id: string;
    specification: string;
    price: number;
    packageCode: string;
    barCode: string;
    num: number;
}

