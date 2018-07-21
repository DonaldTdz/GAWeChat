import 'rxjs/add/observable/fromPromise';
import 'rxjs/add/observable/of';
import 'rxjs/add/observable/throw';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/toPromise';
import 'rxjs/add/operator/mergeMap';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/finally';


import { Observable } from 'rxjs/Observable';
import { Injectable, Inject, Optional, InjectionToken } from '@angular/core';
import { Http, Headers, ResponseContentType, Response } from '@angular/http';
import { Activity } from '@shared/service-proxies/entity/acitivity';
import { API_BASE_URL, SwaggerException } from '@shared/service-proxies/service-proxies';
import { Parameter, ApiResult, WeChatUserStatistic } from '@shared/service-proxies/entity';
import { WechatUser } from '@shared/entity/wechat';
// import * as moment from 'moment';


function throwException(message: string, status: number, response: string, headers: { [key: string]: any; }, result?: any): Observable<any> {
    if (result !== null && result !== undefined)
        return Observable.throw(result);
    else
        return Observable.throw(new SwaggerException(message, status, response, headers, null));
}

export class WechatUserServiceProxy {
    private http: Http;
    private baseUrl: string;
    protected jsonParseReviver: (key: string, value: any) => any = undefined;

    constructor(@Inject(Http) http: Http, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl ? baseUrl : "";
    }

    ExportIntegralExcel(input: any): Observable<ApiResult> {
        let url_ = this.baseUrl + "/api/services/app/WeChatUser/ExportWeChatUsersIntegralExcelAsync";
        url_ = url_.replace(/[?&]$/, "");
        console.log(input);
        const content_ = JSON.stringify(input);

        let options_ = {
            body: content_,
            method: "post",
            headers: new Headers({
                "Content-Type": "application/json",
                "Accept": "application/json"
            })
        };

        return this.http.request(url_, options_).flatMap((response_) => {
            return this.processExportIntegralExcel(response_);
        }).catch((response_: any) => {
            if (response_ instanceof Response) {
                try {
                    return this.processExportExcel(response_);
                } catch (e) {
                    return <Observable<ApiResult>><any>Observable.throw(e);
                }
            } else
                return <Observable<ApiResult>><any>Observable.throw(response_);
        });
    }
    protected processExportIntegralExcel(response: Response): Observable<ApiResult> {
        const status = response.status;

        let _headers: any = response.headers ? response.headers.toJSON() : {};
        if (status === 200) {
            const _responseText = response.text();
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? ApiResult.fromJS(resultData200) : new ApiResult();
            return Observable.of(result200);
        } else if (status === 401) {
            const _responseText = response.text();
            return throwException("A server error occurred.", status, _responseText, _headers);
        } else if (status === 403) {
            const _responseText = response.text();
            return throwException("A server error occurred.", status, _responseText, _headers);
        } else if (status !== 200 && status !== 204) {
            const _responseText = response.text();
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
        }
        return Observable.of<ApiResult>(<any>null);
    }
    getUserInfoAsync(openId: string): Observable<WechatUser> {
        let url_ = this.baseUrl + "/api/services/app/IntegralDetail/GetUserInfoAsync?";
        if (openId !== undefined)
            url_ += "openId=" + encodeURIComponent("" + openId) + "&";
        url_ = url_.replace(/[?&]$/, "");

        let options_ = {
            method: "get",
            headers: new Headers({
                "Content-Type": "application/json",
                "Accept": "application/json"
            })
        };

        return this.http.request(url_, options_).flatMap((response_) => {
            return this.processGetUserInfoAsync(response_);
        }).catch((response_: any) => {
            if (response_ instanceof Response) {
                try {
                    return this.processGetUserInfoAsync(response_);
                } catch (e) {
                    return <Observable<WechatUser>><any>Observable.throw(e);
                }
            } else
                return <Observable<WechatUser>><any>Observable.throw(response_);
        });
    }

    protected processGetUserInfoAsync(response: Response): Observable<WechatUser> {
        const status = response.status;

        let _headers: any = response.headers ? response.headers.toJSON() : {};
        if (status === 200) {
            const _responseText = response.text();
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? WechatUser.fromJS(resultData200) : Observable.of<WechatUser>(<any>null);
            return Observable.of(result200);
        } else if (status === 401) {
            const _responseText = response.text();
            return throwException("A server error occurred.", status, _responseText, _headers);
        } else if (status === 403) {
            const _responseText = response.text();
            return throwException("A server error occurred.", status, _responseText, _headers);
        } else if (status !== 200 && status !== 204) {
            const _responseText = response.text();
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
        }
        return Observable.of<WechatUser>(<any>null);
    }

