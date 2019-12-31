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
import { Parameter } from '@shared/service-proxies/entity';
import { MemberConfigs } from '@shared/entity/member/memberconfig';
import { ConfigCode } from '@shared/entity/member/configcode';

function throwException(message: string, status: number, response: string, headers: { [key: string]: any; }, result?: any): Observable<any> {
    if (result !== null && result !== undefined)
        return Observable.throw(result);
    else
        return Observable.throw(new SwaggerException(message, status, response, headers, null));
}

export class MemberConfigsServiceProxy {
    private http: Http;
    private baseUrl: string;
    protected jsonParseReviver: (key: string, value: any) => any = undefined;

    constructor(@Inject(Http) http: Http, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl ? baseUrl : "";
    }
    getMemberConfigs(): Observable<PagedResultDtoOfMemberConfigs> {
        let url_ = this.baseUrl + "/api/services/app/MemberConfig/GetTenanMemberConfigAsync?";
        url_ = url_.replace(/[?&]$/, "");

        let options_ = {
            method: "get",
            headers: new Headers({
                "Content-Type": "application/json",
                "Accept": "application/json"
            })
        };

        return this.http.request(url_, options_).flatMap((response_) => {
            return this.processGetMemberConfigs(response_);
        }).catch((response_: any) => {
            if (response_ instanceof Response) {
                try {
                    return this.processGetMemberConfigs(response_);
                } catch (e) {
                    return <Observable<PagedResultDtoOfMemberConfigs>><any>Observable.throw(e);
                }
            } else
                return <Observable<PagedResultDtoOfMemberConfigs>><any>Observable.throw(response_);
        });
    }

