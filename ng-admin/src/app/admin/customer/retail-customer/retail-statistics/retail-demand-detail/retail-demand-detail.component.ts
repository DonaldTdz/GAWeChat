import { Component, OnInit, Injector } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { DemandDetail, DemandForecastHead } from '@shared/entity/marketting';
import { DemandForecastServiceProxy, PagedResultDtoOfDemandDetail } from '@shared/service-proxies/marketing-service';
import { Router, ActivatedRoute } from '@angular/router';
import { Parameter } from '@shared/service-proxies/entity';

@Component({
    moduleId: module.id,
    selector: 'retail-demand-detail',
    templateUrl: 'retail-demand-detail.component.html'
})
export class RetailDemandDetailComponent extends AppComponentBase implements OnInit {
    userId: string;
    id: string;
    loading = false;
    info: DemandForecastHead = new DemandForecastHead();
    demandDetailList: DemandDetail[] = [];

    constructor(injector: Injector, private demandForecastService: DemandForecastServiceProxy, private router: Router, private actRouter: ActivatedRoute) {
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
        this.demandForecastService.getHeadInfoAsync(this.getParameter()).subscribe((result: DemandForecastHead) => {
            this.info = result;
        });
    }

    refreshData() {
        this.loading = true;
        this.demandForecastService.getDetailRecordById(this.query.skipCount(), this.query.pageSize, this.getParameter()).subscribe((result: PagedResultDtoOfDemandDetail) => {
            this.loading = false;
            this.demandDetailList = result.items
            this.query.total = result.totalCount;
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
}
