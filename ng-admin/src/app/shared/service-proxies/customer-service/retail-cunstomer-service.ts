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
import { RetailCustomer } from '@shared/service-proxies/entity/retail-customer';
import { Parameter, ApiResult } from '@shared/service-proxies/entity';

function throwException(message: string, status: number, response: string, headers: { [key: string]: any; }, result?: any): Observable<any> {
    if (result !== null && result !== undefined)
        return Observable.throw(result);
    else
        return Observable.throw(new SwaggerException(message, status, response, headers, null));
}
export class RetailCustomerServiceProxy {
    private http: Http;
    private baseUrl: string;
    protected jsonParseReviver: (key: string, value: any) => any = undefined;

    constructor(@Inject(Http) http: Http, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl ? baseUrl : "";
    }


    /**
     * 通过零售户id获取零售户信息
     * @param id 零售户id
     */
    get(id: number): Observable<RetailCustomer> {
        let url_ = this.baseUrl + "/api/services/app/Retailer/GetRetailerByIdDtoAsync?";
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
                    return <Observable<RetailCustomer>><any>Observable.throw(e);
                }
            } else
                return <Observable<RetailCustomer>><any>Observable.throw(response_);
        });
    }

    protected processGet(response: Response): Observable<RetailCustomer> {
        const status = response.status;

        let _headers: any = response.headers ? response.headers.toJSON() : {};
        if (status === 200) {
            const _responseText = response.text();
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? RetailCustomer.fromJS(resultData200) : new RetailCustomer();
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
        return Observable.of<RetailCustomer>(<any>null);
    }
    exportRetailerAllExcel(input: any): Observable<ApiResult> {
        let url_ = this.baseUrl + "/api/services/app/Retailer/ExportRetailerAllExcel";
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
            return this.processExportRetailerAllExcel(response_);
        }).catch((response_: any) => {
            if (response_ instanceof Response) {
                try {
                    return this.processExportRetailerAllExcel(response_);
                } catch (e) {
                    return <Observable<ApiResult>><any>Observable.throw(e);
                }
            } else
                return <Observable<ApiResult>><any>Observable.throw(response_);
        });
    }
    protected processExportRetailerAllExcel(response: Response): Observable<ApiResult> {
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

    exportRetailerLevelExcel(input: any): Observable<ApiResult> {
        let url_ = this.baseUrl + "/api/services/app/Retailer/ExportRetailerLevelExcel";
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
            return this.processExportRetailerLevelExcel(response_);
        }).catch((response_: any) => {
            if (response_ instanceof Response) {
                try {
                    return this.processExportRetailerLevelExcel(response_);
                } catch (e) {
                    return <Observable<ApiResult>><any>Observable.throw(e);
                }
            } else
                return <Observable<ApiResult>><any>Observable.throw(response_);
        });
    }
    protected processExportRetailerLevelExcel(response: Response): Observable<ApiResult> {
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

    importRetailerLevelExcelAsync(): Observable<ApiResult> {
        let url_ = this.baseUrl + "/api/services/app/Retailer/ImportRetailerLevelExcelAsync";
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify({});

        let options_ = {
            body: content_,
            method: "post",
            headers: new Headers({
                "Content-Type": "application/json",
                "Accept": "application/json"
            })
        };

        return this.http.request(url_, options_).flatMap((response_) => {
            return this.processImportRetailerLevelExcelAsync(response_);
        }).catch((response_: any) => {
            if (response_ instanceof Response) {
                try {
                    return this.processImportRetailerLevelExcelAsync(response_);
                } catch (e) {
                    return <Observable<ApiResult>><any>Observable.throw(e);
                }
            } else
                return <Observable<ApiResult>><any>Observable.throw(response_);
        });
    }

    protected processImportRetailerLevelExcelAsync(response: Response): Observable<ApiResult> {
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


    /**
     * 新增或修改零售户信息
     * @param input 
     */
    update(input: RetailCustomer): Observable<RetailCustomer> {
        let url_ = this.baseUrl + "/api/services/app/Retailer/CreateOrUpdateRetailerDto";
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
                    return <Observable<RetailCustomer>><any>Observable.throw(e);
                }
            } else
                return <Observable<RetailCustomer>><any>Observable.throw(response_);
        });
    }

    protected processUpdate(response: Response): Observable<RetailCustomer> {
        const status = response.status;

        let _headers: any = response.headers ? response.headers.toJSON() : {};
        if (status === 200) {
            const _responseText = response.text();
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? RetailCustomer.fromJS(resultData200) : new RetailCustomer();
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
        return Observable.of<RetailCustomer>(<any>null);
    }

    /**
    * @return Success
    */
    delete(id: string): Observable<void> {
        let url_ = this.baseUrl + "/api/services/app/Retailer/DeleteRetailer?";
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


    /**
     * 获取零售户
     * @return Success
     */
    getAll(skipCount: number, maxResultCount: number, parameter: Parameter[]): Observable<PagedResultDtoOfRetailCustomer> {
        let url_ = this.baseUrl + "/api/services/app/Retailer/GetPagedRetailers?";
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
                    return <Observable<PagedResultDtoOfRetailCustomer>><any>Observable.throw(e);
                }
            } else
                return <Observable<PagedResultDtoOfRetailCustomer>><any>Observable.throw(response_);
        });
    }

    protected processGetAll(response: Response): Observable<PagedResultDtoOfRetailCustomer> {
        const status = response.status;

        let _headers: any = response.headers ? response.headers.toJSON() : {};
        if (status === 200) {
            const _responseText = response.text();
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? PagedResultDtoOfRetailCustomer.fromJS(resultData200) : new PagedResultDtoOfRetailCustomer();
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
        return Observable.of<PagedResultDtoOfRetailCustomer>(<any>null);
    }

    CheckCode(code: string, id: string): Observable<boolean> {
        let url_ = this.baseUrl + "/api/services/app/Retailer/CheckName?";
        if (code !== undefined)
            url_ += "code=" + encodeURIComponent("" + code) + "&";
        if (id !== undefined)
            url_ += "id=" + encodeURIComponent("" + id) + "&";
        url_ = url_.replace(/[?&]$/, "");

        // const content_ = JSON.stringify(input);

        let options_ = {
            // body: content_,
            method: "post",
            headers: new Headers({
                "Content-Type": "application/json",
                "Accept": "application/json"
            })
        };

        return this.http.request(url_, options_).flatMap((response_) => {
            return this.processCheckCode(response_);
        }).catch((response_: any) => {
            if (response_ instanceof Response) {
                try {
                    return this.processCheckCode(response_);
                } catch (e) {
                    return <Observable<boolean>><any>Observable.throw(e);
                }
            } else
                return <Observable<boolean>><any>Observable.throw(response_);
        });
    }

    protected processCheckCode(response: Response): Observable<boolean> {
        const status = response.status;

        let _headers: any = response.headers ? response.headers.toJSON() : {};
        if (status === 200) {
            const _responseText = response.text();
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? resultData200 : false;
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
        return Observable.of<boolean>(<any>null);
    }


    getDataStatisticsAsync(parameter: Parameter[]): Observable<PagedResultDtoOfRetailCustomer> {
        let url_ = this.baseUrl + "/api/services/app/Retailer/GetDataStatisticsAsync?";
        // if (skipCount !== undefined)
        //     url_ += "SkipCount=" + encodeURIComponent("" + skipCount) + "&";
        // if (maxResultCount !== undefined)
        //     url_ += "MaxResultCount=" + encodeURIComponent("" + maxResultCount) + "&";

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
            return this.processGetDataStatisticsAsync(response_);
        }).catch((response_: any) => {
            if (response_ instanceof Response) {
                try {
                    return this.processGetDataStatisticsAsync(response_);
                } catch (e) {
                    return <Observable<PagedResultDtoOfRetailCustomer>><any>Observable.throw(e);
                }
            } else
                return <Observable<PagedResultDtoOfRetailCustomer>><any>Observable.throw(response_);
        });
    }

    protected processGetDataStatisticsAsync(response: Response): Observable<PagedResultDtoOfRetailCustomer> {
        const status = response.status;

        let _headers: any = response.headers ? response.headers.toJSON() : {};
        if (status === 200) {
            const _responseText = response.text();
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? PagedResultDtoOfRetailCustomer.fromJS(resultData200) : new PagedResultDtoOfRetailCustomer();
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
        return Observable.of<PagedResultDtoOfRetailCustomer>(<any>null);
    }
}
export class PagedResultDtoOfRetailCustomer implements IPagedResultDtoOfRetailCustomer {
    totalCount: number;
    items: RetailCustomer[];

    constructor(data?: IPagedResultDtoOfRetailCustomer) {
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
                    this.items.push(RetailCustomer.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfRetailCustomer {
        let result = new PagedResultDtoOfRetailCustomer();
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
        let result = new PagedResultDtoOfRetailCustomer();
        result.init(json);
        return result;
    }
}

export interface IPagedResultDtoOfRetailCustomer {
    totalCount: number;
    items: RetailCustomer[];
}