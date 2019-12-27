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


@Injectable()
export class LotteryService {
    constructor(private http: HttpClient) { }
    CreateWXLuckyDrawAsync(input: any): Observable<any[]> {
        return this.http.post('/api/services/app/LuckyDraw/CreateWXLuckyDrawAsync', input).map(data => {
            return data.result;
        });
    }

    getWXLuckyDrawListAsync(): Observable<any[]>{
        return this.http.get('/api/services/app/LuckyDraw/GetWXLuckyDrawListAsync').map(data => {
            if (data.result) {
                return data.result;
            } else {
                return false;
            }
        });
    }
    
    updateWXLuckyDrawPubStatusAsync(input: any): Observable<any[]> {
        return this.http.post('/api/services/app/LuckyDraw/ChangeWXLuckyDrawPubStatusAsync', input).map(data => {
            return data.result;
        });
    }

    getEmployeeNameListAsyn(): Observable<any[]>{
        return this.http.get('/api/services/app/Employee/GetEmployeeNameListAsyn').map(data => {
            if (data.result) {
                return data.result;
            } else {
                return false;
            }
        });
    }
    
    //获取详细签到列表
    getSignListByDeptNameAsync(deptName:string): Observable<any[]>{
        return this.http.get('/api/services/app/Employee/GetSignListByDeptNameAsync?deptName='+deptName).map(data => {
            if (data.result) {
                return data.result;
            } else {
                return false;
            }
        });
    }
    

    //admin 根据管理员获得活动详情
    getLuckyDrawDetailByIdAsync(Id:string,openId:string): Observable<any[]>{
        return this.http.get('/api/services/app/LuckyDraw/GetLuckyDrawDetailByIdAsync?Id='+Id+'&openId='+openId).map(data => {
            if (data.result) {
                return data.result;
            } else {
                return false;
            }
        });
    }
    //内部员工获取活动列表
    getWXLuckyDrawListPublishedAsync(): Observable<any[]>{
        return this.http.get('/api/services/app/LuckyDraw/GetWXLuckyDrawListPublishedAsync').map(data => {
            if (data.result) {
                return data.result;
            } else {
                return false;
            }
        });
    }

     //内部员工获取活动列表
     getLuckySignInfoAsync(id:string): Observable<any[]>{
        return this.http.get('/api/services/app/LuckySign/GetLuckySignInfoAsync?openId='+id).map(data => {
            if (data.result) {
                return data.result;
            } else {
                return false;
            }
        });
    }
    
}