    exportExcel(input: any): Observable<ApiResult> {
        let url_ = this.baseUrl + "/api/services/app/WeChatUser/ExportWeChatUsersExcel";
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(input);

        let options_ = {
            body: content_,
            method: "post",
            headers: new Headers({
                "Content-Type": "application/json",
                "Accept": "application/json"
            })
        };

        return this.http.request(url_, options_).flatMap((response_) => {
            return this.processExportExcel(response_);
        }).catch((response_: any) => {
            if (response_ instanceof Response) {
                try {
                    return this.processExportExcel(response_);
                } catch (e) {
                    return <Observable<ApiResult>><any>Observable.throw(e);
                }
            } else
                return <Observable<ApiResult>><any>Observable.throw(response_);
        });
    }

    protected processExportExcel(response: Response): Observable<ApiResult> {
        const status = response.status;

        let _headers: any = response.headers ? response.headers.toJSON() : {};
        if (status === 200) {
            const _responseText = response.text();
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? ApiResult.fromJS(resultData200) : new ApiResult();
            return Observable.of(result200);
        } else if (status === 401) {
            const _responseText = response.text();
            return throwException("A server error occurred.", status, _responseText, _headers);
        } else if (status === 403) {
            const _responseText = response.text();
            return throwException("A server error occurred.", status, _responseText, _headers);
        } else if (status !== 200 && status !== 204) {
            const _responseText = response.text();
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
        }
        return Observable.of<ApiResult>(<any>null);
    }
    getIntegralList(skipCount: number, maxResultCount: number, parameter: Parameter[]): Observable<PagedResultDtoOfWeChatUser> {
        let url_ = this.baseUrl + "/api/services/app/IntegralDetail/GetPagedIntegralDetailsAsync?";
        if (skipCount !== undefined)
            url_ += "SkipCount=" + encodeURIComponent("" + skipCount) + "&";
        if (maxResultCount !== undefined)
            url_ += "MaxResultCount=" + encodeURIComponent("" + maxResultCount) + "&";

        if (parameter.length > 0) {
            parameter.forEach(element => {
                if (element.value !== undefined && element.value !== null) {
                    url_ += element.key + "=" + encodeURIComponent("" + element.value) + "&";
                }
            });
        }

        url_ = url_.replace(/[?&]$/, "");

        let options_ = {
            method: "get",
            headers: new Headers({
                "Content-Type": "application/json",
                "Accept": "application/json"
            })
        };

        return this.http.request(url_, options_).flatMap((response_) => {
            return this.processGetAll(response_);
        }).catch((response_: any) => {
            if (response_ instanceof Response) {
                try {
                    return this.processGetAll(response_);
                } catch (e) {
                    return <Observable<PagedResultDtoOfWeChatUser>><any>Observable.throw(e);
                }
            } else
                return <Observable<PagedResultDtoOfWeChatUser>><any>Observable.throw(response_);
        });
    }

