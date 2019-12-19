export class QuestionRecord implements IQuestionRecord {
    id: string;
    title: string;
    status: string;
    constructor(data?: IQuestionRecord) {
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
            this.status = data["status"];
        }
    }

    static fromJS(data: any): QuestionRecord {
        let result = new QuestionRecord();
        result.init(data);
        return result;
    }

    static fromJSArray(data: any[]): QuestionRecord[] {
        let arry = []
        data.map(i => {
            let item = new QuestionRecord();
            item.init(i);
            arry.push(item);
        })
        return arry;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id;
        data["title"] = this.title;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new QuestionRecord();
        result.init(json);
        return result;
    }
}
export interface IQuestionRecord {
    id: string;
    title: string;
    status: string;
}