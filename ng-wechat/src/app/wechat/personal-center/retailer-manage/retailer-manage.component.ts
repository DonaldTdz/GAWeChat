import { Component, OnInit, Injector, ViewEncapsulation } from '@angular/core';
import { AppComponentBase } from '../../components/app-component-base';
import { Router } from '@angular/router';
import { WechatUserService } from '../../../services';

@Component({
    moduleId: module.id,
    selector: 'retailer-manage',
    templateUrl: 'retailer-manage.component.html',
    styleUrls: ['retailer-manage.component.scss'],
    encapsulation: ViewEncapsulation.None,
})
export class RetailerManageComponent extends AppComponentBase implements OnInit {

    retailerList: any[] = [];
    status: boolean = false;
    isRetailer: boolean = false;

    constructor(injector: Injector, private router: Router, private wechatUserService: WechatUserService) {
        super(injector);
    }
    ngOnInit() {
        this.getRetailerList();
    }

    getRetailerList() {
        this.wechatUserService.getEmpRetailerList(this.settingsService.openId).subscribe(result => {
            this.retailerList = result;
        });
    }

    goDetail(id: string) {
        this.router.navigate(['/emp-retailers/retailer-demand', { id: id }]);
    }
}
