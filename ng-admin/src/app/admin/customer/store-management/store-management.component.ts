import { Component, OnInit, Injector } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { ShopServiceProxy, PagedResultDtoOfShop } from '@shared/service-proxies/customer-service';
import { Parameter } from '@shared/service-proxies/entity';
import { Shop } from '@shared/entity/customer';
import { Router } from '@angular/router';
import { AppConsts } from '@shared/AppConsts';

@Component({
    moduleId: module.id,
    selector: 'store-management',
    templateUrl: 'store-management.component.html',
})
export class StoreManagementComponent extends AppComponentBase implements OnInit {
    shops: Shop[] = [];
    zipNameIds: String = null;
    zipUrlIds: string = null;
    willDownShopInfo: Shop[];
    search: any = { status: 4 };
    statusList = [
        { text: '已拒绝', value: 0, type: 'error' },
        { text: '待审核', value: 1, type: 'processing' },
        { text: '已审核', value: 2, type: 'success' },
        // { text: '已关闭店铺', value: 3, type: 'error' },
    ];
    statues = [
        { text: '全部', value: 4 },
        { text: '已拒绝', value: 0 },
        { text: '待审核', value: 1 },
        { text: '已审核', value: 2 },
        // { text: '已关闭店铺', value: 3 },
    ];
    isSelectedAll: boolean = false; // 是否全选
    checkboxCount: number = 0; // 所有Checkbox数量
    checkedLength: number = 0; // 已选中的数量

    sortMap = {
        sale: null,
        read: null,
        single: null,
        fans: null,
    };
    host = AppConsts.remoteServiceBaseUrl;
    loading = false;
    exportLoading = false;
    sortSaleTotal = null;
    sortReadTotal = null;
    sortSingleTotal = null;
    sortFansTotal = null;
    constructor(injector: Injector, private shopServie: ShopServiceProxy,
        private router: Router) {
        super(injector);
    }
    ngOnInit(): void {
        this.refreshData();
    }

    sort(value, para: string) {
        if (para == 'sale') {
            this.sortSaleTotal = value;
            this.sortReadTotal = null;
            this.sortSingleTotal = null;
            this.sortFansTotal = null;
            this.sortMap.read = null;
            this.sortMap.single = null;
            this.sortMap.fans = null;
            this.refreshData();
        } else if (para == 'single') {
            this.sortSingleTotal = value;
            this.sortReadTotal = null;
            this.sortSaleTotal = null;
            this.sortFansTotal = null;
            this.sortMap.read = null;
            this.sortMap.sale = null;
            this.sortMap.fans = null;
            this.refreshData();
        }
        else if (para == 'read') {
            this.sortReadTotal = value;
            this.sortSaleTotal = null;
            this.sortSingleTotal = null;
            this.sortFansTotal = null;
            this.sortMap.sale = null;
            this.sortMap.single = null;
            this.sortMap.fans = null;
            this.refreshData();
        } else {
            this.sortFansTotal = value;
            this.sortReadTotal = null;
            this.sortSaleTotal = null;
            this.sortSingleTotal = null;
            this.sortMap.sale = null;
            this.sortMap.single = null;
            this.sortMap.read = null;
            this.refreshData();
        }
    }

    refreshData(reset = false, search?: boolean) {
        this.isSelectedAll = false;
        if (reset) {
            this.query.pageIndex = 1;
            this.search = { status: 4 };
            this.sortSaleTotal = null;
            this.sortReadTotal = null;
            this.sortSingleTotal = null;
            this.sortFansTotal = null;
            this.sortMap = {
                sale: null,
                read: null,
                single: null,
                fans: null
            };
        }
        if (search) {
            this.query.pageIndex = 1;
        }
        this.loading = true;
        this.shopServie.getAll(this.query.skipCount(), this.query.pageSize, this.getParameter()).subscribe((result: PagedResultDtoOfShop) => {
            this.loading = false;
            this.shops = result.items.map(i => {
                const statusItem = this.statusList[i.status];
                i.statusType = statusItem.type;
                return i;
            });
            this.query.total = result.totalCount;
        })
    }

