import { Component, ViewEncapsulation, OnInit, Injector } from '@angular/core';
import { AppComponentBase } from '../../components/app-component-base';
import { Router } from '@angular/router';
import { DemandForecastService } from '../../../services';
import { DemandForecast } from '../../../services/model';
import { ToptipsService } from 'ngx-weui';

@Component({
    moduleId: module.id,
    selector: 'demand-forecast',
    templateUrl: 'demand-forecast.component.html',
    styleUrls: ['demand-forecast.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class DemandForecastComponent extends AppComponentBase implements OnInit {

    demandList: DemandForecast[] = [];
    status: boolean = false;
    isRetailer: boolean = false;
    constructor(injector: Injector, private router: Router, private demandService: DemandForecastService, private srv: ToptipsService) {
        super(injector);
    }
    ngOnInit() {
        this.getDemandForecastList();
    }

    getDemandForecastList() {
        this.demandService.getDemandListAsync().subscribe(result => {
            this.demandList = result;
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
                    this.router.navigate(['/demand-forecasts/demand-detail', { id: id, status: this.status }]);
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