    protected processGetIntegralList(response: Response): Observable<PagedResultDtoOfWeChatUser> {
        const status = response.status;

        let _headers: any = response.headers ? response.headers.toJSON() : {};
        if (status === 200) {
            const _responseText = response.text();
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? PagedResultDtoOfWeChatUser.fromJS(resultData200) : new PagedResultDtoOfWeChatUser();
            return Observable.of(result200);
        } else if (status === 401) {
            const _responseText = response.text();
            return throwException("A server error occurred.", status, _responseText, _headers);
        } else if (status === 403) {
            const _responseText = response.text();
            return throwException("A server error occurred.", status, _responseText, _headers);
        } else if (status !== 200 && status !== 204) {
            const _responseText = response.text();
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
        }
        return Observable.of<PagedResultDtoOfWeChatUser>(<any>null);
    }
    /**
     * 获取所有微信用户信息
     * @return Success
     */
    getAll(skipCount: number, maxResultCount: number, parameter: Parameter[]): Observable<PagedResultDtoOfWeChatUser> {
        let url_ = this.baseUrl + "/api/services/app/WeChatUser/GetPagedWeChatUsersReEmAndRE?";
        if (skipCount !== undefined)
            url_ += "SkipCount=" + encodeURIComponent("" + skipCount) + "&";
        if (maxResultCount !== undefined)
            url_ += "MaxResultCount=" + encodeURIComponent("" + maxResultCount) + "&";

        if (parameter.length > 0) {
            parameter.forEach(element => {
                if (element.value !== undefined && element.value !== null) {
                    url_ += element.key + "=" + encodeURIComponent("" + element.value) + "&";
                }
            });
        }

        url_ = url_.replace(/[?&]$/, "");

        let options_ = {
            method: "get",
            headers: new Headers({
                "Content-Type": "application/json",
                "Accept": "application/json"
            })
        };

        return this.http.request(url_, options_).flatMap((response_) => {
            return this.processGetAll(response_);
        }).catch((response_: any) => {
            if (response_ instanceof Response) {
                try {
                    return this.processGetAll(response_);
                } catch (e) {
                    return <Observable<PagedResultDtoOfWeChatUser>><any>Observable.throw(e);
                }
            } else
                return <Observable<PagedResultDtoOfWeChatUser>><any>Observable.throw(response_);
        });
    }

    protected processGetAll(response: Response): Observable<PagedResultDtoOfWeChatUser> {
        const status = response.status;

        let _headers: any = response.headers ? response.headers.toJSON() : {};
        if (status === 200) {
            const _responseText = response.text();
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? PagedResultDtoOfWeChatUser.fromJS(resultData200) : new PagedResultDtoOfWeChatUser();
            return Observable.of(result200);
        } else if (status === 401) {
            const _responseText = response.text();
            return throwException("A server error occurred.", status, _responseText, _headers);
        } else if (status === 403) {
            const _responseText = response.text();
            return throwException("A server error occurred.", status, _responseText, _headers);
        } else if (status !== 200 && status !== 204) {
            const _responseText = response.text();
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
        }
        return Observable.of<PagedResultDtoOfWeChatUser>(<any>null);
    }

    getShopWeChatUserAll(skipCount: number, maxResultCount: number, parameter: Parameter[]): Observable<PagedResultDtoOfWeChatUser> {
        let url_ = this.baseUrl + "/api/services/app/WeChatUser/GetPagedShopWeChatUsers?";
        if (skipCount !== undefined)
            url_ += "SkipCount=" + encodeURIComponent("" + skipCount) + "&";
        if (maxResultCount !== undefined)
            url_ += "MaxResultCount=" + encodeURIComponent("" + maxResultCount) + "&";

        if (parameter.length > 0) {
            parameter.forEach(element => {
                if (element.value !== undefined && element.value !== null) {
                    url_ += element.key + "=" + encodeURIComponent("" + element.value) + "&";
                }
            });
        }

        url_ = url_.replace(/[?&]$/, "");

        let options_ = {
            method: "get",
            headers: new Headers({
                "Content-Type": "application/json",
                "Accept": "application/json"
            })
        };

        return this.http.request(url_, options_).flatMap((response_) => {
            return this.processGetShopWeChatUserAll(response_);
        }).catch((response_: any) => {
            if (response_ instanceof Response) {
                try {
                    return this.processGetShopWeChatUserAll(response_);
                } catch (e) {
                    return <Observable<PagedResultDtoOfWeChatUser>><any>Observable.throw(e);
                }
            } else
                return <Observable<PagedResultDtoOfWeChatUser>><any>Observable.throw(response_);
        });
    }

    protected processGetShopWeChatUserAll(response: Response): Observable<PagedResultDtoOfWeChatUser> {
        const status = response.status;

        let _headers: any = response.headers ? response.headers.toJSON() : {};
        if (status === 200) {
            const _responseText = response.text();
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? PagedResultDtoOfWeChatUser.fromJS(resultData200) : new PagedResultDtoOfWeChatUser();
            return Observable.of(result200);
        } else if (status === 401) {
            const _responseText = response.text();
            return throwException("A server error occurred.", status, _responseText, _headers);
        } else if (status === 403) {
            const _responseText = response.text();
            return throwException("A server error occurred.", status, _responseText, _headers);
        } else if (status !== 200 && status !== 204) {
            const _responseText = response.text();
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
        }
        return Observable.of<PagedResultDtoOfWeChatUser>(<any>null);
    }

