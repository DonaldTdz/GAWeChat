import 'rxjs/add/observable/fromPromise';
import 'rxjs/add/observable/of';
import 'rxjs/add/observable/throw';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/toPromise';
import 'rxjs/add/operator/mergeMap';
import 'rxjs/add/operator/catch';

import { Injectable } from '@angular/core';
import { HttpClient } from '../httpclient';
import { Observable } from 'rxjs';
import { Questionnaire, QuestionOptions, AnswerRecords, ApiResult, QuestionRecord } from '../model/index';
import { LuckyDraw, WXLuckyDrawDetailIDOutput, LotteryJoinDeptDetailOutput } from '../model/lucky-draw';


@Injectable()
export class LotteryService {
    constructor(private http: HttpClient) { }
    //创建新的活动
    CreateWXLuckyDrawAsync(input: any): Observable<ApiResult<any>> {
        return this.http.post('/api/services/app/LuckyDraw/CreateWXLuckyDrawAsync', input).map(data => {
            let result = new ApiResult<any>();
            result = data.result;
            return result;
        });
    }

    //更新一个活动
    updateWXLuckyDrawAsync(input: any): Observable<ApiResult<any>> {
        return this.http.post('/api/services/app/LuckyDraw/RenewWXLuckyDrawAsync', input).map(data => {
            let result = new ApiResult<any>();
            result = data.result;
            return result;
        });
    }
    //获得活动列表
    getWXLuckyDrawListAsync(): Observable<LuckyDraw[]> {
        return this.http.get('/api/services/app/LuckyDraw/GetWXLuckyDrawListAsync').map(data => {
            return data.result;
        });
    }


    updateWXLuckyDrawPubStatusAsync(input: any): Observable<ApiResult<any>> {
        return this.http.post('/api/services/app/LuckyDraw/ChangeWXLuckyDrawPubStatusAsync', input).map(data => {
            let result = new ApiResult<any>();
            result = data.result;
            return result;
        });
    }
    //获取详细的抽奖部门列表信息
    getEmployeeNameListAsyn(): Observable<any[]> {
        return this.http.get('/api/services/app/Employee/GetEmployeeNameListAsyn').map(data => {
            if (data.result) {
                return data.result;
            } else {
                return false;
            }
        });
    }

    //获取详细签到列表
    getSignListByDeptNameAsync(deptName: string): Observable<any[]> {
        return this.http.get('/api/services/app/Employee/GetSignListByDeptNameAsync?deptName=' + deptName).map(data => {
            if (data.result) {
                return data.result;
            } else {
                return false;
            }
        });
    }


    //admin 根据管理员获得活动详情
    getLuckyDrawDetailByIdAsync(Id: string, openId: string): Observable<WXLuckyDrawDetailIDOutput> {
        return this.http.get('/api/services/app/LuckyDraw/GetLuckyDrawDetailByIdAsync?Id=' + Id + '&openId=' + openId).map(data => {
            if (data.result) {
                return data.result;
            } else {
                return false;
            }
        });
    }
    //内部员工获取活动列表
    getWXLuckyDrawListPublishedAsync(): Observable<LuckyDraw[]> {
        return this.http.get('/api/services/app/LuckyDraw/GetWXLuckyDrawListPublishedAsync').map(data => {
            if (data.result) {
                return data.result;
            } else {
                return false;
            }
        });
    }

    //个人获取签到信息
    getLuckySignInfoAsync(id: string): Observable<ApiResult<any>> {
        return this.http.get('/api/services/app/LuckySign/GetLuckySignInfoAsync?openId=' + id).map(data => {
            let result = new ApiResult<any>();
            result = data.result;
            return result;
        });
    }

    //获取唯一活动唯一部门的人员签到情况
    getLotteryJoinDeptDetailAsync(id: string, deptName: string): Observable<LotteryJoinDeptDetailOutput[]> {
        return this.http.get('/api/services/app/LuckyDraw/GetLotteryJoinDeptDetailAsync?Id=' + id + '&DeptName=' + deptName).map(data => {
            if (data.result) {
                return data.result;
            } else {
                return false;
            }
        });
    }


    //获取签到人数和未签到人数
    getSignInPeronNumAsync(): Observable<ApiResult<any>> {
        return this.http.get('/api/services/app/LuckySign/GetSignInPeronNumAsync').map(data => {
            let result = new ApiResult<any>();
            result = data.result;
            return result;
        });
    }

    //活动发布逻辑
    getLotteryLogicAsync(id: string): Observable<ApiResult<any>> {
        return this.http.get('/api/services/app/LotteryDetail/GetLotteryLogicAsync?Id=' + id).map(data => {
            let result = new ApiResult<any>();
            result = data.result;
            return result;
        });
    }

    //微信端进行签到
    getCreateWXLuckyDrawAsync(id: string): Observable<ApiResult<any>> {
        return this.http.get('/api/services/app/LuckySign/GetCreateWXLuckyDrawAsync?openId=' + id).map(data => {
            let result = new ApiResult<any>();
            result = data.result;
            return result;
        });
    }

    //获取参与活动的人数
    getLuckyDrawPersonCountAsync(id: string): Observable<any> {
        return this.http.get('/api/services/app/LuckyDraw/GetLuckyDrawPersonCountAsync?luckyId=' + id).map(data => {
            return data.result;
        });
    }



    //分部门显示各个部门参加抽奖的人数
    getLuckyDeptmentLotteryPersonAsync(id: string): Observable<any> {
        return this.http.get('/api/services/app/LuckyDraw/GetLuckyDeptmentLotteryPersonAsync?luckyId=' + id).map(data => {
            return data.result;
        });
    }

    getAuthorizationUrl(host: string): Observable<string> {
        return this.http.get('/api/services/app/LuckyDraw/GetAuthorizationUrl?host' + host).map(data => {
            if (data.result) {
                return data.result;
            } else {
                return null;
            }
        });
    }


    //查询工号信息
    getJobNumberInfoAsync(jobNumber: string): Observable<ApiResult<any>> {
        return this.http.get('/api/services/app/Employee/GetJobNumberInfoAsync?jobNumber=' + jobNumber).map(data => {
            let result = new ApiResult<any>();
            result = data.result;
            return result;
        });
    }

    //绑定工号
    bindJobNumberAync(openId: string, jobNumber: string): Observable<ApiResult<any>> {
        return this.http.get('/api/services/app/Employee/BindJobNumberAync?openId=' + openId + '&jobNumber=' + jobNumber).map(data => {
            let result = new ApiResult<any>();
            result = data.result;
            return result;
        });
    }
}
