export class Favorite implements IFavorite {
    id: string;
    shopId: string;
    shopName: string;
    openId: string;
    nickName: string;
    creationTime: Date;
    cancelTime: Date;
    isCancel: boolean;
    productId: string;
    coverPhoto: string;
    constructor(data?: IFavorite) {
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
            this.shopId = data["shopId"];
            this.shopName = data["shopName"];
            this.openId = data["openId"];
            this.nickName = data["nickName"];
            this.creationTime = data["creationTime"];
            this.cancelTime = data["cancelTime"];
            this.isCancel = data["isCancel"];
            this.productId = data["productId"];
            this.coverPhoto = data["coverPhoto"];
        }
    }

    static fromJS(data: any): Favorite {
        let result = new Favorite();
        result.init(data);
        return result;
    }

    static fromJSArray(dataArray: any[]): Favorite[] {
        let array = [];
        dataArray.forEach(result => {
            let item = new Favorite();
            item.init(result);
            array.push(item);
        });
        return array;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id;
        data["shopId"] = this.shopId;
        data["shopName"] = this.shopName;
        data["openId"] = this.openId;
        data["nickName"] = this.nickName;
        data["creationTime"] = this.creationTime;
        data["cancelTime"] = this.cancelTime;
        data["isCancel"] = this.isCancel;
        data["productId"] = this.productId;
        data["coverPhoto"] = this.coverPhoto;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new Favorite();
        result.init(json);
        return result;
    }
}
export interface IFavorite {
    id: string;
    shopId: string;
    shopName: string;
    openId: string;
    nickName: string;
    creationTime: Date;
    cancelTime: Date;
    isCancel: boolean;
    productId: string;
    coverPhoto: string;
}

