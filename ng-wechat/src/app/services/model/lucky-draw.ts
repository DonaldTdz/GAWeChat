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

//抽奖情况详细信息
export class WXLuckyDrawDetailIDOutput {

    name:string;
    beginTime: Date;
    endTime:Date;
    isPublish:boolean;
    lotteryState:Date;
    list:WeiXinPriceInput[];
    lotteryDetails:LotteryDetailDto[];
    constructor(data?: ILotteryDetailDto) { 
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.list = data["list"];
            this.isPublish = data["isPublish"];
            this.lotteryState = data["lotteryState"];
            this.name = data["name"];
            this.beginTime = data["beginTime"];
            this.endTime = data["endTime"];
            this.lotteryDetails=data["lotteryDetails"];
        }
    }

    static fromJS(data: any): LotteryDetailDto {
        let result = new LotteryDetailDto();
        result.init(data);
        return result;
    }

    static fromJSArray(dataArray: any[]): LotteryDetailDto[] {
        let array = [];
        dataArray.forEach(result => {
            let item = new LotteryDetailDto();
            item.init(result);
            array.push(item);
        });
        return array;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["list"] = this.list;
        data["isPublish"] = this.isPublish;
        data["lotteryState"] = this.lotteryState;
        data["name"] = this.name;
        data["beginTime"] = this.beginTime;
        data["endTime"] = this.endTime;
        data["lotteryDetails"]=this.lotteryDetails;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new LotteryDetailDto();
        result.init(json);
        return result;
    }
}
export interface WXLuckyDrawDetailIDOutput{
    name:string;
    beginTime: Date;
    endTime:Date;
    isPublish:boolean;
    lotteryState:Date;
    list:WeiXinPriceInput[];
} 

//奖品等级枚举
export enum PrizeType{
    一等奖 = 1,
    二等奖 = 2,
    三等奖 = 3,
    四等奖 = 4,
    安慰奖 = 5,
    参与奖 = 6
}

//输出子类
export class WeiXinPriceInput {

    name:string;
    num: number;
    type:PrizeType;
    constructor(data?: IWeiXinPriceInput) { 
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
            this.num = data["num"];
            this.type = data["type"];
        }
    }

    static fromJS(data: any): WeiXinPriceInput {
        let result = new WeiXinPriceInput();
        result.init(data);
        return result;
    }

    static fromJSArray(dataArray: any[]): WeiXinPriceInput[] {
        let array = [];
        dataArray.forEach(result => {
            let item = new WeiXinPriceInput();
            item.init(result);
            array.push(item);
        });
        return array;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["name"] = this.name;
        data["num"] = this.num;
        data["type"] = this.type;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new LuckyDraw();
        result.init(json);
        return result;
    }
}

export interface IWeiXinPriceInput {
   
    name:string;
    num: number;
    type:PrizeType;
}

export class LotteryDetailDto {
    name:string;
    num: number;
    prizeName:string;
    constructor(data?: ILotteryDetailDto) { 
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
            this.num = data["num"];
            this.prizeName = data["prizeName"];
        }
    }

    static fromJS(data: any): LotteryDetailDto {
        let result = new LotteryDetailDto();
        result.init(data);
        return result;
    }

    static fromJSArray(dataArray: any[]): LotteryDetailDto[] {
        let array = [];
        dataArray.forEach(result => {
            let item = new LotteryDetailDto();
            item.init(result);
            array.push(item);
        });
        return array;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["name"] = this.name;
        data["num"] = this.num;
        data["prizeName"] = this.prizeName;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new LotteryDetailDto();
        result.init(json);
        return result;
    }
}
export interface ILotteryDetailDto {
   
    name: string;
    num:number;
    prizeName: string;
}

export class LotteryJoinDeptDetailOutput {
    name:string;
    code: string;
    isJoin:boolean;
    constructor(data?: ILotteryJoinDeptDetailOutput) { 
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
            this.isJoin = data["isJoin"];
        }
    }

    static fromJS(data: any): LotteryJoinDeptDetailOutput {
        let result = new LotteryJoinDeptDetailOutput();
        result.init(data);
        return result;
    }

    static fromJSArray(dataArray: any[]): LotteryJoinDeptDetailOutput[] {
        let array = [];
        dataArray.forEach(result => {
            let item = new LotteryJoinDeptDetailOutput();
            item.init(result);
            array.push(item);
        });
        return array;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["name"] = this.name;
        data["code"] = this.code;
        data["isJoin"] = this.isJoin;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new LotteryJoinDeptDetailOutput();
        result.init(json);
        return result;
    }
}
export interface ILotteryJoinDeptDetailOutput {
   
    name: string;
    code:string;
    isJoin: boolean;
}

export class GetEmployeeDetailByDeptOutput {
    name:string;
    code: string;
    isSign:boolean;
    constructor(data?: IGetEmployeeDetailByDeptOutput) { 
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
            this.isSign = data["isSign"];
        }
    }

    static fromJS(data: any): GetEmployeeDetailByDeptOutput {
        let result = new GetEmployeeDetailByDeptOutput();
        result.init(data);
        return result;
    }

    static fromJSArray(dataArray: any[]): GetEmployeeDetailByDeptOutput[] {
        let array = [];
        dataArray.forEach(result => {
            let item = new GetEmployeeDetailByDeptOutput();
            item.init(result);
            array.push(item);
        });
        return array;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["name"] = this.name;
        data["code"] = this.code;
        data["isSign"] = this.isSign;
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new GetEmployeeDetailByDeptOutput();
        result.init(json);
        return result;
    }
}
export interface IGetEmployeeDetailByDeptOutput {
   
    name: string;
    code:string;
    isSign: boolean;
}

