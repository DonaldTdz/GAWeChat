import { Component, OnInit, Injector, ViewChild } from '@angular/core';
import { Shop, ShopEvaluation, ShopProduct } from '@shared/entity/customer';
import { WechatUser, PurchaseRecord } from '@shared/entity/wechat';
import { AppComponentBase } from '@shared/app-component-base';
import { ShopServiceProxy, ShopEvaluationServiceProxy, PagedResultDtoOfShopEvaluation, ShopProductsServiceProxy, PagedResultDtoOfShopProduct } from '@shared/service-proxies/customer-service';
import { WechatUserServiceProxy, PagedResultDtoOfWeChatUser, PagedResultDtoOfPurchaseRecords, PurchaseRecordServiceProxy } from '@shared/service-proxies/wechat-service';
import { ActivatedRoute, Router } from '@angular/router';
import { Parameter } from '@shared/service-proxies/entity';
import { NzModalService } from 'ng-zorro-antd';
import { AppConsts } from '@shared/AppConsts';
import { RefuseComponent } from './refuse/refuse.component';

@Component({
    moduleId: module.id,
    selector: 'store-detail',
    templateUrl: 'store-detail.component.html',
    styleUrls: ['store-detail.component.scss']
})
export class StoreDetailComponent extends AppComponentBase implements OnInit {
    @ViewChild('refuseModal') refuseModal: RefuseComponent;
    //积分明细
    queryE: any = {
        pageIndex: 1,
        pageSize: 10,
        skipCount: function () { return (this.pageIndex - 1) * this.pageSize; },
        total: 0,
        sorter: '',
        status: -1,
        statusList: []
    };
    //特色商品
    querySP: any = {
        pageIndex: 1,
        pageSize: 5,
        skipCount: function () { return (this.pageIndex - 1) * this.pageSize; },
        total: 0,
    };
    //店铺统计
    queryDS: any = {
        pageIndex: 1,
        pageSize: 5,
        skipCount: function () { return (this.pageIndex - 1) * this.pageSize; },
        total: 0,
    };
    shop: Shop = new Shop();
    shopEmployees: WechatUser[] = [];
    shopEvaluations: ShopEvaluation[] = [];
    shopProducts: ShopProduct[] = [];
    dataStatisticsList: PurchaseRecord[] = [];
    loading = false;
    loadingE = false;
    loadingSP = false;
    loadingDS = false;
    shopName = '';
    id: number;
    evaluation: ShopEvaluation;

    //图片放大
    previewImage = ''
    previewVisible = false;
    imgWidth: number = 550;
    defalutImg = '/upload/product/tobacco.jpg';
    //评价

    low: number = 0;
    middle: number = 0;
    hight: number = 0;
    totalE: number = 0;
    evaluations = [
        { id: 0, text: '全部', value: true, color: 'blue' },
        { id: 5, text: '好评', value: false, color: 'red' },
        { id: 3, text: '中评', value: false, color: 'orange' },
        { id: 1, text: '差评', value: false, color: '#AAAAAA' },
    ];
    evaluationSearch: number = 0;
    evaluationShow: string;
    host = '';
    showCoverPhoto = '';
    auteloading = false;

    background = 'white';
    backgroundAlpha = 1.0;
    foreground = 'black';
    foregroundAlpha = 1.0;
    level = 'L';
    mime = 'image/png';
    padding = 10;
    size = 220;

    sortMap = {
        quantity: null,
        price: null,
        integral: null,
    };
    sortQuantityTotal = null;
    sortPriceTotal = null;
    sortIntegralTotal = null;
    exportLoading = false;
    nickName: string = '';
    constructor(injector: Injector, private shopService: ShopServiceProxy, private shopEvaluationService: ShopEvaluationServiceProxy,
        private weChatService: WechatUserServiceProxy, private Acroute: ActivatedRoute, private modal: NzModalService,
        private shopProductService: ShopProductsServiceProxy,
        private purchaseRecordService: PurchaseRecordServiceProxy,
        private router: Router) {
        super(injector);
        this.id = this.Acroute.snapshot.params['id'];
    }
    ngOnInit(): void {
        this.getSingleShop();
        this.host = AppConsts.remoteServiceBaseUrl;
        this.defalutImg = this.host + this.defalutImg;
    }

