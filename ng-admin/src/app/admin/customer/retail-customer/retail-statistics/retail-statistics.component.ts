import { Component, OnInit, Injector } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { Router, ActivatedRoute } from '@angular/router';
import { NzModalService } from 'ng-zorro-antd';
import { DemandForecast, Questionnaire, QuestionRecord } from '@shared/entity/marketting';
import { PagedResultDtoOfDemandForecast, DemandForecastServiceProxy, QuestionnaireServiceProxy, PagedResultDtoOfQuestionRecord } from '@shared/service-proxies/marketing-service';
import { Parameter } from '@shared/service-proxies/entity';

@Component({
    moduleId: module.id,
    selector: 'retail-statistics',
    templateUrl: 'retail-statistics.component.html'
})
export class RetailStatisticsComponent extends AppComponentBase implements OnInit {
    userId: string;
    searchTitle: string;
    loading = false;
    demandForecastList : DemandForecast[] = [];

    //问卷调查
    queryQuestionRecord: any = {
        pageIndex: 1,
        pageSize: 10,
        skipCount: function () { return (this.pageIndex - 1) * this.pageSize; },
        total: 0,
        sorter: '',
        status: -1,
        statusList: []
    };
    questionRecordList : QuestionRecord[] = [];
    searchQuestionRecordTitle:string;


    constructor(injector: Injector, private demandForecastService: DemandForecastServiceProxy, private router: Router,
        private modal: NzModalService, private ActRouter: ActivatedRoute, 
        private questionnaireService:QuestionnaireServiceProxy) {
        super(injector);
        this.userId = this.ActRouter.snapshot.params['uId'];
    }
    ngOnInit(): void {
        if (this.userId) {
            this.getRetailDemandList();
            this.getQuestionRecordList();
        }
    }

    getRetailDemandList(reset = false, search?: boolean) {
        if (reset) {
            this.query.pageIndex = 1;
            this.searchTitle = '';
        }
        if (search) {
            this.query.pageIndex = 1;
        }
        this.loading = true;
        this.demandForecastService.getRetailDemandListById(this.query.skipCount(), this.query.pageSize, this.getDemandParameter()).subscribe((result: PagedResultDtoOfDemandForecast) => {
            this.loading = false;
            this.demandForecastList = result.items
            this.query.total = result.totalCount;
        })
    }
    getDemandParameter(): Parameter[] {
        var arry = [];
        arry.push(Parameter.fromJS({ key: 'userId', value: this.userId }));
        arry.push(Parameter.fromJS({ key: 'filter', value: this.searchTitle }));
        return arry;
    }

    demandDetail(id: string) {
        this.router.navigate(['admin/customer/retail-record', id, this.userId]);
    }

    //问卷调查
    getQuestionRecordList(reset = false, search?: boolean) {
        if (reset) {
            this.queryQuestionRecord.pageIndex = 1;
            this.searchTitle = '';
        }
        if (search) {
            this.queryQuestionRecord.pageIndex = 1;
        }
        this.loading = true;
        this.questionnaireService.getQuestionRecordByRetailerId(this.queryQuestionRecord.skipCount(), this.queryQuestionRecord.pageSize, this.userId, this.searchQuestionRecordTitle).subscribe((result: PagedResultDtoOfQuestionRecord) => {
            this.loading = false;
            this.questionRecordList = result.items
            console.log(this.questionRecordList);
            
            this.queryQuestionRecord.total = result.totalCount;
        })
    }

    questionRecordDetail(id:string){
        this.router.navigate(['admin/customer/retail-answer-record', id, this.userId]);
    }


    return() {
        this.router.navigate(['admin/customer/retail-customer']);
    }
}
