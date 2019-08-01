export class DemandDetail implements IDemandDetail {
    id: string;
    demandForecastId: string;
    name: string;
    type: number;
    wholesalePrice: number;
    suggestPrice: number;
    isAlien: boolean;
    lastMonthNum: number;
    yearOnYear: number;
    retailerName: string;
    retailerCode: string;
    predictiveValue: number;
    constructor(data?: IDemandDetail) {
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
            this.demandForecastId = data["demandForecastId"];
            this.name = data["name"];
            this.type = data["type"];
            this.wholesalePrice = data["wholesalePrice"];
            this.suggestPrice = data["suggestPrice"];
            this.isAlien = data["isAlien"];
            this.lastMonthNum = data["lastMonthNum"];
            this.yearOnYear = data["yearOnYear"];
            this.retailerName = data["retailerName"];
            this.retailerCode = data["retailerCode"];
            this.predictiveValue = data["predictiveValue"];
        }
    }

    static fromJS(data: any): DemandDetail {
        let result = new DemandDetail();
        result.init(data);
        return result;
    }

    static fromJSArray(dataArray: any[]): DemandDetail[] {
        let array = [];
        dataArray.forEach(result => {
            let item = new DemandDetail();
            item.init(result);
            array.push(item);
        });
        return array;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id;
        data["demandForecastId"] = this.demandForecastId;
        data["name"] = this.name;
        data["type"] = this.type;
        data["wholesalePrice"] = this.wholesalePrice;
        data["suggestPrice"] = this.suggestPrice;
        data["isAlien"] = this.isAlien;
        data["lastMonthNum"] = this.lastMonthNum;
        data["yearOnYear"] = this.yearOnYear;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new DemandDetail();
        result.init(json);
        return result;
    }
}
export interface IDemandDetail {
    id: string;
    demandForecastId: string;
    name: string;
    type: number;
    wholesalePrice: number;
    suggestPrice: number;
    isAlien: boolean;
    lastMonthNum: number;
    yearOnYear: number;
    retailerName: string;
    retailerCode: string;
    predictiveValue: number;
}