import { Component, ViewEncapsulation, OnInit, Injector, ViewChild } from '@angular/core';
import { AppComponentBase } from '../../components/app-component-base';
import { PageModel, IntegralDetail, WechatUser, MemberConfigs, ConfigCode } from '../../../services/model';
import { Router } from '@angular/router';
import { IntegralDetailService } from '../../../services';
import { InfiniteLoaderComponent } from 'ngx-weui';
import { ActivatedRoute } from '@angular/router';
import { timer } from 'rxjs/observable/timer';

@Component({
    moduleId: module.id,
    selector: 'integral',
    templateUrl: 'integral.component.html',
    styleUrls: ['integral.component.scss']
})

export class IntegralComponent extends AppComponentBase implements OnInit {
    integralDetailList: IntegralDetail[] = [];
    pageModel: PageModel = new PageModel(); // 分页信息
    user: WechatUser;
    config: MemberConfigs[] = [];
    configCode: ConfigCode = new ConfigCode();
    @ViewChild(InfiniteLoaderComponent) il;

    constructor(injector: Injector, private integralDetailService: IntegralDetailService, private route: ActivatedRoute, private router: Router) {
        super(injector);
    }

    ngOnInit() {
        this.pageModel.isLast = false;
        this.settingsService.getUser().subscribe(result => {
            this.user = result;
        });
        this.getMemberConfigsByTenantId();
        this.GetPagedIntegralDetail();
    }

    onLoadMore(comp: InfiniteLoaderComponent) {
        this.pageModel.pageIndex++;
        if (this.pageModel.isLast) {
            comp.setFinished();
            return;
        }

        timer(1500).subscribe(() => {
            this.pageModel.pageIndex++;
            this.GetPagedIntegralDetail();
            comp.resolveLoading();
        });
    }

    getMemberConfigsByTenantId() {
        let params: any = {};
        if (this.settingsService.tenantId) {
            params.tenantId = this.settingsService.tenantId;
        }
        this.integralDetailService.GetMemberConfigsByTenantId(params).subscribe(result => {
            this.config = result;
            this.config.map(i => {
                if (i.code == 5) {
                    this.configCode.fValue = i.value;
                    this.configCode.fId = i.id;
                }
                if (i.code == 3) {
                    this.configCode.rcValue = i.value;
                    this.configCode.rcId = i.id;
                }
                if (i.code == 2) {
                    this.configCode.eValue = i.value;
                    this.configCode.eId = i.id;
                }
                if (i.code == 1) {
                    this.configCode.cValue = i.value;
                    this.configCode.cId = i.id;
                }
            })
        });
    }

    GetPagedIntegralDetail() {
        let params: any = {};
        if (this.settingsService.tenantId) {
            params.tenantId = this.settingsService.tenantId;
        }
        params.openId = this.settingsService.openId;
        params.pageIndex = this.pageModel.pageIndex;
        params.pageSize = this.pageModel.pageSize;
        this.integralDetailService.GetIntegralDetailById(params).subscribe(result => {
            this.integralDetailList.push(...result);
            if (result && result.length < this.pageModel.pageSize) {
                this.pageModel.isLast = true;
            }
        });
    }
}
