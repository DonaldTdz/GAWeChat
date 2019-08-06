export class QuestionRecord implements IQuestionRecord {
    id: string;
    title:string;
    year:string;
    quarter: number;
    isPublish: boolean;
    creationTime:Date;
    creatorUserId:string;
    publishTime:Date;
    quarterString:string;
    writeTime:Date;
    userName:string;
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
            this.year = data["year"];
            this.quarter = data["quarter"];
            this.isPublish = data["isPublish"];
            this.creationTime = data["creationTime"];
            this.creatorUserId = data["creatorUserId"];
            this.publishTime = data["publishTime"];
            this.quarterString = data["quarterString"];
            this.writeTime = data["writeTime"];
            this.userName = data["userName"];
        }
    }

    static fromJS(data: any): QuestionRecord {
        let result = new QuestionRecord();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id;
        data["title"] = this.title;
        data["year"] = this.year;
        data["quarter"] = this.quarter;
        data["isPublish"] = this.isPublish;
        data["creationTime"] = this.creationTime;
        data["creatorUserId"] = this.creatorUserId;
        data["publishTime"] = this.publishTime;
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
    title:string;
    year:string;
    quarter: number;
    isPublish: boolean;
    creationTime:Date;
    creatorUserId:string;
    publishTime:Date;
    quarterString:string;
    writeTime:Date;
    userName:string;
}