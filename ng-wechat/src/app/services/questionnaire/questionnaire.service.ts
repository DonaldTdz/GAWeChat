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
import { Questionnaire, QuestionOptions, AnswerRecords, QuestionnaireFillRecords, ApiResult } from '../model/index';


@Injectable()
export class QuestionnaireService {
    constructor(private http: HttpClient) { }

    //获取问题列表
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

    //获取答题记录
    GetAnswerRecords(quarter:string,openId:string): Observable<AnswerRecords[]> {
        return this.http.post('/api/services/app/AnswerRecord/WXGetAnswerRecordList', {quarter:quarter,openId:openId}).map(data => {
            if (data.result) {
                return AnswerRecords.fromJSArray(data.result);
            } else {
                return null;
            }
        });
    }

    //获取各季度问卷填写记录
    GetQuestionFillRecords(openId:string):Observable<QuestionnaireFillRecords[]>{
        return this.http.post('/api/services/app/AnswerRecord/WXGetQuestionnaireFillRecords', {openId:openId}).map(data => {
            if (data.result) {
                return QuestionnaireFillRecords.fromJSArray(data.result);
            } else {
                return null;
            }
        });
    }

    BachCreateAnswerRecords(answerRecords:any):Observable<ApiResult<any>>{
        return this.http.post('/api/services/app/AnswerRecord/WXBatchCreateAnswerRecords', answerRecords).map(data => {
            let result = new ApiResult<any>();
            result.code = data.result.code;
            return result;
        });
    }
}
