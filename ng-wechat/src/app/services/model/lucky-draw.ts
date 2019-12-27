export class LuckyDraw implements ILuckyDraw {

    id:string;
    creationTime: Date;
    isPublish:boolean;
    lotteryState:boolean;
    name:string;
    beginTime:Date;
    endTime:Date;
    constructor(data?: ILuckyDraw) {
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
            this.isPublish = data["isPublish"];
            this.lotteryState = data["lotteryState"];
            this.name = data["name"];
            this.beginTime = data["beginTime"];
            this.creationTime = data["creationTime"];
            this.endTime = data["endTime"];
        }
    }

    static fromJS(data: any): LuckyDraw {
        let result = new LuckyDraw();
        result.init(data);
        return result;
    }

    static fromJSArray(dataArray: any[]): LuckyDraw[] {
        let array = [];
        dataArray.forEach(result => {
            let item = new LuckyDraw();
            item.init(result);
            array.push(item);
        });
        return array;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id;
        data["isPublish"] = this.isPublish;
        data["lotteryState"] = this.lotteryState;
        data["name"] = this.name;
        data["beginTime"] = this.beginTime;
        data["creationTime"] = this.creationTime;
        data["endTime"] = this.endTime;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new LuckyDraw();
        result.init(json);
        return result;
    }
}
export interface ILuckyDraw {
   
    name: string;
    id:string;
    creationTime: Date;
    isPublish:boolean;
    beginTime:Date;
    endTime:Date;
    lotteryState:boolean;
}