import { Component, ViewEncapsulation, Injector, OnInit } from '@angular/core';
import { AppComponentBase } from '../../components/app-component-base';
import { Observable } from 'rxjs';
import { ShopService, AppConsts } from '../../../services';
import { JWeiXinService } from 'ngx-weui/jweixin';
import { ShopProduct, NearbyShop } from '../../../services/model';
import { Router } from '@angular/router';

@Component({
    selector: 'wechat-goods',
    templateUrl: './goods.component.html',
    styleUrls: ['./goods.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class GoodsComponent extends AppComponentBase implements OnInit {
    value: string;
    goodes: Observable<ShopProduct[]>;
    shops: NearbyShop[];
    sgoods: ShopProduct;
    lineDesc: string = '搜索并选择特色商品';
    hostUrl: string = AppConsts.remoteServiceBaseUrl;
    latitude: number;//当前纬度
    longitude: number;//当前经度
    productConfig: string = '';
    productList: string[];
    constructor(injector: Injector,
        private shopService: ShopService,
        private wxService: JWeiXinService,
        private router: Router) {
        super(injector);
    }

    ngOnInit() {
        this.wxService.get().then(res => {
            if (!res) {
                console.warn('weixin或qq map js加载失败');
                return;
            }
            let url = this.CurrentUrl;//encodeURIComponent(location.href.split('#')[0]);
            this.settingsService.getJsApiConfig(url).subscribe(result => {
                if (result) {
                    result.jsApiList = ['getLocation'];//指定调用的接口名
                    wx.config(result.toJSON());
                    // 2、通过ready接口处理成功验证
                    this.wxReady().then((res) => {
                        if (res) {
                            this.wxService.getLocation().then((res) => {
                                this.latitude = res.latitude;
                                this.longitude = res.longitude;
                            });
                        }
                    });
                    // 2、通过error接口处理失败验证
                    wx.error(() => {

                    });
                }
            });
        });
        this.getPreProduct();
    }

    wxReady(): Promise<boolean> {
        return (new Promise<any>((resolve, reject) => {
            wx.ready(() => {
                resolve(true);
            });
        }));
    }

    onSearch(term: string) {
        this.value = term;
        if (term) {
            this.goodes = this.shopService.GetRareProductByKeyAsync({ tenantId: this.settingsService.tenantId, key: term });
        }
    }

    onCancel() {
        // this.onClear();
        console.log('取消');
    }

    onClear() {
        console.log('onCancel');
    }

    onSelectGoods(gds: ShopProduct) {
        this.value = '';
        this.sgoods = gds;
        this.shopService.GetShopListByGoodsIdAsync({ tenantId: this.settingsService.tenantId, goodsId: gds.id, latitude: this.latitude, longitude: this.longitude }).subscribe(res => {
            this.shops = res;
            if (!this.shops || this.shops.length == 0) {
                this.lineDesc = '附近3公里没有售卖【' + this.sgoods.specification + '】的店铺';
            }
        });
    }

    goShop(id) {
        this.router.navigate(['/shops/shop', { shopId: id }]);
    }

    getPreProduct() {
        let params: any = {};
        params.tenantId = this.settingsService.tenantId;
        this.shopService.GetPreProductConfig(params).subscribe(res => {
            this.productConfig = res;
            if (this.productConfig != null && this.productConfig.length != 0) {
                this.productList = res.split(',');
            }
        });
    }

    preText(text: string) {
        this.onCancel();
        this.value = text;
        this.onSearch(text)
    }
} 
