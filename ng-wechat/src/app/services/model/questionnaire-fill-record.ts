export class QuestionnaireFillRecords implements IQuestionnaireFillRecords {
    quarter:number;
    status:string;
    desc:string;
    constructor(data?: IQuestionnaireFillRecords) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.quarter = data["quarter"];
            this.status = data["status"];
            this.desc = data["desc"];
        }
    }

    static fromJS(data: any): QuestionnaireFillRecords {
        let result = new QuestionnaireFillRecords();
        result.init(data);
        return result;
    }

    static fromJSArray(data: any[]): QuestionnaireFillRecords[] {
        let arry = []
        data.map(i => {
            let item = new QuestionnaireFillRecords();
            item.init(i);
            arry.push(item);
        })
        return arry;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["quarter"] = this.quarter;
        data["status"] = this.status;
        data["desc"] = this.desc;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new QuestionnaireFillRecords();
        result.init(json);
        return result;
    }
}
export interface IQuestionnaireFillRecords {
    quarter:number;
    status:string;
    desc:string;
}