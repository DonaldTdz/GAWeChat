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
import { Article, ExhibitionShop, Exhibition, } from '../model/index';


@Injectable()
export class ArticleService {
    constructor(private http: HttpClient) { }

    GetPagedArticles(params: any): Observable<Article[]> {
        return this.http.get('/api/services/app/Article/GetWXPagedArticlesAsync', params).map(data => {
            if (data.result) {
                return Article.fromJSArray(data.result);
            } else {
                return null;
            }
        });
    }

    GetArticleById(params: any): Observable<Article> {
        return this.http.get('/api/services/app/Article/GetWXArticlesByIdAsync', params).map(data => {
            if (data.result) {
                return Article.fromJS(data.result);
            } else {
                return null;
            }
        });
    }

    GetWXPagedExpAsync(params: any): Observable<Article[]> {
        return this.http.get('/api/services/app/Article/GetWXPagedExpAsync', params).map(data => {
            if (data.result) {
                return Article.fromJSArray(data.result);
            } else {
                return null;
            }
        });
    }

    GetWXExpByIdAsync(params: any): Observable<Article> {
        return this.http.get('/api/services/app/Article/GetWXExpByIdAsync', params).map(data => {
            if (data.result) {
                return Article.fromJS(data.result);
            } else {
                return null;
            }
        });
    }

    AddStatisticalAsync(input: any): Observable<any> {
        return this.http.post('/api/services/app/StatisticalDetail/AddStatisticalAsync', input).map(data => {
            return data.result;
        });
    }

    GetIsGoodAsync(params: any): Observable<any> {
        return this.http.get('/api/services/app/StatisticalDetail/GetIsGoodAsync', params).map(data => {
            if (data.result) {
                return data.result;
            } else {
                return null;
            }
        });
    }

    AddGoodAsync(params: any): Observable<any> {
        return this.http.post('/api/services/app/StatisticalDetail/AddGoodAsync', params).map(data => {
            if (data.result) {
                return data.result;
            } else {
                return null;
            }
        });
    }
    CreatWXManuscript(params: any): Observable<any> {
        return this.http.post('/api/services/app/Manuscript/CreatWXManuscript', params).map(data => {
            if (data.result) {
                return data.result;
            } else {
                return null;
            }
        });
    }

    GetWXPagedExhibitionShopsAsync(params: any): Observable<any[]> {
        return this.http.get('/api/services/app/ExhibitionShop/GetWXPagedExhibitionShopsAsync', params).map(data => {
            if (data.result) {
                return data.result;
            } else {
                return null;
            }
        });
    }

    GetWXExhibitionShopsCountAsync(params: any): Observable<any> {
        return this.http.get('/api/services/app/ExhibitionShop/GetWXExhibitionShopsCountAsync', params).map(data => {
            if (data.result) {
                return data.result;
            } else {
                return 0;
            }
        });
    }

    GetWXVotesCountAsync(params: any): Observable<any> {
        return this.http.get('/api/services/app/VoteLog/GetWXVotesCountAsync', params).map(data => {
            if (data.result) {
                return data.result;
            } else {
                return 0;
            }
        });
    }

    GetExhibitionConfigAsync(params: any): Observable<Exhibition> {
        return this.http.get('/api/services/app/Exhibition/GetExhibitionConfigAsync', params).map(data => {
            if (data.result) {
                return Exhibition.fromJS(data.result);
            } else {
                return null;
            }
        });
    }

    AddVoteLogAsync(params: any): Observable<any> {
        return this.http.post('/api/services/app/VoteLog/AddVoteLogAsync', params).map(data => {
            if (data.result) {
                return data.result;
            } else {
                return null;
            }
        });
    }

    GetIsAttentionByOpenIdAsync(openId: string): Observable<any> {
        return this.http.get('/api/services/app/WeChatUser/GetIsAttentionByOpenIdAsync?openId=' + openId, null, true).map(data => {
            if (data.result) {
                return data.result;
            } else {
                return false;
            }
        });
    }

    GetCurrentDayVoteByIdAsync(params: any): Observable<any> {
        return this.http.get('/api/services/app/VoteLog/GetCurrentDayVoteByIdAsync', params).map(data => {
            if (data.result) {
                return data.result;
            } else {
                return 0;
            }
        });
    }

    GetExhibitionShopByKeyAsync(params: any): Observable<ExhibitionShop[]> {
        return this.http.get('/api/services/app/ExhibitionShop/GetExhibitionShopByKeyAsync', params).map(data => {
            if (data.result) {
                let rel = ExhibitionShop.fromJSArray(data.result);
                return rel;
            } else {
                return null;
            }
        });
    }

    GetWXExhibitionShopsByIdAsync(shopId: string): Observable<ExhibitionShop> {
        return this.http.get('/api/services/app/ExhibitionShop/GetWXExhibitionShopsByIdAsync?shopId=' + shopId).map(data => {
            if (data.result) {
                return ExhibitionShop.fromJS(data.result);
            } else {
                return null;
            }
        });
    }

    GetAuthorizationUrl(params: any): Observable<string> {
        return this.http.get('/api/services/app/ExhibitionShop/GetAuthorizationUrl', params).map(data => {
            if (data.result) {
                return data.result;
            } else {
                return null;
            }
        });
    }

    GetQRUrlByShopId(shopId: string): Observable<any> {
        return this.http.get('/api/services/app/Shop/GetQRUrlByShopId?shopId=' + shopId).map(data => {
            if (data.result) {
                return data.result;
            } else {
                return null;
            }
        });
    }
}