    /**
     * 通过微信用户id获取微信用户信息
     * @param id 微信用户id
     */
    get(id: string): Observable<WechatUser> {
        let url_ = this.baseUrl + "/api/services/app/WeChatUser/GetWeChatUserByIdAsync?";
        if (id !== undefined)
            url_ += "Id=" + encodeURIComponent("" + id) + "&";
        url_ = url_.replace(/[?&]$/, "");

        let options_ = {
            method: "get",
            headers: new Headers({
                "Content-Type": "application/json",
                "Accept": "application/json"
            })
        };

        return this.http.request(url_, options_).flatMap((response_) => {
            return this.processGet(response_);
        }).catch((response_: any) => {
            if (response_ instanceof Response) {
                try {
                    return this.processGet(response_);
                } catch (e) {
                    return <Observable<WechatUser>><any>Observable.throw(e);
                }
            } else
                return <Observable<WechatUser>><any>Observable.throw(response_);
        });
    }

    protected processGet(response: Response): Observable<WechatUser> {
        const status = response.status;

        let _headers: any = response.headers ? response.headers.toJSON() : {};
        if (status === 200) {
            const _responseText = response.text();
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? WechatUser.fromJS(resultData200) : new WechatUser();
            return Observable.of(result200);
        } else if (status === 401) {
            const _responseText = response.text();
            return throwException("A server error occurred.", status, _responseText, _headers);
        } else if (status === 403) {
            const _responseText = response.text();
            return throwException("A server error occurred.", status, _responseText, _headers);
        } else if (status !== 200 && status !== 204) {
            const _responseText = response.text();
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
        }
        return Observable.of<WechatUser>(<any>null);
    }

    /**
     * 新增或修改微信用户信息(解除绑定)
     * @param input 
     */
    update(input: WechatUser): Observable<WechatUser> {
        let url_ = this.baseUrl + "/api/services/app/WeChatUser/CheckWeChatUserBindStatusAsync";
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(input);

        let options_ = {
            body: content_,
            method: "post",
            headers: new Headers({
                "Content-Type": "application/json",
                "Accept": "application/json"
            })
        };

        return this.http.request(url_, options_).flatMap((response_) => {
            return this.processUpdate(response_);
        }).catch((response_: any) => {
            if (response_ instanceof Response) {
                try {
                    return this.processUpdate(response_);
                } catch (e) {
                    return <Observable<WechatUser>><any>Observable.throw(e);
                }
            } else
                return <Observable<WechatUser>><any>Observable.throw(response_);
        });
    }

    protected processUpdate(response: Response): Observable<WechatUser> {
        const status = response.status;

        let _headers: any = response.headers ? response.headers.toJSON() : {};
        if (status === 200) {
            const _responseText = response.text();
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? WechatUser.fromJS(resultData200) : new WechatUser();
            return Observable.of(result200);
        } else if (status === 401) {
            const _responseText = response.text();
            return throwException("A server error occurred.", status, _responseText, _headers);
        } else if (status === 403) {
            const _responseText = response.text();
            return throwException("A server error occurred.", status, _responseText, _headers);
        } else if (status !== 200 && status !== 204) {
            const _responseText = response.text();
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
        }
        return Observable.of<WechatUser>(<any>null);
    }

    /**
    * @return Success
    */
    delete(id: number): Observable<void> {
        let url_ = this.baseUrl + "/api/services/app/WeChatUser/DeleteWeChatUser?";
        if (id !== undefined)
            url_ += "Id=" + encodeURIComponent("" + id) + "&";
        url_ = url_.replace(/[?&]$/, "");

        let options_ = {
            method: "delete",
            headers: new Headers({
                "Content-Type": "application/json",
            })
        };

        return this.http.request(url_, options_).flatMap((response_) => {
            return this.processDelete(response_);
        }).catch((response_: any) => {
            if (response_ instanceof Response) {
                try {
                    return this.processDelete(response_);
                } catch (e) {
                    return <Observable<void>><any>Observable.throw(e);
                }
            } else
                return <Observable<void>><any>Observable.throw(response_);
        });
    }

