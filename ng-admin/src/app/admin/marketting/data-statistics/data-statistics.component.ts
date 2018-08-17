import { Component, OnInit, Injector } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { Parameter, RetailCustomer } from '@shared/service-proxies/entity';
import { Router } from '@angular/router';
import { NzModalService } from 'ng-zorro-antd';
import { AppConsts } from '@shared/AppConsts';
import { RetailCustomerServiceProxy, PagedResultDtoOfRetailCustomer } from '@shared/service-proxies/customer-service';

@Component({
    moduleId: module.id,
    selector: 'data-statistics',
    templateUrl: 'data-statistics.component.html',
    styleUrls: ['data-statistics.component.scss']
})
export class DataStatisticsComponent extends AppComponentBase implements OnInit {
    dataList: RetailCustomer[] = [];
    loading = false;
    exportLoading = false;
    search: any = {};
    sortMap = {
        registeredShop: null,
        scanCode: null,
        scanNum: null,
        scanCount: null,
        consumerIntegral: null,
        shopIntegral: null
    };
    sortregisteredShopTotal = null;
    sortscanCodeTotal = null;
    sortscanNumTotal = null;
    sortscanCountTotal = null;
    sortconsumerIntegralTotal = null;
    sortshopIntegralTotal = null;

    constructor(injector: Injector, private router: Router, private modal: NzModalService,
        private retailService: RetailCustomerServiceProxy) {
        super(injector);
    }
    ngOnInit(): void {
        this.refreshData();
    }

    sort(value, para: string) {
        if (para == 'registeredShop') {
            this.sortregisteredShopTotal = value;
            this.sortscanCodeTotal = null;
            this.sortscanNumTotal = null;
            this.sortscanCountTotal = null;
            this.sortconsumerIntegralTotal = null;
            this.sortshopIntegralTotal = null;

            this.sortMap.scanCode = null;
            this.sortMap.scanNum = null;
            this.sortMap.scanCount = null;
            this.sortMap.consumerIntegral = null;
            this.sortMap.shopIntegral = null;
            this.refreshData();
        } else if ((para == 'scanCode')) {
            this.sortscanCodeTotal = value;
            this.sortregisteredShopTotal = null;
            this.sortscanNumTotal = null;
            this.sortscanCountTotal = null;
            this.sortconsumerIntegralTotal = null;
            this.sortshopIntegralTotal = null;

            this.sortMap.registeredShop = null;
            this.sortMap.scanNum = null;
            this.sortMap.scanCount = null;
            this.sortMap.consumerIntegral = null;
            this.sortMap.shopIntegral = null;
            this.refreshData();
        } else if ((para == 'scanNum')) {
            this.sortscanNumTotal = value;
            this.sortscanCodeTotal = null;
            this.sortregisteredShopTotal = null;
            this.sortscanCountTotal = null;
            this.sortconsumerIntegralTotal = null;
            this.sortshopIntegralTotal = null;

            this.sortMap.registeredShop = null;
            this.sortMap.scanCode = null;
            this.sortMap.scanCount = null;
            this.sortMap.consumerIntegral = null;
            this.sortMap.shopIntegral = null;
            this.refreshData();
        } else if ((para == 'scanCount')) {
            this.sortscanCountTotal = value;
            this.sortscanNumTotal = null;
            this.sortscanCodeTotal = null;
            this.sortregisteredShopTotal = null;
            this.sortconsumerIntegralTotal = null;
            this.sortshopIntegralTotal = null;

            this.sortMap.registeredShop = null;
            this.sortMap.scanCode = null;
            this.sortMap.scanNum = null;
            this.sortMap.consumerIntegral = null;
            this.sortMap.shopIntegral = null;
            this.refreshData();
        } else if ((para == 'consumerIntegral')) {
            this.sortconsumerIntegralTotal = value;
            this.sortscanCountTotal = null;
            this.sortscanNumTotal = null;
            this.sortscanCodeTotal = null;
            this.sortregisteredShopTotal = null;
            this.sortshopIntegralTotal = null;

            this.sortMap.registeredShop = null;
            this.sortMap.scanCode = null;
            this.sortMap.scanNum = null;
            this.sortMap.scanCount = null;
            this.sortMap.shopIntegral = null;
            this.refreshData();
        } else { // shopIntegral
            this.sortshopIntegralTotal = value;
            this.sortscanCountTotal = null;
            this.sortscanNumTotal = null;
            this.sortscanCodeTotal = null;
            this.sortregisteredShopTotal = null;
            this.sortconsumerIntegralTotal = null;

            this.sortMap.registeredShop = null;
            this.sortMap.scanCode = null;
            this.sortMap.scanNum = null;
            this.sortMap.scanCount = null;
            this.sortMap.consumerIntegral = null;
            this.refreshData();
        }
    }
    refreshData(reset = false, search?: boolean) {
        if (reset) {
            this.query.pageIndex = 1;
            this.search = {};
            this.sortregisteredShopTotal = null;
            this.sortscanCodeTotal = null;
            this.sortscanNumTotal = null;
            this.sortscanCountTotal = null;
            this.sortconsumerIntegralTotal = null;
            this.sortshopIntegralTotal = null;
            this.sortMap = {
                registeredShop: null,
                scanCode: null,
                scanNum: null,
                scanCount: null,
                consumerIntegral: null,
                shopIntegral: null
            };
        }
        if (search) {
            this.query.pageIndex = 1;
        }
        this.loading = true;
        this.retailService.getDataStatisticsAsync(this.getParameter()).subscribe((result: PagedResultDtoOfRetailCustomer) => {
            this.loading = false;
            this.dataList = result.items;
            // this.query.total = result.totalCount;
        })
    }

