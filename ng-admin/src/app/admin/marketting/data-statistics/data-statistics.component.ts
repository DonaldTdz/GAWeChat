import { Component, OnInit, Injector, group } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { Parameter, ShopReportData } from '@shared/service-proxies/entity';
import { Router } from '@angular/router';
import { NzModalService } from 'ng-zorro-antd';
import { RetailCustomerServiceProxy } from '@shared/service-proxies/customer-service';
import { ShopReportDataServiceProxy, PagedResultDtoOfShopReportData } from '@shared/service-proxies/marketing-service/shopReportData-service';
import { AppConsts } from '@shared/AppConsts';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import * as moment from 'moment';

@Component({
    moduleId: module.id,
    selector: 'data-statistics',
    templateUrl: 'data-statistics.component.html',
    styleUrls: ['data-statistics.component.scss']
})
export class DataStatisticsComponent extends AppComponentBase implements OnInit {
    dataList: ShopReportData[] = [];
    loading = false;
    exportLoading = false;
    search: any = { timeType: 1, beginTime: null, endTime: null };
    //search: any = { timeType: 2 }
    timeTypes = [
        { text: '全部', value: 1 },
        { text: '按时间段', value: 2 },
    ]
    form: FormGroup;

    // sortMap = {
    //     registeredShop: null,
    //     scanCode: null,
    //     scanNum: null,
    //     scanCount: null,
    //     consumerIntegral: null,
    //     shopIntegral: null
    // };
    // sortregisteredShopTotal = null;
    // sortscanCodeTotal = null;
    // sortscanNumTotal = null;
    // sortscanCountTotal = null;
    // sortconsumerIntegralTotal = null;
    // sortshopIntegralTotal = null;

    constructor(injector: Injector, private fb: FormBuilder, private router: Router, private modal: NzModalService,
        private shopReportDataService: ShopReportDataServiceProxy) {
        super(injector);
        // this.search.endTime = '2018-07-23';
        // this.search.beginTime = '2018-07-01';
        var currentdate = new Date();
        var y = currentdate.getFullYear();
        var m = currentdate.getMonth() + 1;
        var d = currentdate.getDate();
        var endTime = y + '-' + (m > 9 ? m : '0' + m) + '-' + (d > 9 ? d : '0' + d);
        var beginTime = y + '-' + (m > 9 ? m : '0' + m) + '-' + '01';
        this.search.beginTime = moment(beginTime);
        this.search.endTime = moment(endTime);
    }
    ngOnInit(): void {
        this.form = this.fb.group({
            timeType: [null, [Validators.required]],
            endTime: [null, Validators.compose([Validators.required])],
            beginTime: [null, Validators.compose([Validators.required])],
        });
        this.refreshData();
    }

    getFormControl(id: string) {
        return this.form.controls[id];
    }
    // sort(value, para: string) {
    //     if (para == 'registeredShop') {
    //         this.sortregisteredShopTotal = value;
    //         this.sortscanCodeTotal = null;
    //         this.sortscanNumTotal = null;
    //         this.sortscanCountTotal = null;
    //         this.sortconsumerIntegralTotal = null;
    //         this.sortshopIntegralTotal = null;

    //         this.sortMap.scanCode = null;
    //         this.sortMap.scanNum = null;
    //         this.sortMap.scanCount = null;
    //         this.sortMap.consumerIntegral = null;
    //         this.sortMap.shopIntegral = null;
    //         this.refreshData();
    //     } else if ((para == 'scanCode')) {
    //         this.sortscanCodeTotal = value;
    //         this.sortregisteredShopTotal = null;
    //         this.sortscanNumTotal = null;
    //         this.sortscanCountTotal = null;
    //         this.sortconsumerIntegralTotal = null;
    //         this.sortshopIntegralTotal = null;

