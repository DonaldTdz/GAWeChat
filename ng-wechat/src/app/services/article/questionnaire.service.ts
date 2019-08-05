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
import { Questionnaire, QuestionOptions, AnswerRecords, ApiResult, QuestionRecord } from '../model/index';


@Injectable()
export class QuestionnaireService {
    constructor(private http: HttpClient) { }

    //获取问题列表
    getQuestionnaireList(): Observable<Questionnaire[]> {
        return this.http.get('/api/services/app/Questionnaire/GetWXQuestionnaireList').map(data => {
            if (data.result) {
                return Questionnaire.fromJSArray(data.result);
            } else {
                return null;
            }
        });
    }

    //获取答题记录
    getQuestionRecordById(params): Observable<Questionnaire[]> {
        return this.http.get('/api/services/app/Questionnaire/GetQuestionRecordWXByIdAsync', params).map(data => {
            if (data.result) {
                return Questionnaire.fromJSArray(data.result);
            } else {
                return null;
            }
        });
    }

    //获取各季度问卷填写记录
    // GetQuestionFillRecords(openId:string):Observable<QuestionnaireFillRecords[]>{
    //     return this.http.post('/api/services/app/AnswerRecord/WXGetQuestionnaireFillRecords', {openId:openId}).map(data => {
    //         if (data.result) {
    //             return QuestionnaireFillRecords.fromJSArray(data.result);
    //         } else {
    //             return null;
    //         }
    //     });
    // }

    getQuestionRecordList(): Observable<QuestionRecord[]> {
        return this.http.get('/api/services/app/QuestionRecord/GetQuestionRecordWXListAsync').map(data => {
            if (data.result) {
                return QuestionRecord.fromJSArray(data.result);
            } else {
                return null;
            }
        });
    }

    createAnswerRecord(input: any): Observable<ApiResult<any>> {
        return this.http.post('/api/services/app/AnswerRecord/CreateWXAnswerRecordsAsync', input).map(data => {
            let result = new ApiResult<any>();
            result.code = data.result.code;
            return result;
        });
    }
}