    getParameter(): Parameter[] {
        var arry = [];
        arry.push(Parameter.fromJS({ key: 'OrganizationName', value: this.search.OrganizationName }));
        arry.push(Parameter.fromJS({ key: 'sortregisteredShopTotal', value: this.sortregisteredShopTotal }));
        arry.push(Parameter.fromJS({ key: 'sortscanCodeTotal', value: this.sortscanCodeTotal }));
        arry.push(Parameter.fromJS({ key: 'sortscanNumTotal', value: this.sortscanNumTotal }));
        arry.push(Parameter.fromJS({ key: 'sortscanCountTotal', value: this.sortscanCountTotal }));
        arry.push(Parameter.fromJS({ key: 'sortconsumerIntegralTotal', value: this.sortconsumerIntegralTotal }));
        arry.push(Parameter.fromJS({ key: 'sortshopIntegralTotal', value: this.sortshopIntegralTotal }));
        return arry;
    }

    exhibitionDetails(id: string) {
        this.router.navigate(['admin/marketting/exhibition-detail', id]);
    }

    // exportExcel() {
    //     this.exportLoading = true;
    //     this.exhibitionService.exportExcel(
    //         {
    //             OrganizationName: this.search.OrganizationName,
    //             sortregisteredShopTotal: this.sortregisteredShopTotal,
    //             sortscanCodeTotal: this.sortscanCodeTotal,
    //             sortscanNumTotal: this.sortscanNumTotal,
    //             sortscanCountTotal: this.sortscanCountTotal,
    //             sortconsumerIntegralTotal: this.sortconsumerIntegralTotal,
    //             sortshopIntegralTotal: this.sortshopIntegralTotal,
    //         }).subscribe(result => {
    //             if (result.code == 0) {
    //                 var url = AppConsts.remoteServiceBaseUrl + result.data;
    //                 document.getElementById('aExhibitionShopExcelUrl').setAttribute('href', url);
    //                 document.getElementById('btnExhibitionShopHref').click();
    //             } else {
    //                 this.notify.error(result.msg);
    //             }
    //             this.exportLoading = false;
    //         });
    // }

}
