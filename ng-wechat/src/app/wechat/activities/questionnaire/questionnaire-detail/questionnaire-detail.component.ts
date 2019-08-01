import { Component, ViewEncapsulation, Injector, OnInit, ViewChild } from '@angular/core';
import { AppComponentBase } from '../../../components/app-component-base';
import { QuestionnaireService } from '../../../../services';
import { Router, ActivatedRoute } from '@angular/router';
import { timer } from 'rxjs/observable/timer';
import { Subscription } from 'rxjs';
import { ProgressModule } from 'ngx-weui';
import { Questionnaire, QuestionOptions, AnswerRecords } from '../../../../services/model';
import { e } from '@angular/core/src/render3';

@Component({
    moduleId: module.id,
    selector: 'questionnaire-detail',
    templateUrl: 'questionnaire-detail.component.html',
    styleUrls: ['questionnaire-detail.component.scss'],
    encapsulation: ViewEncapsulation.None,
})
export class QuestionnaireDetailComponent extends AppComponentBase implements OnInit {


    progressBar:any = { 
        p1:{ value: 0, doing: false }
     };
    questionNum:number;
    answerRecords:AnswerRecords[] = [];//回答列表
    questionnaireList : Questionnaire[] = [];//问题列表
    quarter: string = this.route.snapshot.params['quarter'];//季度

    constructor(injector: Injector
        , private router: Router
        , private route: ActivatedRoute
        , private questionnaireService: QuestionnaireService
    ) {
        super(injector);
    }

    ngOnInit() {
        this.getQuestionnaireList();
        // this.progressBar = {
        //     p1: { value: 0, doing: true },
        //   };
        //   timer(0, 40).subscribe(() => {
        //     const item = this.progressBar.p1;
        //       ++item.value;
        //       console.log(item.value);
              
        //       if (item.value >= 100) item.doing = false;
        //   });
    }

    getAnswerRecords(){
        this.questionnaireService.GetQuestionnaireList().subscribe(data=>{
            this.questionnaireList = data;
            this.questionNum = this.questionnaireList.length;
            this.questionnaireList.forEach(element => {
                this.answerRecords.push(new AnswerRecords({
                    id:"",
                    questionnaireId:element.id,
                    values: "",
                    remark: "",
                    openId:""
                }));
            });
        });
    }

    getQuestionnaireList(){
        this.questionnaireService.GetQuestionnaireList().subscribe(data=>{
            this.questionnaireList = data;
            this.questionNum = this.questionnaireList.length;
            this.questionnaireList.forEach(element => {
                this.answerRecords.push(new AnswerRecords({
                    id:"",
                    questionnaireId:element.id,
                    values: "",
                    remark: "",
                    openId:""
                }));
            });
        });
    }

    //单选框单击事件
    onRadioClick(questionnaireId : string, value : string){
        this.answerRecords.forEach(element => {
            if(element.questionnaireId === questionnaireId){
                element.values=value;
            }
        });
        console.log(this.answerRecords);
    }

    //提交
    onSave(){

    }
}
