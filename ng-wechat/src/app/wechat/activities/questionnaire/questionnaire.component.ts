import { Component, OnInit, Injector } from '@angular/core';
import { AppComponentBase } from '../../components/app-component-base';
import { Router } from '@angular/router';
import { QuestionnaireService, DemandForecastService } from '../../../services';
import { QuestionnaireFillRecords } from '../../../services/model';
import { ToptipsService } from 'ngx-weui';

@Component({
    moduleId: module.id,
    selector: 'questionnaire',
    templateUrl: 'questionnaire.component.html',
    styleUrls: ['questionnaire.component.scss']
})
export class QuestionnaireComponent extends AppComponentBase implements OnInit{

    isRetailer:boolean = false;
    questionnaireFillRecords:QuestionnaireFillRecords[]=[];
    constructor(injector: Injector
        , private router: Router
        , private demandService: DemandForecastService
        , private questionnaireService: QuestionnaireService
        , private srv: ToptipsService) {
        super(injector);
    }

    ngOnInit() {
        this.getQuestionnaireFillRecord()
    }

    getQuestionnaireFillRecord(){
        this.questionnaireService.GetQuestionFillRecords(this.settingsService.openId).subscribe(data=>{
            this.questionnaireFillRecords = data;
            console.log(this.questionnaireFillRecords);
            
        });
    }

    goDetail(fillRecord: QuestionnaireFillRecords) {
        this.demandService.getIsRetailerByIdAsync(this.settingsService.openId).subscribe(result => {
            this.isRetailer = result;
            if (this.isRetailer) {
                if (fillRecord.status != '已逾期') {
                    this.router.navigate(['/questionnaires/questionnaire-detail', { quarter: fillRecord.quarter }]);
                } else {
                    this.srv['warn']('当前项目已逾期，无法填写');
                }
            } else {
                this.srv['info']('请先进行零售客户绑定');
                this.router.navigate(['/personals/bind-retailer']);
            }
        });
        
    }
}