    getParameter(): Parameter[] {
        var arry = [];
        arry.push(Parameter.fromJS({ key: 'Name', value: this.search.name }));
        arry.push(Parameter.fromJS({ key: 'Tel', value: this.search.tel }));
        arry.push(Parameter.fromJS({ key: 'Status', value: this.search.status === 4 ? null : this.search.status }));
        arry.push(Parameter.fromJS({ key: 'sortSaleTotal', value: this.sortSaleTotal }));
        arry.push(Parameter.fromJS({ key: 'sortReadTotal', value: this.sortReadTotal }));
        arry.push(Parameter.fromJS({ key: 'sortSingleTotal', value: this.sortSingleTotal }));
        arry.push(Parameter.fromJS({ key: 'sortFansTotal', value: this.sortFansTotal }));
        arry.push(Parameter.fromJS({ key: 'RetailCode', value: this.search.code }));
        return arry;
    }
    editShop(shop: Shop) {
        this.router.navigate(['admin/customer/store-detail', shop.id]);
    }

    /**
     * 导出店铺信息
     */
    exportExcel() {
        this.exportLoading = true;
        this.shopServie.ExportExcel({ name: this.search.name, tel: this.search.tel, status: this.search.status === 4 ? null : this.search.status, sortSaleTotal: this.sortSaleTotal, sortReadTotal: this.sortReadTotal, sortSingleTotal: this.sortSingleTotal, sortFansTotal: this.sortFansTotal }).subscribe(data => {
            if (data.code == 0) {
                var url = AppConsts.remoteServiceBaseUrl + data.data;
                document.getElementById('aShopExcelUrl').setAttribute('href', url);
                document.getElementById('btnShopHref').click();
            } else {
                this.notify.error(data.msg);
            }
            this.exportLoading = false;
        });
    }

    downPromotionCodeZip() {
        this.zipNameIds = '';
        this.zipUrlIds = '';
        this.willDownShopInfo = this.shops.filter(v => v.selected && v.status == 2);
        var name = this.willDownShopInfo.forEach(v => {
            if (v.qrUrl) {
                this.zipNameIds += v.retailerCode + v.retailerName + ',';
            } else {
                v.qrUrl = null;
            }
        });
        var url = this.willDownShopInfo.forEach(v => {
            if (v.qrUrl) {
                this.zipUrlIds += v.qrUrl + ',';
            } else {
                v.qrUrl = null;
            }
        });
        if (this.zipNameIds != '' && this.zipUrlIds != '') {
            this.exportLoading = true;
            this.shopServie.PromotionCodeZip({ url: this.zipUrlIds, fileName: this.zipNameIds }).subscribe(data => {
                if (data.code == 0) {
                    var url = AppConsts.remoteServiceBaseUrl + data.data;
                    document.getElementById('aShopPicZipUrl').setAttribute('href', url);
                    document.getElementById('btnShopPicZipHref').click();
                } else {
                    this.notify.error(data.msg);
                }
                this.exportLoading = false;
            });
        }
        else if (this.checkedLength == 0) {
            this.notify.warn(this.l('请选择需要下载推广码的店铺！'));
        } else {
            this.notify.warn(this.l('当前店铺没有推广码！'));
        }
    }

    checkAll(e) {
        var v = this.isSelectedAll;
        this.shops.filter(v => v.status == 2).forEach(u => {
            u.selected = v;
        });
        if (this.isSelectedAll == false) {
            this.checkedLength == 0;
        } else {
            this.checkedLength == this.shops.filter(v => v.selected && v.status == 2).length;
        }
    }

    isCancelCheck(x: any) {
        this.checkedLength = this.shops.filter(v => v.selected && v.status == 2).length;
        console.log(this.checkedLength);
        this.checkboxCount = this.shops.filter(v => v.status == 2).length;
        if (this.checkboxCount - this.checkedLength > 0) {
            this.isSelectedAll = false;
        } else {
            this.isSelectedAll = true;
        }
    }
}
