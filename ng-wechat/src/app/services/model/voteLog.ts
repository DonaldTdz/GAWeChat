export class VoteLog
    implements IVoteLog {
    id: string;
    createTime: Date;
    openId: string;
    userName: string;
    exhibitionId: string;
    constructor(data?: IVoteLog) {
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
            this.createTime = data["createTime"];
            this.openId = data["openId"];
            this.userName = data["userName"];
            this.exhibitionId = data["exhibitionId"];
        }
    }

    static fromJS(data: any): VoteLog {
        let result = new VoteLog
            ();
        result.init(data);
        return result;
    }

    static fromJSArray(dataArray: any[]): VoteLog[] {
        let array = [];
        dataArray.forEach(result => {
            let item = new VoteLog();
            item.init(result);
            array.push(item);
        });
        return array;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id;
        data["createTime"] = this.createTime;
        data["openId"] = this.openId;
        data["userName"] = this.userName;
        data["exhibitionId"] = this.exhibitionId;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new VoteLog();
        result.init(json);
        return result;
    }
}
export interface IVoteLog {
    id: string;
    createTime: Date;
    openId: string;
    userName: string;
    exhibitionId: string;
}