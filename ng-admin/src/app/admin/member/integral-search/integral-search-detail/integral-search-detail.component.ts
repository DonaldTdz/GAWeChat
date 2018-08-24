import { Component, OnInit, Injector } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { ActivatedRoute, Router } from '@angular/router';
import { NzModalService } from 'ng-zorro-antd';
import { IntegralServiceProxy, PagedResultDtoOfIntegralDetails } from '@shared/service-proxies/member';
import { IntegralDetails } from '@shared/entity/member';
import { WechatUser, PurchaseRecord } from '@shared/entity/wechat';
import { WechatUserServiceProxy, PurchaseRecordServiceProxy, PagedResultDtoOfPurchaseRecords } from '@shared/service-proxies/wechat-service';
import { WeChatUser } from '@shared/entity/customer/wechat-user';
import { AppConsts } from '@shared/AppConsts';
import { Parameter } from '@shared/service-proxies/entity';

@Component({
    moduleId: module.id,
    selector: 'integral-search-detail',
    templateUrl: 'integral-search-detail.component.html',
    styleUrls: ['integral-search-detail.component.scss']
})
export class IntegralSearchDetailComponent extends AppComponentBase implements OnInit {
    integralDetails: IntegralDetails[] = [];
    loading = false;
    recordLoading = false;
    openId: string;
    host: string;
    weChatUser: WechatUser = new WechatUser();
    queryRecord: any = {
        pageIndex: 1,
        pageSize: 10,
        skipCount: function () { return (this.pageIndex - 1) * this.pageSize; },
        total: 0,
        sorter: '',
        status: -1,
        statusList: []
    };
    purchaseRecord: PurchaseRecord[] = [];
    constructor(injector: Injector, private integralService: IntegralServiceProxy,
        private Acroute: ActivatedRoute, private modal: NzModalService,
        private wechatUserService: WechatUserServiceProxy,
        private purchaseRecordService: PurchaseRecordServiceProxy,
        private router: Router) {
        super(injector);
        this.openId = this.Acroute.snapshot.params['openId'];
        this.host = AppConsts.remoteServiceBaseUrl;
    }
    ngOnInit(): void {
        this.getUserInfo();
    }

    getUserInfo() {
        this.wechatUserService.getUserInfoAsync(this.openId, this.host).subscribe((result: WechatUser) => {
            this.weChatUser = result;
            this.getSingleIntegral();
            this.getPurchaseRecordsById();
        });
    }

    getPurchaseRecordsById() {
        this.recordLoading = true;
        this.purchaseRecordService.getGetPagedPurchaseRecordsByIdAsync(this.queryRecord.skipCount(), this.queryRecord.pageSize, this.openId).subscribe((result: PagedResultDtoOfPurchaseRecords) => {
            this.recordLoading = false;
            this.purchaseRecord = result.items;
            this.queryRecord.total = result.totalCount;
        });
    }
    getSingleIntegral() {
        this.loading = true;
        this.integralService.getIntegralDetailsById(this.query.skipCount(), this.query.pageSize, this.openId).subscribe((result: PagedResultDtoOfIntegralDetails) => {
            this.loading = false;
            this.integralDetails = result.items;
            this.query.total = result.totalCount;
        });
    }

    return() {
        this.router.navigate(['admin/member/integral-search']);
    }
}
