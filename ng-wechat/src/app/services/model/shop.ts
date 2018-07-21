export class Shop implements IShop {
    id: string;
    name: string;
    address: string;
    desc: string;
    retailerId: string;
    coverPhoto: string;
    saleTotal: number;
    readTotal: number;
    singleTotal: number;
    evaluation: string;
    longitude: number;
    latitude: number;
    qqLongitude: number;
    qqLatitude: number;
    status: number;
    auditTime: Date;
    creationTime: Date;
    tenantId: number;
    tel: string;
    evaluationArry: string[];
    ticket:string;
    wechatUrl:string;
    qrUrl:string;
    fansNum:number;
    constructor(data?: IShop) {
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
            this.name = data["name"];
            this.address = data["address"];
            this.desc = data["desc"];
            this.retailerId = data["retailerId"];
            this.coverPhoto = data["coverPhoto"];
            this.saleTotal = data["saleTotal"];
            this.readTotal = data["readTotal"];
            this.evaluation = data["evaluation"];
            this.longitude = data["longitude"];
            this.latitude = data["latitude"];
            this.qqLongitude = data["qqLongitude"];
            this.qqLatitude = data["qqLatitude"];
            this.status = data["status"];
            this.auditTime = data["auditTime"];
            this.creationTime = data["creationTime"];
            this.tenantId = data["tenantId"];
            this.tel = data["tel"];
            this.singleTotal = data["singleTotal"];
            this.ticket = data["ticket"];
            this.wechatUrl = data["wechatUrl"];
            this.qrUrl = data["qrUrl"];
            this.fansNum = data["fansNum"];
            
        }
    }

    static fromJS(data: any): Shop {
        let result = new Shop();
        result.init(data);
        return result;
    }

    static fromJSArray(dataArray: any[]): Shop[] {
        let array = [];
        dataArray.forEach(result => {
            let item = new Shop();
            item.init(result);
            array.push(item);
        });

        return array;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id;
        data["name"] = this.name;
        data["address"] = this.address;
        data["desc"] = this.desc;
        data["retailerId"] = this.retailerId;
        data["coverPhoto"] = this.coverPhoto;
        data["saleTotal"] = this.saleTotal;
        data["readTotal"] = this.readTotal;
        data["evaluation"] = this.evaluation;
        data["longitude"] = this.longitude;
        data["latitude"] = this.latitude;
        data["qqLongitude"] = this.qqLongitude;
        data["qqLatitude"] = this.qqLatitude;
        data["status"] = this.status;
        data["auditTime"] = this.auditTime;
        data["creationTime"] = this.creationTime;
        data["tenantId"] = this.tenantId;
        data["tel"] = this.tel;
        data["singleTotal"] = this.singleTotal;
        data["ticket"] = this.ticket;
        data["wechatUrl"] = this.wechatUrl;
        data["qrUrl"] = this.qrUrl;
        data["fansNum"] = this.fansNum;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new Shop();
        result.init(json);
        return result;
    }
}
export interface IShop {
    id: string;
    name: string;
    address: string;
    desc: string;
    retailerId: string;
    coverPhoto: string;
    saleTotal: number;
    readTotal: number;
    singleTotal: number;
    evaluation: string;
    longitude: number;
    latitude: number;
    qqLongitude: number;
    qqLatitude: number;
    status: number;
    auditTime: Date;
    creationTime: Date;
    tenantId: number;
    tel: string;
    ticket:string;
    wechatUrl:string;
    qrUrl:string;
    fansNum:number;
}

export class NearbyShop implements INearbyShop {
    id: string;
    name: string;
    address: string;
    coverPhoto: string;
    longitude: number;
    latitude: number;
    qqLongitude: number;
    qqLatitude: number;
    tel: string;
    distance: number;

    constructor(data?: INearbyShop) {
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
            this.name = data["name"];
            this.address = data["address"];
            this.coverPhoto = data["coverPhoto"];
            this.longitude = data["longitude"];
            this.latitude = data["latitude"];
            this.qqLongitude = data["qqLongitude"];
            this.qqLatitude = data["qqLatitude"];
            this.tel = data["tel"];
            this.distance = data["distance"];
        }
    }

    static fromJS(data: any): NearbyShop {
        let result = new NearbyShop();
        result.init(data);
        return result;
    }

    static fromJSArray(dataArray: any[]): NearbyShop[] {
        let array = [];
        dataArray.forEach(result => {
            let item = new NearbyShop();
            item.init(result);
            array.push(item);
        });

        return array;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id;
        data["name"] = this.name;
        data["address"] = this.address;
        data["coverPhoto"] = this.coverPhoto;
        data["longitude"] = this.longitude;
        data["latitude"] = this.latitude;
        data["qqLongitude"] = this.qqLongitude;
        data["qqLatitude"] = this.qqLatitude;
        data["tel"] = this.tel;
        data["distance"] = this.distance;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new NearbyShop();
        result.init(json);
        return result;
    }
}
export interface INearbyShop {
    id: string;
    name: string;
    address: string;
    coverPhoto: string;
    longitude: number;
    latitude: number;
    qqLongitude: number;
    qqLatitude: number;
    tel: string;
    distance: number;
}

