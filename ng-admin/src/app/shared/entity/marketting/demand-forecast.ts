export class DemandForecast implements IDemandForecast {
    id: string;
    title: string;
    month: string;
    creationTime: Date;
    creatorUserId: number;
    isPublish: boolean;
    publishTime: Date;
    constructor(data?: IDemandForecast) {
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
            this.title = data["title"];
            this.month = data["month"];
            this.creationTime = data["creationTime"];
            this.creatorUserId = data["creatorUserId"];
            this.isPublish = data["isPublish"];
            this.publishTime = data["publishTime"];
        }
    }

    static fromJS(data: any): DemandForecast {
        let result = new DemandForecast();
        result.init(data);
        return result;
    }

    static fromJSArray(dataArray: any[]): DemandForecast[] {
        let array = [];
        dataArray.forEach(result => {
            let item = new DemandForecast();
            item.init(result);
            array.push(item);
        });
        return array;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id;
        data["title"] = this.title;
        data["month"] = this.month;
        data["creationTime"] = this.creationTime;
        data["creatorUserId"] = this.creatorUserId;
        data["isPublish"] = this.isPublish;
        data["publishTime"] = this.publishTime;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new DemandForecast();
        result.init(json);
        return result;
    }
}
export interface IDemandForecast {
    id: string;
    title: string;
    month: string;
    creationTime: Date;
    creatorUserId: number;
    isPublish: boolean;
    publishTime: Date;
}