    protected processDelete(response: Response): Observable<void> {
        const status = response.status;

        let _headers: any = response.headers ? response.headers.toJSON() : {};
        if (status === 200) {
            const _responseText = response.text();
            return Observable.of<void>(<any>null);
        } else if (status === 401) {
            const _responseText = response.text();
            return throwException("A server error occurred.", status, _responseText, _headers);
        } else if (status === 403) {
            const _responseText = response.text();
            return throwException("A server error occurred.", status, _responseText, _headers);
        } else if (status !== 200 && status !== 204) {
            const _responseText = response.text();
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
        }
        return Observable.of<void>(<any>null);
    }

    getWeChatUserStatistic(): Observable<ResultDtoOfWeChatUserStatistic> {
        let url_ = this.baseUrl + "/api/services/app/WeChatUser/GetWeChatUserStatistic";

        url_ = url_.replace(/[?&]$/, "");

        let options_ = {
            method: "get",
            headers: new Headers({
                "Content-Type": "application/json",
                "Accept": "application/json"
            })
        };

        return this.http.request(url_, options_).flatMap((response_) => {
            return this.processWeChatUserStatistic(response_);
        }).catch((response_: any) => {
            if (response_ instanceof Response) {
                try {
                    return this.processWeChatUserStatistic(response_);
                } catch (e) {
                    return <Observable<ResultDtoOfWeChatUserStatistic>><any>Observable.throw(e);
                }
            } else
                return <Observable<ResultDtoOfWeChatUserStatistic>><any>Observable.throw(response_);
        });
    }

    protected processWeChatUserStatistic(response: Response): Observable<ResultDtoOfWeChatUserStatistic> {
        const status = response.status;

        let _headers: any = response.headers ? response.headers.toJSON() : {};
        if (status === 200) {
            const _responseText = response.text();
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? ResultDtoOfWeChatUserStatistic.fromJS(resultData200) : new ResultDtoOfWeChatUserStatistic();
            return Observable.of(result200);
        } else if (status === 401) {
            const _responseText = response.text();
            return throwException("A server error occurred.", status, _responseText, _headers);
        } else if (status === 403) {
            const _responseText = response.text();
            return throwException("A server error occurred.", status, _responseText, _headers);
        } else if (status !== 200 && status !== 204) {
            const _responseText = response.text();
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
        }
        return Observable.of<ResultDtoOfWeChatUserStatistic>(<any>null);
    }
}
export class PagedResultDtoOfWeChatUser implements IPagedResultDtoOfWeChatUser {
    totalCount: number;
    items: WechatUser[];

    constructor(data?: IPagedResultDtoOfWeChatUser) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.totalCount = data["totalCount"];
            if (data["items"] && data["items"].constructor === Array) {
                this.items = [];
                for (let item of data["items"])
                    this.items.push(WechatUser.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfWeChatUser {
        let result = new PagedResultDtoOfWeChatUser();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["totalCount"] = this.totalCount;
        if (this.items && this.items.constructor === Array) {
            data["items"] = [];
            for (let item of this.items)
                data["items"].push(item.toJSON());
        }
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new PagedResultDtoOfWeChatUser();
        result.init(json);
        return result;
    }
}

export interface IPagedResultDtoOfWeChatUser {
    totalCount: number;
    items: WechatUser[];
}

export class ResultDtoOfWeChatUserStatistic implements IResultDtoOfWeChatUserStatistic {
    total:number;
    items: WeChatUserStatistic[];

    constructor(data?: IResultDtoOfWeChatUserStatistic) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.total = data["total"];
            if (data["wechatUserStaDto"] && data["wechatUserStaDto"].constructor === Array) {
            // if (data && data.constructor === Array) {
                this.items = [];
                for (let item of data["wechatUserStaDto"])
                    this.items.push(WeChatUserStatistic.fromJS(item));
            }
        }
    }

    static fromJS(data: any): ResultDtoOfWeChatUserStatistic {
        let result = new ResultDtoOfWeChatUserStatistic();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["total"] = this.total;
        if (this.items && this.items.constructor === Array) {
            data["wechatUserStaDto"] = [];
            // data = [];
            for (let item of this.items)
                data["wechatUserStaDto"].push(item.toJSON());
                // data.push(item.toJSON());
        }
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new ResultDtoOfWeChatUserStatistic();
        result.init(json);
        return result;
    }
}

export interface IResultDtoOfWeChatUserStatistic {
    items: WeChatUserStatistic[];
}
