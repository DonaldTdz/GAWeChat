export class QuestionOptions implements IQuestionOptions {
    id: string;
    questionnaireId: string;
    value: string;
    desc: string;
    isChecked: boolean = false;
    constructor(data?: IQuestionOptions) {
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
            this.value = data["value"];
            this.desc = data["desc"];
            this.isChecked = data["isChecked"];
        }
    }

    static fromJS(data: any): QuestionOptions {
        let result = new QuestionOptions();
        result.init(data);
        return result;
    }

    static fromJSArray(data: any[]): QuestionOptions[] {
        let arry = []
        data.map(i => {
            let item = new QuestionOptions();
            item.init(i);
            arry.push(item);
        })
        return arry;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id;
        data["questionnaireId"] = this.questionnaireId;
        data["value"] = this.value;
        data["desc"] = this.desc;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new QuestionOptions();
        result.init(json);
        return result;
    }
}
export interface IQuestionOptions {
    id: string;
    questionnaireId: string;
    value: string;
    desc: string;
    isChecked: boolean
}