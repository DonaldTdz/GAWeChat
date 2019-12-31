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
import { WechatUser, ApiResult } from '../model/index';


@Injectable()
export class WechatUserService {
  constructor(private http: HttpClient) { }

  GetWeChatUserByMemberBarCodeAsync(mCode: string, tId: string): Observable<WechatUser> {
    let param: any = {};
    param.memberBarCode = mCode;
    if (tId) {
      param.tenantId = tId;
    }
    return this.http.get('/api/services/app/WeChatUser/GetWeChatUserByMemberBarCodeAsync', param).map(data => {
      return WechatUser.fromJS(data.result);
    });
  }

  BindMemberAsync(params: any): Observable<ApiResult<WechatUser>> {
    return this.http.post('/api/services/app/WeChatUser/BindMemberAsync', params).map(data => {
      let result = new ApiResult<WechatUser>();
      result.code = data.result.code;
      result.msg = data.result.msg;
      result.data = WechatUser.fromJS(data.result.data);
      return result;
    });
  }

  BindWeChatUserAsync(params: any): Observable<ApiResult<WechatUser>> {
    return this.http.post('/api/services/app/WeChatUser/BindWeChatUserAsync', params).map(data => {
      let result = new ApiResult<WechatUser>();
      result.code = data.result.code;
      result.msg = data.result.msg;
      result.data = WechatUser.fromJS(data.result.data);
      return result;
    });
  }

  getSingleWeChatUser(params: any): Observable<WechatUser> {
    return this.http.get('/api/services/app/WeChatUser/GetSingleWeChatUser', params).map(data => {
      return WechatUser.fromJS(data);
    })
  }

  getShopemployee(input: any): Observable<WechatUser[]> {
    return this.http.get('/api/services/app/WeChatUser/GetShopEmployeesAsync', input).map(data => {
      return data.result;
    });
  }

  checkShopEmployee(input: WechatUser): Observable<any> {
    return this.http.post('/api/services/app/WeChatUser/CheckShopEmployeeAsync', input, null, true).map(data => {
      return data.result;
    });
  }
  unBindShopEmployee(input: any): Observable<any> {
    return this.http.post('/api/services/app/WeChatUser/CheckWeChatUserBindStatusAsync', input, null, true).map(data => {
      return data.result;
    });
  }

  getNoCheckShopEmployeeCount(input: any): Observable<number> {
    return this.http.get('/api/services/app/WeChatUser/GetShopEmployeesNoCheckCountAsync', input).map(data => {
      return data.result;
    });
  }

  getCustMemberCode(openId: any): Observable<WechatUser> {
    let url_ = "/api/services/app/WeChatUser/GetMemberBarCodeAsync?openId=" + openId;
    return this.http.get(url_).map(data => {
      return WechatUser.fromJS(data.result);
    });
  }

  getEmpRetailerList(openId: string): Observable<any> {
    return this.http.get('/api/services/app/Retailer/GetWXEmpRetailerListByIdAsync?openId=' + openId).map(data => {
      return data.result;
    });
  }

  getIsLotteryAdmin(openId: string): Observable<boolean> {
    return this.http.get('/api/services/app/MemberConfig/GetIsLotteryAdminAsync?openId=' + openId).map(data => {
      return <boolean>data.result;
    });
  }
}
