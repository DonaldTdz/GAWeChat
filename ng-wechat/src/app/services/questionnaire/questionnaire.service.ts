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
import { Questionnaire } from '../model/index';


@Injectable()
export class QuestionnaireService {
    constructor(private http: HttpClient) { }

    GetPagedArticles(params: any): Observable<Questionnaire[]> {
        return this.http.get('/api/services/app/Questionnaire/GetPaged', params).map(data => {
            if (data.result) {
                return Questionnaire.fromJSArray(data.result);
            } else {
                return null;
            }
        });
    }
}
