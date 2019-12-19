import { Component, OnInit, Injector, Input } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { AppConsts } from '@shared/AppConsts';
import { DemandForecast, DemandDetail } from '@shared/entity/marketting';
import { DemandForecastServiceProxy, PagedResultDtoOfDemandForecast, PagedResultDtoOfDemandDetail } from '@shared/service-proxies/marketing-service';
import { Router } from '@angular/router';
import { NzModalService, UploadFile } from 'ng-zorro-antd';
import { Parameter } from '@shared/service-proxies/entity';

@Component({
    moduleId: module.id,
    selector: 'demand-list',
    templateUrl: 'demand-list.component.html'
})
export class DemandListComponent extends AppComponentBase implements OnInit {
    @Input() id: string;
    @Input() isPublish: boolean;
    loading = false;
    exportExcelUrl: string;
    exportLoading = false;
    search: any = { status: true };
    host: string = AppConsts.remoteServiceBaseUrl;
    uploadLoading = false;

    demandDetailList: DemandDetail[] = [];
    constructor(injector: Injector, private demandForecastService: DemandForecastServiceProxy, private router: Router,
        private modal: NzModalService, ) {
        super(injector);
    }
    ngOnInit(): void {
        // console.log(this.id);
        // if (this.id) {
        //     this.refreshData();
        // }
    }

    refreshData() {
        if (this.id) {
            this.loading = true;
            this.demandForecastService.getDemandDetailList(this.query.skipCount(), this.query.pageSize, this.getParameter()).subscribe((result: PagedResultDtoOfDemandDetail) => {
                this.loading = false;
                this.demandDetailList = result.items
                this.query.total = result.totalCount;
            });
        }
    }
    getParameter(): Parameter[] {
        var arry = [];
        arry.push(Parameter.fromJS({ key: 'DemandForecastId', value: this.id }));
        return arry;
    }

    beforeExcelUpload = (file: UploadFile): boolean => {
        if (!this.id) {
            this.notify.error('请先填写需求预测基本信息');
            return false;
        }
        if (this.uploadLoading) {
            this.notify.info('上次数据导入还未完成');
            return false;
        }
        if (!file.name.includes('.xlsx')) {
            this.notify.error('上传文件必须是Excel文件(*.xlsx)');
            return false;
        }
        this.uploadLoading = true;
        return true;
    }

    handleChange = (info: { file: UploadFile }): void => {
        if (info.file.status === 'error') {
            this.notify.error('上传文件异常，请重试');
            this.uploadLoading = false;
        }
        if (info.file.status === 'done') {
            this.uploadLoading = true;
            let input: any = {}
            input.DemandForecastId = this.id;
            this.demandForecastService.importDemandDetailExcelAsync(input).subscribe((res) => {
                if (res && res.code == 0) {
                    this.notify.success('导入成功');
                    this.refreshData();
                } else {
                    this.notify.error('导入失败');
                }
                this.uploadLoading = false;
            });
        }
    }
}