    getSingleShop() {
        this.shopService.get(this.id).subscribe((result: Shop) => {
            this.shop = result;
            if (this.shop.coverPhoto) {
                // this.showCoverPhoto = this.host + this.shop.coverPhoto;
                this.showCoverPhoto = this.shop.coverPhoto;
            }
            this.evaluations = [
                { id: 0, text: '全部', value: true, color: 'blue' },
                { id: 5, text: '好评', value: false, color: 'red' },
                { id: 3, text: '中评', value: false, color: 'orange' },
                { id: 1, text: '差评', value: false, color: '#AAAAAA' },
            ];
            if (result.evaluation) {
                var assess = result.evaluation.split(',');
                this.hight = parseInt(assess[0]);
                this.middle = parseInt(assess[1]);
                this.low = parseInt(assess[2]);
                this.evaluationShow = '好评：' + this.hight + ' 中评：' + this.middle + ' 差评：' + this.low;
                this.totalE = this.low + this.middle + this.hight;
                this.evaluations.map(i => {
                    if (i.id == 0) {
                        i.text = i.text + '(' + this.totalE + ')';
                    } else if (i.id == 5) {
                        i.text = i.text + '(' + this.hight + ')';
                    } else if (i.id == 3) {
                        i.text = i.text + '(' + this.middle + ')';
                    } else if (i.id == 1) {
                        i.text = i.text + '(' + this.low + ')';
                    }
                })
            }
            this.shopName = this.shop.name + '(' + this.shop.statusName + ')';
            this.refreshData();
            this.refreshDataE();
            this.getShopProducts();
            this.refreshDataDS();
        });
    }
    refreshData() {
        this.loading = true;
        this.weChatService.getShopWeChatUserAll(this.query.skipCount(), this.query.pageSize, this.getParameter()).subscribe((result: PagedResultDtoOfWeChatUser) => {
            this.loading = false;
            this.shopEmployees = result.items;
            this.query.total = result.totalCount;
        });
    }
    getParameter(): Parameter[] {
        var arry = [];
        arry.push(Parameter.fromJS({ key: 'ShopOwnerId', value: this.shop.retailerId }));
        return arry;
    }
    refreshDataE() {
        this.loadingE = true;
        this.shopEvaluationService.getAll(this.queryE.skipCount(), this.queryE.pageSize, this.getParameterEvaluation()).subscribe((result: PagedResultDtoOfShopEvaluation) => {
            this.loadingE = false;
            this.shopEvaluations = result.items;
            this.queryE.total = result.totalCount;
        });
    }
    getParameterEvaluation(): Parameter[] {
        var arry = [];
        arry.push(Parameter.fromJS({ key: 'ShopId', value: this.shop.id }));
        arry.push(Parameter.fromJS({ key: 'Evaluation', value: this.evaluationSearch == 0 ? null : this.evaluationSearch }));
        return arry;
    }

    /**
     * 用于显示评价详情
     * @param evaluation 
     */
    editShopEvalustion(evaluation: ShopEvaluation) {
        this.evaluation = evaluation;
    }

    changeCategory(status: boolean, idx: number, id: number) {
        // if (id === 0) {
        //     this.evaluations.map(i => i.value = status);
        // } else {
        //     this.evaluations[idx].value = status;
        // }
        this.evaluations.map(i => i.value = !status);
        this.evaluations[idx].value = status;
        // this.evaluations[idx].color = 'green';
        this.evaluationSearch = id;
        this.refreshDataE();
    }

    //审核通过
    check(status: number, contentTpl) {
        this.modal.confirm({
            content: contentTpl,
            okText: '是',
            cancelText: '否',
            onOk: () => {
                this.auteloading = true;
                this.shop.status = status;
                // this.shop.auditTime = this.dateFormat(new Date);
                this.shopService.CheckShop({ id: this.shop.id, status: status }).subscribe((result: Shop) => {
                    this.getSingleShop();
                    this.notify.info(this.l('店铺状态更新成功！'));
                    this.auteloading = false;
                });
            }
        });
    }
    //拒绝
    Refuse() {
        this.refuseModal.show(this.shop.id);
    }