    //         this.sortMap.registeredShop = null;
    //         this.sortMap.scanNum = null;
    //         this.sortMap.scanCount = null;
    //         this.sortMap.consumerIntegral = null;
    //         this.sortMap.shopIntegral = null;
    //         this.refreshData();
    //     } else if ((para == 'scanNum')) {
    //         this.sortscanNumTotal = value;
    //         this.sortscanCodeTotal = null;
    //         this.sortregisteredShopTotal = null;
    //         this.sortscanCountTotal = null;
    //         this.sortconsumerIntegralTotal = null;
    //         this.sortshopIntegralTotal = null;

    //         this.sortMap.registeredShop = null;
    //         this.sortMap.scanCode = null;
    //         this.sortMap.scanCount = null;
    //         this.sortMap.consumerIntegral = null;
    //         this.sortMap.shopIntegral = null;
    //         this.refreshData();
    //     } else if ((para == 'scanCount')) {
    //         this.sortscanCountTotal = value;
    //         this.sortscanNumTotal = null;
    //         this.sortscanCodeTotal = null;
    //         this.sortregisteredShopTotal = null;
    //         this.sortconsumerIntegralTotal = null;
    //         this.sortshopIntegralTotal = null;

    //         this.sortMap.registeredShop = null;
    //         this.sortMap.scanCode = null;
    //         this.sortMap.scanNum = null;
    //         this.sortMap.consumerIntegral = null;
    //         this.sortMap.shopIntegral = null;
    //         this.refreshData();
    //     } else if ((para == 'consumerIntegral')) {
    //         this.sortconsumerIntegralTotal = value;
    //         this.sortscanCountTotal = null;
    //         this.sortscanNumTotal = null;
    //         this.sortscanCodeTotal = null;
    //         this.sortregisteredShopTotal = null;
    //         this.sortshopIntegralTotal = null;

    //         this.sortMap.registeredShop = null;
    //         this.sortMap.scanCode = null;
    //         this.sortMap.scanNum = null;
    //         this.sortMap.scanCount = null;
    //         this.sortMap.shopIntegral = null;
    //         this.refreshData();
    //     } else { // shopIntegral
    //         this.sortshopIntegralTotal = value;
    //         this.sortscanCountTotal = null;
    //         this.sortscanNumTotal = null;
    //         this.sortscanCodeTotal = null;
    //         this.sortregisteredShopTotal = null;
    //         this.sortconsumerIntegralTotal = null;

    //         this.sortMap.registeredShop = null;
    //         this.sortMap.scanCode = null;
    //         this.sortMap.scanNum = null;
    //         this.sortMap.scanCount = null;
    //         this.sortMap.consumerIntegral = null;
    //         this.refreshData();
    //     }
    // }

    cleanTime() {
        var currentdate = new Date();
        var y = currentdate.getFullYear();
        var m = currentdate.getMonth() + 1;
        var d = currentdate.getDate();
        var endTime = y + '-' + (m > 9 ? m : '0' + m) + '-' + (d > 9 ? d : '0' + d);
        var beginTime = y + '-' + (m > 9 ? m : '0' + m) + '-' + '01';
        this.search.beginTime = moment(beginTime);
        this.search.endTime = moment(endTime);
        // console.log(this.search);

        // this.search = { beginTime: null, endTime: null, timeType: this.search.timeType };
    }

