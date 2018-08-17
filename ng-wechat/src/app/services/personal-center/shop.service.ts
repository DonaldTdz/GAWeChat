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
import { Shop, ApiResult, ShopProduct, ShopGoods, NearbyShop } from '../model/index';


@Injectable()
export class ShopService {
  constructor(private http: HttpClient) { }

  WechatCreateOrUpdateShop(params: any): Observable<boolean> {
    return this.http.post('/api/services/app/Shop/WechatCreateOrUpdateShop', params, null, true).map(data => {
      return <boolean>data.success;
    });
  }

  GetShopByOpenId(params: any): Observable<Shop> {
    return this.http.get('/api/services/app/Shop/GetShopByOpenId', params).map(data => {
      if (data.result) {
        let rel = Shop.fromJS(data.result);
        rel.evaluationArry = rel.evaluation.split(',');
        return rel;
      } else {
        return null;
      }
    });
  }

  GetShopProductByCode(params: any): Observable<ShopGoods> {
    return this.http.get('/api/services/app/Product/GetShopProductByCode', params).map(data => {
      if (data.result) {
        let rel = ShopGoods.fromJS(data.result);
        return rel;
      } else {
        return null;
      }
    });
  }

  GetShopProductsByShopId(params: any): Observable<ShopProduct[]> {
    return this.http.get('/api/services/app/ShopProduct/GetShopProductsByShopId', params).map(data => {
      if (data.result) {
        let rel = ShopProduct.fromJSArray(data.result);
        return rel;
      } else {
        return null;
      }
    });
  }

  GetRareProductByKeyAsync(params: any): Observable<ShopProduct[]> {
    return this.http.get('/api/services/app/Product/GetRareProductByKeyAsync', params).map(data => {
      if (data.result) {
        let rel = ShopProduct.fromJSArray(data.result);
        return rel;
      } else {
        return null;
      }
    });
  }

  GetRareProduct(params: any): Observable<any> {
    return this.http.get('/api/services/app/Product/GetRareProduct', params).map(data => {
      if (data.result) {
        return data.result;
      } else {
        return null;
      }
    });
  }

  SaveShopProducts(params: any): Observable<any> {
    return this.http.post('/api/services/app/ShopProduct/SaveShopProducts', params).map(data => {
      return data.result;
    });
  }

  ExchangeIntegral(params: any): Observable<any> {
    return this.http.post('/api/services/app/PurchaseRecord/ExchangeIntegralAsync', params, null, true).map(data => {
      return data.result;
    });
  }

  GetNearbyShopByLocationAsync(params: any): Observable<NearbyShop[]> {
    return this.http.get('/api/services/app/Shop/GetNearbyShopByLocationAsync', params).map(data => {
      if (data.result) {
        return NearbyShop.fromJSArray(data.result);
      } else {
        return null;
      }
    });
  }

  GetViewShopByIdAsync(params: any): Observable<Shop> {
    return this.http.get('/api/services/app/Shop/GetViewShopByIdAsync', params).map(data => {
      if (data.result) {
        let rel = Shop.fromJS(data.result);
        rel.evaluationArry = rel.evaluation.split(',');
        return rel;
      } else {
        return null;
      }
    });
  }

  AddSingleTotalAsync(input: any): Observable<Shop> {
    return this.http.post('/api/services/app/StatisticalDetail/AddSingleTotalAsync', input).map(data => {
      if (data.result) {
        let rel = Shop.fromJS(data.result);
        rel.evaluationArry = rel.evaluation.split(',');
        return rel;
      } else {
        return null;
      }
    });
  }

  GetShopListByGoodsIdAsync(params: any): Observable<NearbyShop[]> {
    return this.http.get('/api/services/app/Shop/GetShopListByGoodsIdAsync', params).map(data => {
      if (data.result) {
        return NearbyShop.fromJSArray(data.result);
      } else {
        return null;
      }
    });
  }

  CheckShop(params: any): Observable<boolean> {
    return this.http.post('/api/services/app/Shop/CheckShop', params).map(data => {
      return <boolean>data.success;
    });
  }

  FilesPostsBase64(params: any): Observable<any> {
    return this.http.post('/WeChatFile/FilesPostsBase64', params, null, true).map(data => {
      return data.result;
    });
  }

  GetQrCodeUrl(params: any): Observable<string> {
    return this.http.get('/api/services/app/Shop/GetQrCodeUrl', params, true).map(data => {
      return data.result;
    });
  }

  GetShopQrCodeUrl(params: any): Observable<string> {
    return this.http.get('/api/services/app/Shop/GetShopQrCodeURL', params, true).map(data => {
      return data.result;
    });
  }

  GetPreProductConfig(params: any): Observable<any> {
    return this.http.get('/api/services/app/MemberConfig/GetWXPreProductConfigAsync', params).map(data => {
      return data.result;
    });
  }
}
