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
    WXLuckyDrawCreateAsyn(input: any): Observable<any[]> {
        return this.http.post('/api/services/app/LuckyDraw/WXLuckyDrawCreateAsyn', input).map(data => {
            return data.result;
        });
    }

    GetWXLuckyDrawListAsyn(): Observable<any[]>{
        return this.http.get('/api/services/app/LuckyDraw/GetWXLuckyDrawListAsyn').map(data => {
            if (data.result) {
                return data.result;
            } else {
                return false;
            }
        });
    }
    
    WXLuckyDrawUpdatePubStatusAsync(input: any): Observable<any[]> {
        return this.http.post('/api/services/app/LuckyDraw/WXLuckyDrawUpdatePubStatusAsync', input).map(data => {
            return data.result;
        });
    }

    GetEmployeeNameListAsyn(): Observable<any[]>{
        return this.http.get('/api/services/app/Employee/GetEmployeeNameListAsyn').map(data => {
            if (data.result) {
                return data.result;
            } else {
                return false;
            }
        });
    }
    
    //获取详细签到列表
    GetSignListByDeptNameAsync(deptName:string): Observable<any[]>{
        return this.http.get('/api/services/app/Employee/GetSignListByDeptNameAsync?deptName='+deptName).map(data => {
            if (data.result) {
                return data.result;
            } else {
                return false;
            }
        });
    }
    
}
