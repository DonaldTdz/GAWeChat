import 'rxjs/add/observable/fromPromise';
import 'rxjs/add/observable/of';
import 'rxjs/add/observable/throw';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/toPromise';
import 'rxjs/add/operator/mergeMap';
import 'rxjs/add/operator/catch';

import { Injectable } from '@angular/core';
import { HttpClient } from '../httpclient'
import { Observable } from 'rxjs/Observable';
import * as moment from 'moment';
import { ApiResult, Favorite, MemberConfigs } from '../model/index';


@Injectable()
export class FavoriteService {
    constructor(private http: HttpClient) { }

    GetUserIsCancelShopAsycn(params: any): Observable<any> {
        return this.http.get('/api/services/app/Favorite/GetUserIsCancelShopAsycn', params, true).map(data => {
            if (data.result) {
                return data.result;
            } else {
                return false;
            }
        });
    }

    AddOrCancelFavoriteShop(params: any): Observable<any> {
        return this.http.post('/api/services/app/Favorite/AddOrCancelFavouriteShopAsync', params).map(data => {
            return data.result;
        });
    }

    GetWXMyFavoriteShopsAsync(params: any): Observable<Favorite[]> {
        return this.http.get('/api/services/app/Favorite/GetWXMyFavoriteShopsAsync', params).map(data => {
            if (data.result) {
                return Favorite.fromJSArray(data.result);
            } else {
                return null;
            }
        });
    }
}
