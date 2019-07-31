import 'rxjs/add/observable/fromPromise';
import 'rxjs/add/observable/of';
import 'rxjs/add/observable/throw';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/toPromise';
import 'rxjs/add/operator/mergeMap';
import 'rxjs/add/operator/catch';

import { Injectable } from '@angular/core';
import { HttpClient } from '../httpclient';
import { Observable } from 'rxjs/Observable';
import { DemandForecast, DemandDetail, ApiResult } from '../model';


@Injectable()
export class DemandForecastService {
    constructor(private http: HttpClient) { }

    getDemandListAsync(): Observable<DemandForecast[]> {
        return this.http.get('/api/services/app/DemandForecast/GetWXDemandListAsync').map(data => {
            if (data.result) {
                return DemandForecast.fromJSArray(data.result);
            } else {
                return null;
            }
        });
    }

    getDetailListByIdAsync(params: any): Observable<DemandDetail[]> {
        return this.http.get('/api/services/app/DemandDetail/GetWXDetailListByIdAsync', params).map(data => {
            if (data.result) {
                return DemandDetail.fromJSArray(data.result);
            } else {
                return null;
            }
        });
    }

    getDetailRecordByIdAsync(params: any): Observable<DemandDetail[]> {
        return this.http.get('/api/services/app/DemandDetail/GetWXDetailRecordByIdAsync', params).map(data => {
            if (data.result) {
                return DemandDetail.fromJSArray(data.result);
            } else {
                return null;
            }
        });
    }

    createForecastRecordAsync(input: any): Observable<ApiResult<any>> {
        return this.http.post('/api/services/app/ForecastRecord/CreateForecastRecordAsync', input).map(data => {
            let result = new ApiResult<any>();
            result.code = data.result.code;
            return result;
        });
    }

    getIsRetailerByIdAsync(openId: string): Observable<boolean> {
        let url_ = "/api/services/app/WeChatUser/GetIsRetailerByIdAsync?openId=" + openId;
        return this.http.get(url_).map(data => {
            if (data.result) {
                return data.result;
            } else {
                return false;
            }
        });
    }
}
