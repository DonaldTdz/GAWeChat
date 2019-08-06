export class AnswerRecords implements IAnswerRecords {
    id: string;
    questionnaireId: string;
    values: string;
    remark: string;
    openId: string;
    constructor(data?: IAnswerRecords) {
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
            this.questionnaireId = data["questionnaireId"];
            this.values = data["values"];
            this.remark = data["remark"];
            this.openId = data["openId"];
        }
    }

    static fromJS(data: any): AnswerRecords {
        let result = new AnswerRecords();
        result.init(data);
        return result;
    }

    static fromJSArray(data: any[]): AnswerRecords[] {
        let arry = []
        data.map(i => {
            let item = new AnswerRecords();
            item.init(i);
            arry.push(item);
        })
        return arry;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id;
        data["questionnaireId"] = this.questionnaireId;
        data["values"] = this.values;
        data["remark"] = this.remark;
        data["openId"] = this.openId;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new AnswerRecords();
        result.init(json);
        return result;
    }
}
export interface IAnswerRecords {
    id: string;
    questionnaireId: string;
    values: string;
    remark: string;
    openId: string;
}


export class QuestionAnswer implements IQuestionAnswer {
    questionnaireId: string;
    values: string;
    remark: string;
    constructor(data?: IQuestionAnswer) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.questionnaireId = data["id"];
            this.values = data["value"];
            this.remark = data["remark"];
        }
    }

    static fromJSArray(data: any[]): QuestionAnswer[] {
        let arry = []
        data.map(i => {
            let item = new QuestionAnswer();
            item.init(i);
            arry.push(item);
        })
        return arry;
    }
}
export interface IQuestionAnswer {
    questionnaireId: string;
    values: string;
    remark: string;
}