export class RetailCustomer implements IRetailCustomer {
    code: string;
    name: string;
    businessAddress: string;
    archivalLevel: string;
    orderCycle: string;
    storeType: string;
    telephone: string;
    isAction: boolean;
    branchCompany: string;
    department: string;
    employeeId: string;
    manager: string;
    orderMode: number;
    terminalType: number;
    businessType: string;
    scale: number;
    marketType: number;
    deliveryLine: string;
    tenantId: number;
    licenseKey: string;
    id: string;
    orderModeName: string;
    terminalTypeName: string;
    scaleName: string;
    marketTypeName: string;
    activeText: string;
    activeType: string;
    isDeleted: boolean;
    deleterUserId: number;
    deletionTime: Date;
    lastModificationTime: Date;
    lastModifierUserId: number;
    creationTime: Date;
    creatorUserId: number;
    verificationCode: string;
    retailerVerificationCode: string;

    custId: string;
    disLineCode: string;
    category: string;
    departmentId: string;
    slsmanId: string;
    slsmanName: string;

    organization: string;
    shopTotal: number;
    custIntegral: number;
    retailerIntegral: number;
    scanFrequency: number;
    scanQuantity: number;
    priceTotal: number;

    constructor(data?: IRetailCustomer) {
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
            this.code = data["code"];
            this.businessAddress = data["businessAddress"];
            this.archivalLevel = data["archivalLevel"];
            this.orderCycle = data["orderCycle"];
            this.storeType = data["storeType"];
            this.telephone = data["telephone"];
            this.isAction = data["isAction"];
            this.tenantId = data["tenantId"];
            this.branchCompany = data["branchCompany"];
            this.department = data["department"];
            this.employeeId = data["employeeId"];
            this.manager = data["manager"];
            this.orderMode = data["orderMode"];
            this.terminalType = data["terminalType"];
            this.businessType = data["businessType"];
            this.scale = data["scale"];
            this.marketType = data["marketType"];
            this.deliveryLine = data["deliveryLine"];
            this.licenseKey = data["licenseKey"];
            this.orderModeName = data["orderModeName"];
            this.terminalTypeName = data["terminalTypeName"];
            this.scaleName = data["scaleName"];
            this.marketTypeName = data["marketTypeName"];
            this.isDeleted = data["isDeleted"];
            this.creationTime = data["creationTime"];
            this.creatorUserId = data["creatorUserId"];
            this.lastModificationTime = data["lastModificationTime"];
            this.lastModifierUserId = data["lastModifierUserId"];
            this.deletionTime = data["deletionTime"];
            this.deleterUserId = data["deleterUserId"];
            this.verificationCode = data["verificationCode"];
            this.retailerVerificationCode = data["retailerVerificationCode"];
            this.custId = data["custId"];
            this.disLineCode = data["disLineCode"];
            this.category = data["category"];
            this.departmentId = data["departmentId"];
            this.slsmanId = data["slsmanId"];
            this.slsmanName = data["slsmanName"];

            this.organization = data["organization"];
            this.shopTotal = data["shopTotal"];
            this.custIntegral = data["custIntegral"];
            this.retailerIntegral = data["retailerIntegral"];
            this.scanFrequency = data["scanFrequency"];
            this.scanQuantity = data["scanQuantity"];
            this.priceTotal = data["priceTotal"];
        }
    }

    static fromJS(data: any): RetailCustomer {
        let result = new RetailCustomer();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id;
        data["code"] = this.code;
        data["name"] = this.name;
        data["businessAddress"] = this.businessAddress;
        data["archivalLevel"] = this.archivalLevel;
        data["orderCycle"] = this.orderCycle;
        data["storeType"] = this.storeType;
        data["telephone"] = this.telephone;
        data["isAction"] = this.isAction;
        data["branchCompany"] = this.branchCompany;
        data["department"] = this.department;
        data["employeeId"] = this.employeeId;
        data["manager"] = this.manager;
        data["orderMode"] = this.orderMode;
        data["terminalType"] = this.terminalType;
        data["businessType"] = this.businessType;
        data["scale"] = this.scale;
        data["marketType"] = this.marketType;
        data["deliveryLine"] = this.deliveryLine;
        data["tenantId"] = this.tenantId;
        data["employeeId"] = this.employeeId;
        data["licenseKey"] = this.licenseKey;
        data["isDeleted"] = this.isDeleted;
        data["creationTime"] = this.creationTime;
        data["creatorUserId"] = this.creatorUserId;
        data["deletionTime"] = this.deletionTime;
        data["deleterUserId"] = this.deleterUserId;
        data["custId"] = this.custId;
        data["disLineCode"] = this.disLineCode;
        data["category"] = this.category;
        data["departmentId"] = this.departmentId;
        data["slsmanId"] = this.slsmanId;
        data["slsmanName"] = this.slsmanName;
        data["organization"] = this.organization;
        data["shopTotal"] = this.shopTotal;
        data["custIntegral"] = this.custIntegral;
        data["retailerIntegral"] = this.retailerIntegral;
        data["scanFrequency"] = this.scanFrequency;
        data["scanQuantity"] = this.scanQuantity;
        data["priceTotal"] = this.priceTotal;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new RetailCustomer();
        result.init(json);
        return result;
    }
}

export interface IRetailCustomer {
    code: string;
    name: string;
    businessAddress: string;
    archivalLevel: string;
    orderCycle: string;
    storeType: string;
    telephone: string;
    isAction: boolean;
    branchCompany: string;
    department: string;
    employeeId: string;
    manager: string;
    orderMode: number;
    terminalType: number;
    businessType: string;
    scale: number;
    marketType: number;
    deliveryLine: string;
    tenantId: number;
    licenseKey: string;
    id: string;
    verificationCode: string;
    retailerVerificationCode: string;
    custId: string;
    disLineCode: string;
    category: string;
    departmentId: string;
    slsmanId: string;
    slsmanName: string;

    organization: string;
    shopTotal: number;
    custIntegral: number;
    retailerIntegral: number;
    scanFrequency: number;
    scanQuantity: number;
    priceTotal: number;
}