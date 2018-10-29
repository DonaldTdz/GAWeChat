import { Component, ViewEncapsulation, OnInit, Injector, ViewChild } from '@angular/core';
import { AppComponentBase } from '../../components/app-component-base';
import { PageModel, WechatUser, PurchaseRecord } from '../../../services/model';
import { Router } from '@angular/router';
import { PurchaserecordService, AppConsts } from '../../../services';
import { InfiniteLoaderComponent } from 'ngx-weui';
import { ActivatedRoute } from '@angular/router';
import { timer } from 'rxjs/observable/timer';

@Component({
    moduleId: module.id,
    selector: 'purchaserecord',
    templateUrl: 'purchaserecord.component.html',
    styleUrls: ['purchaserecord.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class PurchaserecordComponent extends AppComponentBase implements OnInit {
    @ViewChild(InfiniteLoaderComponent) il;

    pageModel: PageModel = new PageModel(); // 分页信息
    user: WechatUser;
    purchaseRecordList: PurchaseRecord[] = [];
    pageType: string = this.route.snapshot.params['pageType'];
    hostUrl: string = AppConsts.remoteServiceBaseUrl;
    tittleType: boolean = false;

    constructor(injector: Injector, private purchaserecordService: PurchaserecordService, private route: ActivatedRoute, private router: Router) {
        super(injector);
    }
    ngOnInit() {
        this.pageModel.isLast = false;
        this.settingsService.getUser().subscribe(result => {
            this.user = result;
        });
        if (this.pageType == 'shopEvaluation') {
            this.tittleType = true;
            this.GetWXNotEvaluationByIdAsync();
        } else {
            this.tittleType = false;
            this.GetPagedPurchaseRecord();
        }
    }
    onLoadMore(comp: InfiniteLoaderComponent) {
        if (this.pageModel.isLast) {
            comp.setFinished();
            return;
        }
        timer(1500).subscribe(() => {
            this.pageModel.pageIndex++;
            if (this.pageType == 'shopEvaluation') {
                this.tittleType = true;
                this.GetWXNotEvaluationByIdAsync();
            } else {
                this.tittleType = false;
                this.GetPagedPurchaseRecord();
            }
            comp.resolveLoading();
        });
    }

    // onLoadMore(comp: InfiniteLoaderComponent) {
    //     this.pageModel.pageIndex++;
    //     if (this.pageModel.isLast) {
    //         comp.setFinished();
    //         return;
    //     }
    //     this.GetPagedPurchaseRecord();
    //     comp.resolveLoading();
    // }

    //购买记录分页处理
    GetPagedPurchaseRecord() {
        let params: any = {};
        if (this.settingsService.tenantId) {
            params.tenantId = this.settingsService.tenantId;
        }
        params.openId = this.settingsService.openId;
        params.pageIndex = this.pageModel.pageIndex;
        params.pageSize = this.pageModel.pageSize;
        this.purchaserecordService.GetPurchaseRecordById(params).subscribe(result => {
            this.purchaseRecordList.push(...result);
            if (result && result.length < this.pageModel.pageSize) {
                this.pageModel.isLast = true;
            }
        });
    }

    //购买记录暂不分页
    // GetPagedPurchaseRecord() {
    //     let params: any = {};
    //     if (this.settingsService.tenantId) {
    //         params.tenantId = this.settingsService.tenantId;
    //     }
    //     params.openId = this.settingsService.openId;
    //     this.purchaserecordService.GetPurchaseRecordById(params).subscribe(result => {
    //         this.purchaseRecordList = result;
    //     });
    // }

    //评价暂不分页
    // GetWXNotEvaluationByIdAsync() {
    //     let params: any = {};
    //     if (this.settingsService.tenantId) {
    //         params.tenantId = this.settingsService.tenantId;
    //     }
    //     params.openId = this.settingsService.openId;
    //     this.purchaserecordService.GetWXNotEvaluationByIdAsync(params).subscribe(result => {
    //         this.purchaseRecordList = result;
    //     });
    // }

    GetWXNotEvaluationByIdAsync() {
        let params: any = {};
        if (this.settingsService.tenantId) {
            params.tenantId = this.settingsService.tenantId;
        }
        params.openId = this.settingsService.openId;
        params.pageIndex = this.pageModel.pageIndex;
        params.pageSize = this.pageModel.pageSize;
        this.purchaserecordService.GetWXNotEvaluationByIdAsync(params).subscribe(result => {
            this.purchaseRecordList.push(...result);
            if (result && result.length < this.pageModel.pageSize) {
                this.pageModel.isLast = true;
            }
        });
    }

    goEvaluation(productId: string, purchaseRecordId: string, pageType: string) {
        this.router.navigate(['/shopevaluations/evaluation-detail', { id: purchaseRecordId, productId: productId }]);
    }
    goDetail(productId: string, purchaseRecordId: string, pageType: string) {
        this.router.navigate(['/shopevaluations/evaluation-detail', { id: purchaseRecordId, productId: productId, pageType: 'detail' }]);
    }
    goFindGoods() {
        this.router.navigate(['/goodses/goods']);
    }
}