    refreshData(reset = false, search?: boolean) {
        if (this.search.timeType == 1) { //timeType==1不验证input
            this.loading = true;
            this.shopReportDataService.getDataStatisticsAsync(this.getParameter()).subscribe((result: PagedResultDtoOfShopReportData) => {
                this.loading = false;
                this.dataList = result.items;
                this.query.total = result.totalCount;
            })
        }// timeType==2时验证input
        else {
            for (const i in this.form.controls) {
                this.form.controls[i].markAsDirty();
            } if (this.form.valid) {
                if (reset) {
                    // this.query.pageIndex = 1;
                    // this.search = { beginTime: null, endTime: null, timeType: 1 };
                    // this.sortregisteredShopTotal = null;
                    // this.sortscanCodeTotal = null;
                    // this.sortscanNumTotal = null;
                    // this.sortscanCountTotal = null;
                    // this.sortconsumerIntegralTotal = null;
                    // this.sortshopIntegralTotal = null;
                    // this.sortMap = {
                    //     registeredShop: null,
                    //     scanCode: null,
                    //     scanNum: null,
                    //     scanCount: null,
                    //     consumerIntegral: null,
                    //     shopIntegral: null
                    // };
                }
                if (search) {
                    this.search.beginTime = this.dateFormat(this.search.beginTime);
                    this.search.endTime = this.dateFormat(this.search.endTime);
                }
                this.loading = true;
                this.shopReportDataService.getDataStatisticsAsync(this.getParameter()).subscribe((result: PagedResultDtoOfShopReportData) => {
                    this.loading = false;
                    this.dataList = result.items;
                    this.query.total = result.totalCount;
                })
            }
        }
    }

    getParameter(): Parameter[] {
        var arry = [];
        arry.push(Parameter.fromJS({ key: 'timeType', value: this.search.timeType }));
        arry.push(Parameter.fromJS({ key: 'beginTime', value: this.dateFormat(this.search.beginTime) }));
        arry.push(Parameter.fromJS({ key: 'endTime', value: this.dateFormat(this.search.endTime) }));
        // arry.push(Parameter.fromJS({ key: 'OrganizationName', value: this.search.OrganizationName }));
        // arry.push(Parameter.fromJS({ key: 'sortregisteredShopTotal', value: this.sortregisteredShopTotal }));
        // arry.push(Parameter.fromJS({ key: 'sortscanCodeTotal', value: this.sortscanCodeTotal }));
        // arry.push(Parameter.fromJS({ key: 'sortscanNumTotal', value: this.sortscanNumTotal }));
        // arry.push(Parameter.fromJS({ key: 'sortscanCountTotal', value: this.sortscanCountTotal }));
        // arry.push(Parameter.fromJS({ key: 'sortconsumerIntegralTotal', value: this.sortconsumerIntegralTotal }));
        // arry.push(Parameter.fromJS({ key: 'sortshopIntegralTotal', value: this.sortshopIntegralTotal }));
        return arry;
    }

    goDataStatisticsDetail(groupNum: number, organization: string) {
        this.router.navigate(['admin/marketting/data-statistics-detail', groupNum, organization]);
    }

    exportExcel() {
        if (this.search.timeType == 1) {
            this.exportLoading = true;
            this.shopReportDataService.exportShopReportDataDetailExcel(
                { timeType: this.search.timeType, beginTime: this.search.beginTime, endTime: this.search.endTime }).subscribe(result => {
                    if (result.code == 0) {
                        var url = AppConsts.remoteServiceBaseUrl + result.data;
                        document.getElementById('aShopReportDataExcelUrl').setAttribute('href', url);
                        document.getElementById('btnShopReportDataHref').click();
                    } else {
                        this.notify.error(result.msg);
                    }
                    this.exportLoading = false;
                });
        }
        else {
            for (const i in this.form.controls) {
                this.form.controls[i].markAsDirty();
            } if (this.form.valid) {
                this.exportLoading = true;
                this.shopReportDataService.exportShopReportDataDetailExcel(
                    { timeType: this.search.timeType, beginTime: this.search.beginTime, endTime: this.search.endTime }).subscribe(result => {
                        if (result.code == 0) {
                            var url = AppConsts.remoteServiceBaseUrl + result.data;
                            document.getElementById('aShopReportDataExcelUrl').setAttribute('href', url);
                            document.getElementById('btnShopReportDataHref').click();
                        } else {
                            this.notify.error(result.msg);
                        }
                        this.exportLoading = false;
                    });
            }
        }
    }
}
