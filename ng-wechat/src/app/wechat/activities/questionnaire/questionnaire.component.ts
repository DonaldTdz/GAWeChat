import { Component, OnInit, Injector } from '@angular/core';
import { AppComponentBase } from '../../components/app-component-base';
import { Router } from '@angular/router';
import { QuestionnaireService, DemandForecastService } from '../../../services';
import { QuestionRecord } from '../../../services/model';
import { ToptipsService } from 'ngx-weui';

@Component({
    moduleId: module.id,
    selector: 'questionnaire',
    templateUrl: 'questionnaire.component.html',
    styleUrls: ['questionnaire.component.scss']
})
export class QuestionnaireComponent extends AppComponentBase implements OnInit {
    status: boolean = false;
    isRetailer: boolean = false;
    questionRecordList: QuestionRecord[] = [];
    constructor(injector: Injector
        , private router: Router
        , private demandService: DemandForecastService
        , private questionnaireService: QuestionnaireService
        , private srv: ToptipsService) {
        super(injector);
    }

    ngOnInit() {
        this.getQuestionRecordList()
    }

    getQuestionRecordList() {
        this.questionnaireService.getQuestionRecordList().subscribe(data => {
            this.questionRecordList = data;
        });
    }

    goDetail(id: string, status: string) {
        this.demandService.getIsRetailerByIdAsync(this.settingsService.openId).subscribe(result => {
            this.isRetailer = result;
            if (this.isRetailer) {
                if (status != '已逾期') {
                    if (status == '查看记录') {
                        this.status = false;
                    } else {
                        this.status = true;
                    }
                    this.router.navigate(['/questionnaires/questionnaire-detail', { id: id, status: this.status }]);
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
