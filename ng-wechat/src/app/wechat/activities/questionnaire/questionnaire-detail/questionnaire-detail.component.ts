import { Component, ViewEncapsulation, Injector, OnInit, ViewChild } from '@angular/core';
import { AppComponentBase } from '../../../components/app-component-base';
import { QuestionnaireService } from '../../../../services';
import { Router, ActivatedRoute } from '@angular/router';
import { ToptipsService } from 'ngx-weui';
import { Questionnaire, QuestionAnswer } from '../../../../services/model';

@Component({
    moduleId: module.id,
    selector: 'questionnaire-detail',
    templateUrl: 'questionnaire-detail.component.html',
    styleUrls: ['questionnaire-detail.component.scss'],
    encapsulation: ViewEncapsulation.None,
})
export class QuestionnaireDetailComponent extends AppComponentBase implements OnInit {
    progressBar: number = 0;
    checkedNum: number = 0;
    status: boolean = false;
    questionnaireList: Questionnaire[] = [];//问题列表
    questionRecordId: string = this.route.snapshot.params['id'];
    statusParams: string = this.route.snapshot.params['status'];
    constructor(injector: Injector
        , private router: Router
        , private route: ActivatedRoute
        , private questionnaireService: QuestionnaireService
        , private srv: ToptipsService
    ) {
        super(injector);
    }

    ngOnInit() {
        if (this.statusParams == 'true') {
            this.status = true;
            this.getQuestionnaireList();
        } else {
            this.status = false;
            this.getQuestionRecordById();
        }
    }

    getQuestionRecordById() {
        let params: any = {};
        params.openId = this.settingsService.openId;
        params.questionRecordId = this.questionRecordId;
        this.questionnaireService.getQuestionRecordById(params).subscribe(data => {
            this.questionnaireList = data;
        });
    }

    getQuestionnaireList() {
        this.questionnaireService.getQuestionnaireList().subscribe(data => {
            this.questionnaireList = data;
        });
    }

    //#region 废弃代码
    // getAnswerRecords() {
    //     this.questionnaireService.GetAnswerRecords(this.quarter.toString(), this.settingsService.openId).subscribe(data => {
    //         this.answerRecords = data;
    //         if (this.answerRecords != null) {
    //             this.isFilled = true;
    //         } else {
    //             this.answerRecords = [];
    //         }
    //         console.log(this.answerRecords);

    //     });
    // }
    // getQuestionnaireList() {
    //     this.questionnaireService.getQuestionnaireList().subscribe(data => {
    //         this.questionnaireList = data;
    //         this.questionNum = 0;
    //         if (!this.isFilled) {
    //             this.questionnaireList.forEach(element => {
    //                 this.answerRecords.push(new AnswerRecords({
    //                     id: '',
    //                     questionnaireId: element.id,
    //                     values: '',
    //                     remark: '',
    //                     openId: this.settingsService.openId
    //                 }));
    //                 this.questionNum++;
    //             });
    //         } else {
    //             this.answerRecords.forEach(element => {
    //                 this.questionnaireList.forEach(questionnaire => {
    //                     if (questionnaire.id === element.questionnaireId) {
    //                         questionnaire.questionOptions.forEach(option => {
    //                             if (element.values === option.value) {
    //                                 // console.log(questionnaire.id+"--"+option.questionnaireId);
    //                                 option.isChecked = true;
    //                             }
    //                         })
    //                     }
    //                 })
    //             });
    //             // this.answerRecords.
    //         }
    //     });
    // }

    // startProgress(){
    //     this.progressBar = {
    //         p1: { value: 0, doing: true },
    //       };
    //       const item = this.progressBar.p1;
    //         item.value+=100/this.questionNum;
    //         console.log(item.value);

    //         if (item.value >= 100) item.doing = false;
    // }

    //单选框单击事件
    // onRadioClick(questionnaireId: string, value: string) {
    //     console.log(this.questionnaireList);

    //     if (this.isFilled) {
    //         return;
    //     }
    //     this.answerRecords.forEach(element => {
    //         if (element.questionnaireId === questionnaireId) {
    //             element.values = value;
    //         }
    //     });
    //     console.log(this.answerRecords);
    //     let count = 0;
    //     this.questionnaireList.forEach(element => {
    //         let moduleChecked = false;
    //         element.questionOptions.forEach(q => {
    //             if (q.value === value) {
    //                 q.isChecked = true;
    //             }
    //             if (q.isChecked) {
    //                 moduleChecked = true;
    //             }
    //         })
    //         if (element.id === questionnaireId) {
    //             element.isChecked = true;
    //         }
    //         if (moduleChecked) {
    //             count++;
    //         }
    //     });

    //     if (count > this.checkedCount) {
    //         this.checkedCount++;
    //         const item = this.progressBar.p1;
    //         item.value += 100 / this.questionNum;
    //         //console.log(item.value);

    //         if (item.value >= 100) item.doing = false;
    //     }
    // }
    //#endregion
    onRadioClick(index) {
        if (this.questionnaireList[index].value) {
            return;
        } else {
            this.checkedNum++;
        }
        this.progressBar = (this.checkedNum / this.questionnaireList.length) * 100;
    }

    save() {
        if (this.progressBar < 100) {
            this.srv['warn']('还有未选择的项');
            return;
        }
        let input: any = {};
        input.openId = this.settingsService.openId;
        input.questionRecordId = this.questionRecordId;
        input.list = QuestionAnswer.fromJSArray(this.questionnaireList);
        this.questionnaireService.createAnswerRecord(input).subscribe(data => {
            if (data && data.code == 0) {
                this.router.navigate(['/questionnaires/submit-success']);
            } else {
                this.srv['warn']('提交失败，请重试');
            }
        });
    }
}
