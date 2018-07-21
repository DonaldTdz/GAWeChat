import 'rxjs/add/observable/fromPromise';
import 'rxjs/add/observable/of';
import 'rxjs/add/observable/throw';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/toPromise';
import 'rxjs/add/operator/mergeMap';
import 'rxjs/add/operator/catch';

import { Injectable } from '@angular/core';
import { Http, Headers, Response, Request, RequestMethod, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import * as moment from 'moment';
import { AppConsts } from './AppConsts';
import { ToastService } from "ngx-weui/toast";

export class SwaggerException extends Error {
    message: string;
    status: number;
    response: string;
    headers: { [key: string]: any; };
    result: any;

    constructor(message: string, status: number, response: string, headers: { [key: string]: any; }, result: any) {
        super();

        this.message = message;
        this.status = status;
        this.response = response;
        this.headers = headers;
        this.result = result;
    }

    protected isSwaggerException = true;

    static isSwaggerException(obj: any): obj is SwaggerException {
        return obj.isSwaggerException === true;
    }
}

function throwException(message: string, status: number, response: string, headers: { [key: string]: any; }, result?: any): Observable<any> {
    if (result !== null && result !== undefined)
        return Observable.throw(result);
    else
        return Observable.throw(new SwaggerException(message, status, response, headers, null));
}

@Injectable()
export class HttpClient {

    protected jsonParseReviver: (key: string, value: any) => any = undefined;
    private baseHost: string = AppConsts.remoteServiceBaseUrl;

    constructor(private http: Http, private srv: ToastService) { }

    get(url: string, params?: { [key: string]: string }, showLoading?: boolean): Observable<any> {
        url = this.baseHost + url;
        return this.request(url + this._formatUrl(params), RequestMethod.Get, null, showLoading);
    }

    post(url: string, body?: any, params?: { [key: string]: any }, showLoading?: boolean): Observable<any> {
        url = this.baseHost + url;
        if (params) {
            url += this._formatUrl(params);
        }
        return this.request(url, RequestMethod.Post, body, showLoading);
    }

    request(url: string, method: RequestMethod, body?: any, showLoading?: boolean): Observable<any> {
        //let headers = new Headers();

        let headers = new Headers({
            "Content-Type": "application/json",
            "Accept": "application/json"
        });

        let options = new RequestOptions();
        options.headers = headers;
        options.url = url;
        options.method = method;
        if (body) {
            const content_ = JSON.stringify(body);
            options.body = content_;
        }
        //options.withCredentials = true;

        let request = new Request(options);

        if (showLoading === true) {
            this.srv.show(null, 10000, null, 'loading');
        }

        return this.http.request(request).flatMap((response_) => {
            this.srv.hide();
            return this.process(response_);
        }).catch(x => this.handleError(x));
    }

    /**
     * 将字典转为QueryString
     */
    private _formatUrl(params?: { [key: string]: string }): string {
        if (!params) return '';

        let fegment = [];
        for (let k in params) {
            let v: any = params[k];
            if (v) {
                if (v instanceof Date) {
                    v = moment(v).format('YYYY-MM-DD HH:mm:SS');
                }
                fegment.push(`${k}=${v}`);
            }
        }
        return '?' + fegment.join('&');
    }

    /**
    * 通用异常处理
    */
    private handleError(error: Response | any) {
        let errMsg: string;
        if (error instanceof Response) {
            const body = error.json() || '';
            const err = body.error || JSON.stringify(body);
            errMsg = `${error.status} - ${error.statusText || ''} ${err}`;
        } else {
            errMsg = error ? error.toString() : '服务器发生异常，请稍后再试';
        }
        return Observable.throw(errMsg);
    }

    private process(response: Response): Observable<any> {
        const status = response.status;

        let _headers: any = response.headers ? response.headers.toJSON() : {};
        if (status === 200) {
            const _responseText = response.text();
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? resultData200 : Observable.of<any>(<any>null);
            return Observable.of(result200);
        } else if (status === 401) {
            const _responseText = response.text();
            return throwException("发生服务器错误.", status, _responseText, _headers);
        } else if (status === 403) {
            const _responseText = response.text();
            return throwException("发生服务器错误.", status, _responseText, _headers);
        } else if (status !== 200 && status !== 204) {
            const _responseText = response.text();
            return throwException("发生了意外的服务器错误.", status, _responseText, _headers);
        }
        return Observable.of<any>(<any>null);
    }
}
