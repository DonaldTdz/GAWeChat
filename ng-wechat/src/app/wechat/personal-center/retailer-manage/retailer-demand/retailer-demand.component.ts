import { Component, ViewEncapsulation, OnInit, Injector } from '@angular/core';
import { AppComponentBase } from '../../../components/app-component-base';
import { DemandForecast } from '../../../../services/model';
import { Router, ActivatedRoute } from '@angular/router';
import { DemandForecastService } from '../../../../services';

@Component({
    moduleId: module.id,
    selector: 'retailer-demand',
    templateUrl: 'retailer-demand.component.html',
    styleUrls: ['retailer-demand.component.scss'],
    encapsulation: ViewEncapsulation.None,
})
export class RetailerDemandComponent extends AppComponentBase implements OnInit {

    demandList: DemandForecast[] = [];
    userId: string = this.route.snapshot.params['id'];
    constructor(injector: Injector
        , private router: Router
        , private route: ActivatedRoute
        , private demandService: DemandForecastService) {
        super(injector);
    }
    ngOnInit() {
        this.getRetailDemandedList();
    }

    getRetailDemandedList() {
        this.demandService.getRetailDemandedListAsync(this.userId).subscribe(result => {
            this.demandList = result;
        });
    }

    goDetail(id: string) {
        this.router.navigate(['/demand-forecasts/demand-detail', { id: id, status: false }]);
    }
}
