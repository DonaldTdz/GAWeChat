import 'rxjs/add/observable/fromPromise';
import 'rxjs/add/observable/of';
import 'rxjs/add/observable/throw';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/toPromise';
import 'rxjs/add/operator/mergeMap';
import 'rxjs/add/operator/catch';

import { Observable } from 'rxjs/Observable';
import { SwaggerException, API_BASE_URL } from '@shared/service-proxies/service-proxies';
import { Http, Headers, ResponseContentType, Response } from '@angular/http';
import { Inject, Optional, Injectable, InjectionToken } from '@angular/core';
import { Parameter, HomeInfo, ApiResult, ShopStatistic } from '@shared/service-proxies/entity';
import { Shop } from '@shared/entity/customer/shop';

function throwException(message: string, status: number, response: string, headers: { [key: string]: any; }, result?: any): Observable<any> {
    if (result !== null && result !== undefined)
        return Observable.throw(result);
    else
        return Observable.throw(new SwaggerException(message, status, response, headers, null));
}
export class ShopServiceProxy {
    private http: Http;
    private baseUrl: string;
    protected jsonParseReviver: (key: string, value: any) => any = undefined;

    constructor(@Inject(Http) http: Http, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl ? baseUrl : "";
    }
    /**
    * 获取所有店铺信息
    */
    getAll(skipCount: number, maxResultCount: number, parameter: Parameter[]): Observable<PagedResultDtoOfShop> {
        let url_ = this.baseUrl + "/api/services/app/Shop/GetPagedShopsByRetailer?";
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
                    return <Observable<PagedResultDtoOfShop>><any>Observable.throw(e);
                }
            } else
                return <Observable<PagedResultDtoOfShop>><any>Observable.throw(response_);
        });
    }

    protected processGetAll(response: Response): Observable<PagedResultDtoOfShop> {
        const status = response.status;

        let _headers: any = response.headers ? response.headers.toJSON() : {};
        if (status === 200) {
            const _responseText = response.text();
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? PagedResultDtoOfShop.fromJS(resultData200) : new PagedResultDtoOfShop();
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
        return Observable.of<PagedResultDtoOfShop>(<any>null);
    }

    getHomeInfo(): Observable<HomeInfo> {
        let url_ = this.baseUrl + "/api/services/app/Shop/GetHomeInfo";

        url_ = url_.replace(/[?&]$/, "");

        let options_ = {
            method: "get",
            headers: new Headers({
                "Content-Type": "application/json",
                "Accept": "application/json"
            })
        };

        return this.http.request(url_, options_).flatMap((response_) => {
            return this.processGetHomeInfo(response_);
        }).catch((response_: any) => {
            if (response_ instanceof Response) {
                try {
                    return this.processGetHomeInfo(response_);
                } catch (e) {
                    return <Observable<HomeInfo>><any>Observable.throw(e);
                }
            } else
                return <Observable<HomeInfo>><any>Observable.throw(response_);
        });
    }

    protected processGetHomeInfo(response: Response): Observable<HomeInfo> {
        const status = response.status;

        let _headers: any = response.headers ? response.headers.toJSON() : {};
        if (status === 200) {
            const _responseText = response.text();
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? HomeInfo.fromJS(resultData200) : new HomeInfo();
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
        return Observable.of<HomeInfo>(<any>null);
    }

    getPendingShopList(): Observable<ApiResult> {
        let url_ = this.baseUrl + "/api/services/app/Shop/GetPendingShopList";

        url_ = url_.replace(/[?&]$/, "");

        let options_ = {
            method: "get",
            headers: new Headers({
                "Content-Type": "application/json",
                "Accept": "application/json"
            })
        };

        return this.http.request(url_, options_).flatMap((response_) => {
            return this.processGetPendingShopList(response_);
        }).catch((response_: any) => {
            if (response_ instanceof Response) {
                try {
                    return this.processGetPendingShopList(response_);
                } catch (e) {
                    return <Observable<ApiResult>><any>Observable.throw(e);
                }
            } else
                return <Observable<ApiResult>><any>Observable.throw(response_);
        });
    }

    protected processGetPendingShopList(response: Response): Observable<ApiResult> {
        const status = response.status;

        let _headers: any = response.headers ? response.headers.toJSON() : {};
        if (status === 200) {
            const _responseText = response.text();
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? ApiResult.fromJS(resultData200) : Observable.of<ApiResult>(<any>null);
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

    /**
    * 通过店铺id获取店铺信息
    * @param id 店铺id
    */
    get(id: number): Observable<Shop> {
        let url_ = this.baseUrl + "/api/services/app/Shop/GetShopByIdRetailerAsync?";
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
                    return <Observable<Shop>><any>Observable.throw(e);
                }
            } else
                return <Observable<Shop>><any>Observable.throw(response_);
        });
    }

    protected processGet(response: Response): Observable<Shop> {
        const status = response.status;

        let _headers: any = response.headers ? response.headers.toJSON() : {};
        if (status === 200) {
            const _responseText = response.text();
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? Shop.fromJS(resultData200) : new Shop();
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
        return Observable.of<Shop>(<any>null);
    }

    /**
     * 新增或修改店铺信息
     * @param input 
     */
    update(input: Shop): Observable<Shop> {
        let url_ = this.baseUrl + "/api/services/app/Shop/CreateOrUpdateShopDto";
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
                    return <Observable<Shop>><any>Observable.throw(e);
                }
            } else
                return <Observable<Shop>><any>Observable.throw(response_);
        });
    }

    protected processUpdate(response: Response): Observable<Shop> {
        const status = response.status;

        let _headers: any = response.headers ? response.headers.toJSON() : {};
        if (status === 200) {
            const _responseText = response.text();
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? Shop.fromJS(resultData200) : new Shop();
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
        return Observable.of<Shop>(<any>null);
    }


    CheckShop(input: any): Observable<Shop> {
        let url_ = this.baseUrl + "/api/services/app/Shop/CheckShop";
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
            return this.processCheckShop(response_);
        }).catch((response_: any) => {
            if (response_ instanceof Response) {
                try {
                    return this.processCheckShop(response_);
                } catch (e) {
                    return <Observable<Shop>><any>Observable.throw(e);
                }
            } else
                return <Observable<Shop>><any>Observable.throw(response_);
        });
    }

    protected processCheckShop(response: Response): Observable<Shop> {
        const status = response.status;

        let _headers: any = response.headers ? response.headers.toJSON() : {};
        if (status === 200) {
            const _responseText = response.text();
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? Shop.fromJS(resultData200) : new Shop();
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
        return Observable.of<Shop>(<any>null);
    }

    ExportExcel(input: any): Observable<ApiResult> {
        let url_ = this.baseUrl + "/api/services/app/Shop/ExportShopExcel";
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

    PromotionCodeZip(input: any): Observable<ApiResult> {
        let url_ = this.baseUrl + "/api/services/app/Shop/PromotionCodeZip";
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
    protected processPromotionCodeZip(response: Response): Observable<ApiResult> {
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

    getShopStatistic(): Observable<ResultDtoOfShopStatistic> {
        let url_ = this.baseUrl + "/api/services/app/Shop/GetShopStatisticsByCompany";

        url_ = url_.replace(/[?&]$/, "");

        let options_ = {
            method: "get",
            headers: new Headers({
                "Content-Type": "application/json",
                "Accept": "application/json"
            })
        };

        return this.http.request(url_, options_).flatMap((response_) => {
            return this.processShopStatistic(response_);
        }).catch((response_: any) => {
            if (response_ instanceof Response) {
                try {
                    return this.processShopStatistic(response_);
                } catch (e) {
                    return <Observable<ResultDtoOfShopStatistic>><any>Observable.throw(e);
                }
            } else
                return <Observable<ResultDtoOfShopStatistic>><any>Observable.throw(response_);
        });
    }

    protected processShopStatistic(response: Response): Observable<ResultDtoOfShopStatistic> {
        const status = response.status;

        let _headers: any = response.headers ? response.headers.toJSON() : {};
        if (status === 200) {
            const _responseText = response.text();
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? ResultDtoOfShopStatistic.fromJS(resultData200) : new ResultDtoOfShopStatistic();
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
        return Observable.of<ResultDtoOfShopStatistic>(<any>null);
    }
}

export class PagedResultDtoOfShop implements IPagedResultDtoOfShop {
    totalCount: number;
    items: Shop[];

    constructor(data?: IPagedResultDtoOfShop) {
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
                    this.items.push(Shop.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfShop {
        let result = new PagedResultDtoOfShop();
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
        let result = new PagedResultDtoOfShop();
        result.init(json);
        return result;
    }
}

export interface IPagedResultDtoOfShop {
    totalCount: number;
    items: Shop[];
}

export class ResultDtoOfShopStatistic implements IResultDtoOfShopStatistic {
    total:number;
    items: ShopStatistic[];

    constructor(data?: IResultDtoOfShopStatistic) {
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
            if (data["shopStaDto"] && data["shopStaDto"].constructor === Array) {
            // if (data && data.constructor === Array) {
                this.items = [];
                for (let item of data["shopStaDto"])
                    this.items.push(ShopStatistic.fromJS(item));
            }
        }
    }

    static fromJS(data: any): ResultDtoOfShopStatistic {
        let result = new ResultDtoOfShopStatistic();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["total"] = this.total;
        if (this.items && this.items.constructor === Array) {
            data["shopStaDto"] = [];
            // data = [];
            for (let item of this.items)
                data["shopStaDto"].push(item.toJSON());
                // data.push(item.toJSON());
        }
        return data;
    }


    clone() {
        const json = this.toJSON();
        let result = new ResultDtoOfShopStatistic();
        result.init(json);
        return result;
    }
}

export interface IResultDtoOfShopStatistic {
    total:number;
    items: ShopStatistic[];
}