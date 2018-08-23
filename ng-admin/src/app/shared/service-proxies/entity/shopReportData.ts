export class ShopReportData implements IShopReportData {
    rootId: number;
    companyId: number;
    areaId: number;
    slsmanNameId: number;
    groupNum: number;
    organization: string;
    shopTotal: number;
    scanQuantity: number;
    scanFrequency: number;
    priceTotal: number;
    custIntegral: number;
    retailerIntegral: number;
    creationTime: Date;
    specification: string;
    constructor(data?: IShopReportData) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.rootId = data["rootId"];
            this.companyId = data["companyId"];
            this.areaId = data["areaId"];
            this.slsmanNameId = data["slsmanNameId"];
            this.groupNum = data["groupNum"];
            this.organization = data["organization"];
            this.shopTotal = data["shopTotal"];
            this.scanQuantity = data["scanQuantity"];
            this.scanFrequency = data["scanFrequency"];
            this.priceTotal = data["priceTotal"];
            this.custIntegral = data["custIntegral"];
            this.retailerIntegral = data["retailerIntegral"];
            this.creationTime = data["creationTime"];
            this.specification = data["specification"];
        }
    }

    static fromJS(data: any): ShopReportData {
        let result = new ShopReportData();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["rootId"] = this.rootId;
        data["companyId"] = this.companyId;
        data["areaId"] = this.areaId;
        data["slsmanNameId"] = this.slsmanNameId;
        data["groupNum"] = this.groupNum;
        data["organization"] = this.organization;
        data["shopTotal"] = this.shopTotal;
        data["scanQuantity"] = this.scanQuantity;
        data["scanFrequency"] = this.scanFrequency;
        data["priceTotal"] = this.priceTotal;
        data["custIntegral"] = this.custIntegral;
        data["retailerIntegral"] = this.retailerIntegral;
        data["creationTime"] = this.creationTime;
        data["specification"] = this.specification;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new ShopReportData();
        result.init(json);
        return result;
    }
}
export interface IShopReportData {
    rootId: number;
    companyId: number;
    areaId: number;
    slsmanNameId: number;
    groupNum: number;
    organization: string;
    shopTotal: number;
    scanQuantity: number;
    scanFrequency: number;
    priceTotal: number;
    custIntegral: number;
    retailerIntegral: number;
    creationTime: Date;
    specification: string;
}