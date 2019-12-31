//查询个人签到状态返回
export class GetLuckySignInfoDto {
    name:string;
    code: string;
    lotteryState:boolean;
    deptName:string;
    constructor(data?: IGetLuckySignInfoDto) { 
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.name = data["name"];
            this.code = data["code"];
            this.lotteryState = data["lotteryState"];
            this.deptName = data["deptName"];
        }
    }

    static fromJS(data: any): GetLuckySignInfoDto {
        let result = new GetLuckySignInfoDto();
        result.init(data);
        return result;
    }

    static fromJSArray(dataArray: any[]): GetLuckySignInfoDto[] {
        let array = [];
        dataArray.forEach(result => {
            let item = new GetLuckySignInfoDto();
            item.init(result);
            array.push(item);
        });
        return array;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["name"] = this.name;
        data["code"] = this.code;
        data["lotteryState"] = this.lotteryState;
        data["deptName"]=this.deptName;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new GetLuckySignInfoDto();
        result.init(json);
        return result;
    }
}
export interface IGetLuckySignInfoDto {
   
    name: string;
    code:string;
    lotteryState:boolean;
    deptName:string;
}
