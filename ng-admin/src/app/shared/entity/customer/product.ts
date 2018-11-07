export class Products implements IProducts {
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
    activeText: string;
    activeType: string;
    showPhotoUrl: string;
    itemId: string;
    itemCode: string;
    mfrId: string;
    company: string;
    fileName: string;
    img64: string;
    desc: string;
    tags: string;
    constructor(data?: IProducts) {
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
            this.itemId = data["itemId"];
            this.itemCode = data["itemCode"];
            this.mfrId = data["mfrId"];
            this.company = data["company"];
            this.fileName = data["fileName"];
            this.img64 = data["img64"];
            this.desc = data["desc"];
            this.tags = data["tags"];
        }
    }

    static fromJS(data: any): Products {
        let result = new Products();
        result.init(data);
        return result;
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
        data["itemId"] = this.itemId;
        data["itemCode"] = this.itemCode;
        data["mfrId"] = this.mfrId;
        data["company"] = this.company;
        data["fileName"] = this.fileName;
        data["img64"] = this.img64;
        data["desc"] = this.desc;
        data["tags"] = this.tags;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new Products();
        result.init(json);
        return result;
    }
}
export interface IProducts {
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
    itemId: string;
    itemCode: string;
    mfrId: string;
    company: string;
    fileName: string;
    img64: string;
    desc: string;
    tags: string;
}