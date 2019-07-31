import { Component, ViewEncapsulation, OnInit, Injector } from '@angular/core';
import { AppComponentBase } from '../../components/app-component-base';
import { Router } from '@angular/router';
import { DemandForecastService } from '../../../services';
import { DemandForecast } from '../../../services/model';

@Component({
    moduleId: module.id,
    selector: 'demand-forecast',
    templateUrl: 'demand-forecast.component.html',
    styleUrls: ['demand-forecast.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class DemandForecastComponent extends AppComponentBase implements OnInit {

    demandList: DemandForecast[] = [];
    constructor(injector: Injector, private router: Router, private demandService: DemandForecastService) {
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

    goDetail(id: string) {
        this.router.navigate(['/demand-forecasts/demand-detail', { id: id }]);
    }
}
