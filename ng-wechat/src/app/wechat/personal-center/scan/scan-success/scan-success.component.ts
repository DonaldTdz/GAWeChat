import { Component, ViewEncapsulation, Injector, OnInit } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { AppComponentBase } from '../../../components/app-component-base';
import { WechatUser } from '../../../../services/model';
import { Router, Params } from '@angular/router';

@Component({
    selector: 'wechat-scan-success',
    templateUrl: './scan-success.component.html',
    styleUrls: ['./scan-success.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class ScanSuccessComponent extends AppComponentBase implements OnInit {

    retailerIntegral: number;
    userIntegral: number;
    gDate: Date = new Date();
    disabled: boolean = true;

    constructor(injector: Injector,
        private router: Router) {
        super(injector);
    }

    ngOnInit() {
        this.activatedRoute.params.subscribe((params: Params) => {
            this.userIntegral = params['userIntegral'];
            this.retailerIntegral = params['retailerIntegral'];
        });
    }

    goBack() {
        this.router.navigate(['/scans/scan']);
    }
    goEvaluation() {
        this.router.navigate(['/purchaserecords/purchaserecord']);
    }
}
