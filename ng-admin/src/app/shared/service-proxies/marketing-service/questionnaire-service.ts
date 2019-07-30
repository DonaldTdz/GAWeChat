import 'rxjs/add/observable/fromPromise';
import 'rxjs/add/observable/of';
import 'rxjs/add/observable/throw';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/toPromise';
import 'rxjs/add/operator/mergeMap';
import 'rxjs/add/operator/catch';

import { SwaggerException, API_BASE_URL } from "@shared/service-proxies/service-proxies";
import { Inject, Optional, Injectable, InjectionToken } from "@angular/core";
import { Observable } from "rxjs/Observable";
import { Http, Headers, ResponseContentType, Response } from "@angular/http";
import { Parameter, ApiResult } from '@shared/service-proxies/entity';
import { Questionnaire } from '@shared/entity/marketting';

function throwException(message: string, status: number, response: string, headers: { [key: string]: any; }, result?: any): Observable<any> {
    if (result !== null && result !== undefined)
        return Observable.throw(result);
    else
        return Observable.throw(new SwaggerException(message, status, response, headers, null));
}
export class QuestionnaireServiceProxy {
    private http: Http;
    private baseUrl: string;
    protected jsonParseReviver: (key: string, value: any) => any = undefined;

    constructor(@Inject(Http) http: Http, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl ? baseUrl : "";
    }

    create(input: Questionnaire): Observable<ApiResult> {
        let url_ = this.baseUrl + "/api/services/app/Questionnaire/CreateOrUpdateQuestionnaire";
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
            return this.processCreate(response_);
        }).catch((response_: Questionnaire) => {
            if (response_ instanceof Response) {
                try {
                    return this.processCreate(response_);
                } catch (e) {
                    return <Observable<ApiResult>><any>Observable.throw(e);
                }
            } else
                return <Observable<ApiResult>><any>Observable.throw(response_);
        });
    }
    protected processCreate(response: Response): Observable<ApiResult> {
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
     * 获取所有投稿信息
     */
    getAll(skipCount: number, maxResultCount: number, type?:number): Observable<PagedResultDtoOfQuestionnaire> {
        let url_ = this.baseUrl + "/api/services/app/Questionnaire/GetPaged?";
        if (skipCount !== undefined)
            url_ += "SkipCount=" + encodeURIComponent("" + skipCount) + "&";
        if (maxResultCount !== undefined)
            url_ += "MaxResultCount=" + encodeURIComponent("" + maxResultCount) + "&";
        if (type !== null) {
            url_ += "Type="+encodeURIComponent(""+type)+"&";
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
                    return <Observable<PagedResultDtoOfQuestionnaire>><any>Observable.throw(e);
                }
            } else
                return <Observable<PagedResultDtoOfQuestionnaire>><any>Observable.throw(response_);
        });
    }

    protected processGetAll(response: Response): Observable<PagedResultDtoOfQuestionnaire> {
        const status = response.status;

        let _headers: any = response.headers ? response.headers.toJSON() : {};
        if (status === 200) {
            const _responseText = response.text();
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? PagedResultDtoOfQuestionnaire.fromJS(resultData200) : new PagedResultDtoOfQuestionnaire();
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
        return Observable.of<PagedResultDtoOfQuestionnaire>(<any>null);
    }
    get(id: string): Observable<Questionnaire> {
        let url_ = this.baseUrl + "/api/services/app/Questionnaire/GetById?";
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
                    return <Observable<Questionnaire>><any>Observable.throw(e);
                }
            } else
                return <Observable<Questionnaire>><any>Observable.throw(response_);
        });
    }

    protected processGet(response: Response): Observable<Questionnaire> {
        const status = response.status;

        let _headers: any = response.headers ? response.headers.toJSON() : {};
        if (status === 200) {
            const _responseText = response.text();
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? Questionnaire.fromJS(resultData200) : new Questionnaire();
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
        return Observable.of<Questionnaire>(<any>null);
    }

    update(input: Questionnaire): Observable<Questionnaire> {
        let url_ = this.baseUrl + "/api/services/app/Questionnaire/CreateOrUpdateQuestionnaire";
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
                    return <Observable<Questionnaire>><any>Observable.throw(e);
                }
            } else
                return <Observable<Questionnaire>><any>Observable.throw(response_);
        });
    }

    protected processUpdate(response: Response): Observable<Questionnaire> {
        const status = response.status;

        let _headers: any = response.headers ? response.headers.toJSON() : {};
        if (status === 200) {
            const _responseText = response.text();
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? Questionnaire.fromJS(resultData200) : new Questionnaire();
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
        return Observable.of<Questionnaire>(<any>null);
    }

    /**
    * @return Success
    */
   delete(id: string): Observable<ApiResult> {
    let url_ = this.baseUrl + "/api/services/app/Questionnaire/Delete?";
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
    }).catch((response_: ApiResult) => {
        if (response_ instanceof Response) {
            try {
                return this.processDelete(response_);
            } catch (e) {
                return <Observable<ApiResult>><any>Observable.throw(e);
            }
        } else
            return <Observable<ApiResult>><any>Observable.throw(response_);
    });
}

protected processDelete(response: Response): Observable<ApiResult> {
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

}

export class PagedResultDtoOfQuestionnaire implements IPagedResultDtoOfQuestionnaire {
    totalCount: number;
    items: Questionnaire[];

    constructor(data?: IPagedResultDtoOfQuestionnaire) {
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
                    this.items.push(Questionnaire.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfQuestionnaire {
        let result = new PagedResultDtoOfQuestionnaire();
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
        let result = new PagedResultDtoOfQuestionnaire();
        result.init(json);
        return result;
    }
}

export interface IPagedResultDtoOfQuestionnaire {
    totalCount: number;
    items: Questionnaire[];
}