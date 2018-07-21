export class IntegralDetails implements IIntegralDetails {
    id: string;
    openId: string;
    initialIntegral: number;
    integral: number;
    finalIntegral: number;
    type: number;
    desc: string;
    refId: string;
    creationTime: Date;
    tenantId: number;
    wxName: string;
    typeName: string;
    userTypeName: string;
    memberBarCode: string;
    phone: string;
    nickName: string;
    integralTotal: number;
    code: string;
    constructor(data?: IIntegralDetails) {
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
            this.openId = data["openId"];
            this.initialIntegral = data["initialIntegral"];
            this.integral = data["integral"];
            this.finalIntegral = data["finalIntegral"];
            this.type = data["type"];
            this.desc = data["desc"];
            this.refId = data["refId"];
            this.creationTime = data["creationTime"];
            this.tenantId = data["tenantId"];
            this.wxName = data["wxName"];
            this.typeName = data["typeName"];
            this.userTypeName = data["userTypeName"];
            this.memberBarCode = data["memberBarCode"];
            this.phone = data["phone"];
            this.nickName = data["nickName"];
            this.integralTotal = data["integralTotal"];
            this.code = data["code"];
        }
    }

    static fromJS(data: any): IntegralDetails {
        let result = new IntegralDetails();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id;
        data["openId"] = this.openId;
        data["initialIntegral"] = this.initialIntegral;
        data["integral"] = this.integral;
        data["finalIntegral"] = this.finalIntegral;
        data["type"] = this.type;
        data["desc"] = this.desc;
        data["refId"] = this.refId;
        data["creationTime"] = this.creationTime;
        data["tenantId"] = this.tenantId;
        data["wxName"] = this.wxName;
        data["phone"] = this.phone;
        data["integralTotal"] = this.integralTotal;
        data["code"] = this.code;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new IntegralDetails();
        result.init(json);
        return result;
    }
}
export interface IIntegralDetails {
    id: string;
    openId: string;
    initialIntegral: number;
    integral: number;
    finalIntegral: number;
    type: number;
    desc: string;
    refId: string;
    creationTime: Date;
    tenantId: number;
    integralTotal: number;
    code: string;
}