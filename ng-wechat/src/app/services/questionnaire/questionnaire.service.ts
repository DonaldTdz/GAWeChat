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
import { Questionnaire, QuestionOptions } from '../model/index';


@Injectable()
export class QuestionnaireService {
    constructor(private http: HttpClient) { }

    GetQuestionnaireList(): Observable<Questionnaire[]> {
        return this.http.get('/api/services/app/Questionnaire/GetWXQuestionnaireList').map(data => {
            if (data.result) {
                var result = Questionnaire.fromJSArray(data.result);
                result.forEach(item => {
                    QuestionOptions.fromJSArray(item.questionOptions);
                });
                return result;
            } else {
                return null;
            }
        });
    }
}
