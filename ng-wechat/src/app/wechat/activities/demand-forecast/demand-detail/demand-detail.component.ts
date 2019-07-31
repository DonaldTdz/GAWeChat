import { Component, ViewEncapsulation, OnInit, Injector } from '@angular/core';
import { AppComponentBase } from '../../../components/app-component-base';
import { Router } from '@angular/router';
import { DemandForecastService } from '../../../../services';
import { DemandDetail } from '../../../../services/model';

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
    constructor(injector: Injector, private router: Router, private demandService: DemandForecastService) {
        super(injector);
    }
    ngOnInit() {
        // this.getDemandForecastList();
    }
}