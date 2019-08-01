export class Questionnaire implements IQuestionnaire {
    id: string;
    type: number;
    isMultiple: boolean;
    no:string;
    question:string;
    typeName:string;
    constructor(data?: IQuestionnaire) {
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
            this.type = data["type"];
            this.isMultiple = data["isMultiple"];
            this.no = data["no"];
            this.question = data["question"];
            this.typeName = data["typeName"];
        }
    }

    static fromJS(data: any): Questionnaire {
        let result = new Questionnaire();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id;
        data["type"] = this.type;
        data["isMultiple"] = this.isMultiple;
        data["no"] = this.no;
        data["question"] = this.question;
        data["typeName"] = this.typeName;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new Questionnaire();
        result.init(json);
        return result;
    }
}
export interface IQuestionnaire {
    id: string;
    type: number;
    isMultiple: boolean;
    no:string;
    question:string;
    typeName:string;
}