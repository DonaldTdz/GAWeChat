export class ForecastRecordDto implements IForecastRecordDto {
    demandDetailId: string;
    predictiveValue: number;

    constructor(data?: IForecastRecordDto) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.demandDetailId = data["id"];
            this.predictiveValue = data["predictiveValue"];
        }
    }
    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["demandDetailId"] = this.demandDetailId;
        data["predictiveValue"] = this.predictiveValue;
        return data;
    }

    static fromJSArray(dataArray: any[]): ForecastRecordDto[] {
        let array = [];
        dataArray.forEach(result => {
            let item = new ForecastRecordDto();
            item.init(result);
            array.push(item);
        });
        return array;
    }
}
export interface IForecastRecordDto {
    demandDetailId: string;
    predictiveValue: number;
}