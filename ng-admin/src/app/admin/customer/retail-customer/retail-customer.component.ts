import { Component, OnInit, Injector } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { RetailCustomer, Parameter } from '@shared/service-proxies/entity';
import { RetailCustomerServiceProxy, PagedResultDtoOfRetailCustomer } from '@shared/service-proxies/customer-service';
import { NzModalService, UploadFile } from 'ng-zorro-antd';
import { Router } from '@angular/router';
import { AppConsts } from '@shared/AppConsts';

@Component({
    moduleId: module.id,
    selector: 'retail-customer',
    templateUrl: 'retail-customer.component.html',
})
export class RetailCustomerComponent extends AppComponentBase implements OnInit {
    loading = false;
    exportExcelUrl: string;
    exportLoading = false;
    search: any = { status: true };
    host: string = AppConsts.remoteServiceBaseUrl;
    uploadLoading = false;
    status = [
        { text: '全部', value: 0, type: '123' },
        { text: '有效', value: true, type: 'success' },
        { text: '无效', value: false, type: 'default' },
    ];
    scales = [
        { text: '小', value: 1 },
        { text: '中', value: 2 },
        { text: '大', value: 3 },
    ];
    markets = [
        { text: '乡村', value: 1 },
        { text: '城镇', value: 2 },
    ];
    retailCustomer: RetailCustomer[] = [];
    constructor(injector: Injector, private retailService: RetailCustomerServiceProxy, private router: Router,
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
        this.retailService.getAll(this.query.skipCount(), this.query.pageSize, this.getParameter()).subscribe((result: PagedResultDtoOfRetailCustomer) => {
            this.loading = false;
            let status = 0;
            this.retailCustomer = result.items.map(i => {
                if (i.isAction) {
                    status = 1;
                } else {
                    status = 2;
                }
                const statusItem = this.status[status];
                i.activeText = statusItem.text;
                i.activeType = statusItem.type;
                return i;
            });
            this.query.total = result.totalCount;
        })
    }
    getParameter(): Parameter[] {
        var arry = [];
        arry.push(Parameter.fromJS({ key: 'Name', value: this.search.name }));
        arry.push(Parameter.fromJS({ key: 'Scale', value: this.search.scale }));
        arry.push(Parameter.fromJS({ key: 'Markets', value: this.search.market }));
        arry.push(Parameter.fromJS({ key: 'Status', value: this.search.isAction === 0 ? null : this.search.isAction }));
        return arry;
    }
    editRetail(id: string) {
        this.router.navigate(['admin/customer/retail-detail', id]);
    }

    goStatistics(id: string) {
        this.router.navigate(['admin/customer/retail-statistics', id]);
    }

    /**
     * 删除零售户
     */
    delete(id: string, TplContent) {
        this.modal.confirm({
            content: TplContent,
            cancelText: '否',
            okText: '是',
            onOk: () => {
                this.retailService.delete(id).subscribe(() => {
                    this.notify.info(this.l('删除成功！'));
                    this.refreshData();
                });
            }
        })
    }
    createRetail() {
        this.router.navigate(['admin/customer/retail-detail']);
    }

    /**
     * 导出档级
     */
    exportExcel() {
        this.exportLoading = true;
        this.retailService.exportRetailerLevelExcel({ name: this.search.name, scale: this.search.scale, markets: this.search.market, status: this.search.isAction === 0 ? null : this.search.isAction }).subscribe(result => {
            if (result.code == 0) {
                var url = AppConsts.remoteServiceBaseUrl + result.data;
                document.getElementById('aRetailExcelUrl').setAttribute('href', url);
                document.getElementById('btnRetailHref').click();
            } else {
                this.notify.error(result.msg);
            }
            this.exportLoading = false;
        });
    }
    /**
     * 导出详情
     * @memberof RetailCustomerComponent
     */
    exportExcelAll() {
        this.exportLoading = true;
        this.retailService.exportRetailerAllExcel({ name: this.search.name, scale: this.search.scale, markets: this.search.market, status: this.search.isAction === 0 ? null : this.search.isAction }).subscribe(result => {
            if (result.code == 0) {
                var url = AppConsts.remoteServiceBaseUrl + result.data;
                document.getElementById('aRetailAllExcelUrl').setAttribute('href', url);
                document.getElementById('btnRetailAllHref').click();
            } else {
                this.notify.error(result.msg);
            }
            this.exportLoading = false;
        });
    }
    beforeExcelUpload = (file: UploadFile): boolean => {
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
            this.retailService.importRetailerLevelExcelAsync().subscribe((res) => {
                if (res && res.code == 0) {
                    this.notify.success('导入成功');
                    this.refreshData(false, true);
                } else {
                    this.notify.error('导入失败');
                }
                this.uploadLoading = false;
            });
        }
    }
}
