export class Exhibition implements IExhibition {
    id: string;
    beginTime: string;
    endTime: string;
    content: string;
    desc: string;
    topTotal: number;
    frequency: number;
    constructor(data?: IExhibition) {
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
            this.beginTime = data["beginTime"];
            this.endTime = data["endTime"];
            this.content = data["content"];
            this.desc = data["desc"];
            this.topTotal = data["topTotal"];
            this.frequency = data["frequency"];
        }
    }

    static fromJS(data: any): Exhibition {
        let result = new Exhibition();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id;
        data["beginTime"] = this.beginTime;
        data["endTime"] = this.endTime;
        data["content"] = this.content;
        data["desc"] = this.desc;
        data["topTotal"] = this.topTotal;
        data["frequency"] = this.frequency;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new Exhibition();
        result.init(json);
        return result;
    }
}
export interface IExhibition {
    id: string;
    beginTime: string;
    endTime: string;
    content: string;
    desc: string;
    topTotal: number;
    frequency: number;
}