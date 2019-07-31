import { Component, ViewEncapsulation, OnInit, Injector } from '@angular/core';
import { AppComponentBase } from '../../../components/app-component-base';
import { Router, ActivatedRoute } from '@angular/router';
import { DemandForecastService } from '../../../../services';
import { DemandDetail, ForecastRecordDto } from '../../../../services/model';
import { ToptipsService } from 'ngx-weui';

@Component({
    moduleId: module.id,
    selector: 'demand-detail',
    templateUrl: 'demand-detail.component.html',
    styleUrls: ['demand-detail.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class DemandDetailComponent extends AppComponentBase implements OnInit {
    num = 1;
    detailList: DemandDetail[] = [];
    demandId: string = this.route.snapshot.params['id'];
    statusParams: string = this.route.snapshot.params['status'];
    status: boolean = false;
    constructor(injector: Injector, private router: Router, private demandService: DemandForecastService, private route: ActivatedRoute, private srv: ToptipsService) {
        super(injector);
    }
    ngOnInit() {
        if (this.statusParams == 'true') {
            this.status = true;
            this.getDetailListById();
        } else {
            this.status = false;
            this.getDetailRecordByIdAsync();
        }
    }

    getDetailListById() {
        let params: any = {};
        params.openId = this.settingsService.openId;
        params.demandForecastId = this.demandId;
        this.demandService.getDetailListByIdAsync(params).subscribe(result => {
            this.detailList = result;
        });
    }

    getDetailRecordByIdAsync() {
        let params: any = {};
        params.openId = this.settingsService.openId;
        params.demandForecastId = this.demandId;
        this.demandService.getDetailRecordByIdAsync(params).subscribe(result => {
            this.detailList = result;
        });
    }

    save() {
        let input: any = {};
        input.openId = this.settingsService.openId;
        input.demandForecastId = this.demandId;
        input.list = ForecastRecordDto.fromJSArray(this.detailList);
        this.demandService.createForecastRecordAsync(input).subscribe(result => {
            if (result && result.code == 0) {
                this.router.navigate(['/demand-forecasts/feedback-success']);
            } else {
                this.srv['warn']('提交失败，请重试');
            }
        });
    }
}