    return() {
        this.router.navigate(['admin/customer/store-management']);
    }
    /**
     * 获取特色商品信息
     */
    getShopProducts() {
        this.loadingSP = true;
        this.shopProductService.getAll(this.querySP.skipCount(), this.querySP.pageSize, this.getParameteShopProduct()).subscribe((result: PagedResultDtoOfShopProduct) => {
            this.loadingSP = false;
            this.shopProducts = result.items.map(i => {
                // if (i.photoUrl) {
                //     i.showPhotoUrl = this.host + i.photoUrl;
                // } else {
                //     i.showPhotoUrl = this.defalutImg;
                // }
                return i;
            });
            this.querySP.total = result.totalCount;

        })
    }
    getParameteShopProduct(): Parameter[] {
        var arry = [];
        arry.push(Parameter.fromJS({ key: 'ShopId', value: this.shop.id }));
        return arry;
    }

    handlePreview = (url: string) => {
        // if (!url) {
        //     this.previewImage = this.defalutImg;
        // }
        // else {
        //     this.previewImage = url;
        // }
        this.previewImage = url;
        this.previewVisible = true;
    }

    refreshDataDS(reset = false) {
        this.loadingDS = true;
        if (reset) {
            this.query.pageIndex = 1;
            this.nickName = '';
            this.sortQuantityTotal = null;
            this.sortPriceTotal = null;
            this.sortIntegralTotal = null;
            this.sortMap = {
                quantity: null,
                price: null,
                integral: null,
            };
        }
        this.purchaseRecordService.getShopDataStatistics(this.queryDS.skipCount(), this.queryDS.pageSize, this.getParameterDS()).subscribe((result: PagedResultDtoOfPurchaseRecords) => {
            this.loadingDS = false;
            this.dataStatisticsList = result.items;
            this.queryDS.total = result.totalCount;
        });
    }

    getParameterDS(): Parameter[] {
        var arry = [];
        arry.push(Parameter.fromJS({ key: 'ShopId', value: this.shop.id }));
        arry.push(Parameter.fromJS({ key: 'Name', value: this.nickName }));
        arry.push(Parameter.fromJS({ key: 'SortQuantityTotal', value: this.sortQuantityTotal }));
        arry.push(Parameter.fromJS({ key: 'SortPriceTotal', value: this.sortPriceTotal }));
        arry.push(Parameter.fromJS({ key: 'SortIntegralTotal', value: this.sortIntegralTotal }));
        return arry;
    }
    sort(value, para: string) {
        if (para == 'quantity') {
            this.sortQuantityTotal = value;
            this.sortPriceTotal = null;
            this.sortIntegralTotal = null;
            this.sortMap.price = null;
            this.sortMap.integral = null;
            this.refreshDataDS();
        } else if (para == 'price') {
            this.sortPriceTotal = value;
            this.sortQuantityTotal = null;
            this.sortIntegralTotal = null;
            this.sortMap.integral = null;
            this.sortMap.quantity = null;
            this.refreshDataDS();
        } else {
            this.sortIntegralTotal = value;
            this.sortQuantityTotal = null;
            this.sortPriceTotal = null;
            this.sortMap.quantity = null;
            this.sortMap.price = null;
            this.refreshDataDS();
        }
    }

    /**
     * 导出店铺信息
     */
    exportExcel() {
        this.exportLoading = true;
        this.purchaseRecordService.ExportShopDataExcel(
            {
                shopId: this.shop.id, name: this.nickName, sortQuantityTotal: this.sortQuantityTotal, sortPriceTotal: this.sortPriceTotal, sortIntegralTotal: this.sortIntegralTotal
            }).subscribe(data => {
                if (data.code == 0) {
                    var url = AppConsts.remoteServiceBaseUrl + data.data;
                    document.getElementById('aShopDataExcelUrl').setAttribute('href', url);
                    document.getElementById('btnShopDataHref').click();
                } else {
                    this.notify.error(data.msg);
                }
                this.exportLoading = false;
            });
    }
}
