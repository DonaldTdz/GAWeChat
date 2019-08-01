import { Component, OnInit, Injector } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { Router, ActivatedRoute } from '@angular/router';
import { NzModalService } from 'ng-zorro-antd';
import { DemandForecast } from '@shared/entity/marketting';
import { PagedResultDtoOfDemandForecast, DemandForecastServiceProxy } from '@shared/service-proxies/marketing-service';
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
    demandForecastList: DemandForecast[] = [];

    constructor(injector: Injector, private demandForecastService: DemandForecastServiceProxy, private router: Router,
        private modal: NzModalService, private ActRouter: ActivatedRoute) {
        super(injector);
        this.userId = this.ActRouter.snapshot.params['uId'];
    }
    ngOnInit(): void {
        if (this.userId) {
            this.getRetailDemandList();
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

    return() {
        this.router.navigate(['admin/customer/retail-customer']);
    }
}
