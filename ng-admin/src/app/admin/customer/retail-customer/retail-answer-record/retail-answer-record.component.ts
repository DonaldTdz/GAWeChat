import { Component, OnInit, Injector } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { DemandDetail, DemandForecastHead, QuestionRecord, Questionnaire } from '@shared/entity/marketting';
import { DemandForecastServiceProxy, PagedResultDtoOfDemandDetail, QuestionnaireServiceProxy } from '@shared/service-proxies/marketing-service';
import { Router, ActivatedRoute } from '@angular/router';
import { Parameter } from '@shared/service-proxies/entity';

@Component({
    moduleId: module.id,
    selector: 'retail-answer-record',
    templateUrl: 'retail-answer-record.component.html',
    styleUrls: ['retail-answer-record.component.scss']
})
export class RetailAnswerRecordComponent extends AppComponentBase implements OnInit {
    userId: string;
    id: string;
    loading = false;
    info: QuestionRecord = new QuestionRecord();
    answerDetailList: Questionnaire[] = [];


    constructor(injector: Injector
        ,private questionnaireService:QuestionnaireServiceProxy
        , private router: Router, private actRouter: ActivatedRoute) {
        super(injector);
        this.id = this.actRouter.snapshot.params['id'];
        this.userId = this.actRouter.snapshot.params['uId'];
    }
    ngOnInit(): void {
        if (this.id) {
            this.getInfo();
            this.refreshData();
        }
    }
    getInfo() {
        this.questionnaireService.getRetailQuetionRecordHead(this.userId,this.id).subscribe((result: QuestionRecord) => {
            this.info = result;
        });
    }

    refreshData() {
        this.loading = true;
        this.questionnaireService.getAnswerRecordsByRetailerId(this.userId,this.id).subscribe((result: Questionnaire[]) => {
            this.loading = false;
            this.answerDetailList = result;
            //console.log(this.answerDetailList);
            
        });
    }

    getParameter(): Parameter[] {
        var arry = [];
        arry.push(Parameter.fromJS({ key: 'demandForecastId', value: this.id }));
        arry.push(Parameter.fromJS({ key: 'userId', value: this.userId }));
        return arry;
    }

    return() {
        this.router.navigate(['admin/customer/retail-statistics', this.userId]);
    }

    noThing(){

    }
}
