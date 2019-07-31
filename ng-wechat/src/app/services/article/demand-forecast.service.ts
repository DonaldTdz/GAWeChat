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
import { DemandForecast } from '../model';


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
}
