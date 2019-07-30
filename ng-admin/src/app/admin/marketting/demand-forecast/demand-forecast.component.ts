import { Component, OnInit, Injector } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { AppConsts } from '@shared/AppConsts';
import { RetailCustomer, Parameter } from '@shared/service-proxies/entity';
import { Router } from '@angular/router';
import { NzModalService } from 'ng-zorro-antd';
import { DemandForecast } from '@shared/entity/marketting';
import { DemandForecastServiceProxy, PagedResultDtoOfDemandForecast } from '@shared/service-proxies/marketing-service';

@Component({
    moduleId: module.id,
    selector: 'demand-forecast',
    templateUrl: 'demand-forecast.component.html',
})
export class DemandForecastComponent extends AppComponentBase implements OnInit {
    loading = false;
    exportExcelUrl: string;
    exportLoading = false;
    search: any = { status: true };
    host: string = AppConsts.remoteServiceBaseUrl;
    uploadLoading = false;

    demandForecastList: DemandForecast[] = [];
    constructor(injector: Injector, private demandForecastService: DemandForecastServiceProxy, private router: Router,
        private modal: NzModalService, ) {
        super(injector);
    }
    ngOnInit(): void {
        this.search = { isAction: true }
        this.refreshData();
    }

    refreshData(reset = false, search?: boolean) {
        if (reset) {
            this.query.pageIndex = 1;
            this.search = { isAction: 0 }
        }
        if (search) {
            this.query.pageIndex = 1;
        }
        this.loading = true;
        this.demandForecastService.getAll(this.query.skipCount(), this.query.pageSize, this.getParameter()).subscribe((result: PagedResultDtoOfDemandForecast) => {
            this.loading = false;
            this.demandForecastList = result.items
            this.query.total = result.totalCount;
        })
    }
    getParameter(): Parameter[] {
        var arry = [];
        // arry.push(Parameter.fromJS({ key: 'Name', value: this.search.name }));
        // arry.push(Parameter.fromJS({ key: 'Scale', value: this.search.scale }));
        // arry.push(Parameter.fromJS({ key: 'Markets', value: this.search.market }));
        // arry.push(Parameter.fromJS({ key: 'Status', value: this.search.isAction === 0 ? null : this.search.isAction }));
        return arry;
    }

    detail(id: string) {
        this.router.navigate(['admin/marketting/demand-detai', id])
    }
    /**
     * 删除零售户
     */
    delete(retail: RetailCustomer, TplContent) {
        this.modal.confirm({
            content: TplContent,
            cancelText: '否',
            okText: '是',
            onOk: () => {
                // this.demandForecastService.delete(retail.id).subscribe(() => {
                //     this.notify.info(this.l('删除成功！'));
                //     this.refreshData();
                // });
            }
        })
    }
    create() {
        this.router.navigate(['admin/marketting/demand-detai']);
    }
}