    protected processGetMemberConfigs(response: Response): Observable<PagedResultDtoOfMemberConfigs> {
        const status = response.status;

        let _headers: any = response.headers ? response.headers.toJSON() : {};
        if (status === 200) {
            const _responseText = response.text();
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? PagedResultDtoOfMemberConfigs.fromJS(resultData200) : new PagedResultDtoOfMemberConfigs();
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
        return Observable.of<PagedResultDtoOfMemberConfigs>(<any>null);
    }

    getName(openId: string): Observable<PagedResultDtoOfMemberConfigs> {
        let url_ = this.baseUrl + "/api/services/app/WeChatUser/GetUserNameByOpenIdAsync?";
        url_ = url_.replace(/[?&]$/, "");
        if (openId !== undefined)
            url_ += "?openId=" + encodeURIComponent("" + openId);
        let options_ = {
            method: "get",
            headers: new Headers({
                "Content-Type": "application/json",
                "Accept": "application/json"
            })
        };

        return this.http.request(url_, options_).flatMap((response_) => {
            return this.processGetMemberConfigs(response_);
        }).catch((response_: any) => {
            if (response_ instanceof Response) {
                try {
                    return this.processGetMemberConfigs(response_);
                } catch (e) {
                    return <Observable<PagedResultDtoOfMemberConfigs>><any>Observable.throw(e);
                }
            } else
                return <Observable<PagedResultDtoOfMemberConfigs>><any>Observable.throw(response_);
        });
    }

    protected processGetName(response: Response): Observable<PagedResultDtoOfMemberConfigs> {
        const status = response.status;

        let _headers: any = response.headers ? response.headers.toJSON() : {};
        if (status === 200) {
            const _responseText = response.text();
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? PagedResultDtoOfMemberConfigs.fromJS(resultData200) : new PagedResultDtoOfMemberConfigs();
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
        return Observable.of<PagedResultDtoOfMemberConfigs>(<any>null);
    }

    updateWXinfo(input: ConfigCode): Observable<ConfigCode> {
        let url_ = this.baseUrl + "/api/services/app/MemberConfig/CreateOrUpdateWXMemberConfigDtoAsync";
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
                    return <Observable<ConfigCode>><any>Observable.throw(e);
                }
            } else
                return <Observable<ConfigCode>><any>Observable.throw(response_);
        });
    }

    protected processupdateWXinfo(response: Response): Observable<ConfigCode> {
        const status = response.status;

        let _headers: any = response.headers ? response.headers.toJSON() : {};
        if (status === 200) {
            const _responseText = response.text();
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? ConfigCode.fromJS(resultData200) : new ConfigCode();
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
        return Observable.of<ConfigCode>(<any>null);
    }

    update(input: ConfigCode): Observable<ConfigCode> {
        let url_ = this.baseUrl + "/api/services/app/MemberConfig/CreateOrUpdateMemberConfigDtoAsync";
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
                    return <Observable<ConfigCode>><any>Observable.throw(e);
                }
            } else
                return <Observable<ConfigCode>><any>Observable.throw(response_);
        });
    }

    protected processUpdate(response: Response): Observable<ConfigCode> {
        const status = response.status;

        let _headers: any = response.headers ? response.headers.toJSON() : {};
        if (status === 200) {
            const _responseText = response.text();
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? ConfigCode.fromJS(resultData200) : new ConfigCode();
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
        return Observable.of<ConfigCode>(<any>null);
    }

    updatePreProduct(input: MemberConfigs): Observable<MemberConfigs> {
        let url_ = this.baseUrl + "/api/services/app/MemberConfig/CreateOrUpdatePreProductConfig";
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
            return this.processUpdatePreProduct(response_);
        }).catch((response_: any) => {
            if (response_ instanceof Response) {
                try {
                    return this.processUpdatePreProduct(response_);
                } catch (e) {
                    return <Observable<MemberConfigs>><any>Observable.throw(e);
                }
            } else
                return <Observable<MemberConfigs>><any>Observable.throw(response_);
        });
    }

    protected processUpdatePreProduct(response: Response): Observable<MemberConfigs> {
        const status = response.status;

        let _headers: any = response.headers ? response.headers.toJSON() : {};
        if (status === 200) {
            const _responseText = response.text();
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? MemberConfigs.fromJS(resultData200) : new MemberConfigs();
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
        return Observable.of<MemberConfigs>(<any>null);
    }

    getPreProductConfigAsync(): Observable<MemberConfigs> {
        let url_ = this.baseUrl + "/api/services/app/MemberConfig/GetPreProductConfigAsync?";
        url_ = url_.replace(/[?&]$/, "");

        let options_ = {
            method: "get",
            headers: new Headers({
                "Content-Type": "application/json",
                "Accept": "application/json"
            })
        };

        return this.http.request(url_, options_).flatMap((response_) => {
            return this.processGetPreProductConfigAsync(response_);
        }).catch((response_: any) => {
            if (response_ instanceof Response) {
                try {
                    return this.processGetPreProductConfigAsync(response_);
                } catch (e) {
                    return <Observable<MemberConfigs>><any>Observable.throw(e);
                }
            } else
                return <Observable<MemberConfigs>><any>Observable.throw(response_);
        });
    }

    protected processGetPreProductConfigAsync(response: Response): Observable<MemberConfigs> {
        const status = response.status;

        let _headers: any = response.headers ? response.headers.toJSON() : {};
        if (status === 200) {
            const _responseText = response.text();
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? MemberConfigs.fromJS(resultData200) : new MemberConfigs();
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
        return Observable.of<MemberConfigs>(<any>null);
    }

    getLotteryConfigs(): Observable<MemberConfigs> {
        let url_ = this.baseUrl + "/api/services/app/MemberConfig/GetLotteryConfigAsync?";
        url_ = url_.replace(/[?&]$/, "");

        let options_ = {
            method: "get",
            headers: new Headers({
                "Content-Type": "application/json",
                "Accept": "application/json"
            })
        };

        return this.http.request(url_, options_).flatMap((response_) => {
            return this.processGetLotteryConfigs(response_);
        }).catch((response_: any) => {
            if (response_ instanceof Response) {
                try {
                    return this.processGetLotteryConfigs(response_);
                } catch (e) {
                    return <Observable<MemberConfigs>><any>Observable.throw(e);
                }
            } else
                return <Observable<MemberConfigs>><any>Observable.throw(response_);
        });
    }

    protected processGetLotteryConfigs(response: Response): Observable<MemberConfigs> {
        const status = response.status;

        let _headers: any = response.headers ? response.headers.toJSON() : {};
        if (status === 200) {
            const _responseText = response.text();
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? MemberConfigs.fromJS(resultData200) : new MemberConfigs();
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
        return Observable.of<MemberConfigs>(<any>null);
    }

    updateLotteryConfig(input: MemberConfigs): Observable<MemberConfigs> {
        let url_ = this.baseUrl + "/api/services/app/MemberConfig/CreateOrUpdateMemberConfig";
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
            return this.processUpdateLotteryConfig(response_);
        }).catch((response_: any) => {
            if (response_ instanceof Response) {
                try {
                    return this.processUpdateLotteryConfig(response_);
                } catch (e) {
                    return <Observable<MemberConfigs>><any>Observable.throw(e);
                }
            } else
                return <Observable<MemberConfigs>><any>Observable.throw(response_);
        });
    }

    protected processUpdateLotteryConfig(response: Response): Observable<MemberConfigs> {
        const status = response.status;

        let _headers: any = response.headers ? response.headers.toJSON() : {};
        if (status === 200) {
            const _responseText = response.text();
            let result200: any = null;
            let resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            result200 = resultData200 ? MemberConfigs.fromJS(resultData200) : new MemberConfigs();
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
        return Observable.of<MemberConfigs>(<any>null);
    }
}

export class PagedResultDtoOfMemberConfigs implements PagedResultDtoOfMemberConfigs {
    // totalCount: number;
    items: MemberConfigs[];

    constructor(data?: PagedResultDtoOfMemberConfigs) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            // this.totalCount = data["totalCount"];
            // if (data["items"] && data["items"].constructor === Array) {
            //     this.items = [];
            //     for (let item of data["items"])
            //         this.items.push(MemberConfigs.fromJS(item));
            // }
            if (data && data.constructor === Array) {
                this.items = [];
                for (let item of data)
                    this.items.push(MemberConfigs.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfMemberConfigs {
        let result = new PagedResultDtoOfMemberConfigs();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        // data["totalCount"] = this.totalCount;
        // if (this.items && this.items.constructor === Array) {
        //     data["items"] = [];
        //     for (let item of this.items)
        //         data["items"].push(item.toJSON());
        // }
        if (this.items && this.items.constructor === Array) {
            data = [];
            for (let item of this.items)
                data.push(item.toJSON());
        }
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new PagedResultDtoOfMemberConfigs();
        result.init(json);
        return result;
    }
}

export interface IPagedResultDtoOfMemberConfigs {
    // totalCount: number;
    items: MemberConfigs[];
}