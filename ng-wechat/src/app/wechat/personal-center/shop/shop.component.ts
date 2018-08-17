import { Component, ViewEncapsulation, Injector, OnInit, ViewChild } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/timer';
import { AppComponentBase } from '../../components/app-component-base';
import { WechatUser, UserType, Shop, ShopProduct } from '../../../services/model';
import { Router, Params } from '@angular/router';
import { ShopService, AppConsts, FavoriteService } from '../../../services';

import { PopupComponent } from "ngx-weui/popup";
import { ToptipsService } from "ngx-weui/toptips";
import { JWeiXinService } from 'ngx-weui/jweixin';

import { DialogService, DialogConfig, DialogComponent } from 'ngx-weui/dialog';
import 'jsbarcode';
import { LoaderService } from 'ngx-weui/utils/loader.service';
import { ShopQrcodeComponent } from './shop-qrcode/shop-qrcode.component';
import { ToastService, ToastComponent } from 'ngx-weui/toast';
import { ToastModule } from 'ngx-weui';

@Component({
    selector: 'wechat-shop',
    templateUrl: './shop.component.html',
    styleUrls: ['./shop.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class ShopComponent extends AppComponentBase implements OnInit {

    @ViewChild('auto') autoAS: DialogComponent;
    @ViewChild('qrcode') qrcodeAS: ShopQrcodeComponent;
    @ViewChild('success') successToast: ToastComponent;
    @ViewChild('loading') loadingToast: ToastComponent;
    user: WechatUser;
    shop: Shop;
    shopId: string;
    x: boolean;
    shopProducts: ShopProduct[];
    shopProductIds: string[];
    @ViewChild('product') productPopup: PopupComponent;
    cigaretteProducts: ShopProduct[];//卷烟类
    specialProducts: ShopProduct[];//特产类
    isView: boolean = false;
    hostUrl: string = AppConsts.remoteServiceBaseUrl;
    isAudit: boolean = false;
    qrCodeUrl = '';
    public DEFCONFIG: DialogConfig = <DialogConfig>{
        // title: '弹窗标题',
        // content: '弹窗内容，告知当前状态、信息和解决方法，描述文字尽量控制在三行内',
        skin: 'ios',
        backdrop: true,
        cancel: null,
        confirm: null,
    };
    config2: DialogConfig = {};
    //content = '';
    isCancel: boolean = false;

    constructor(injector: Injector,
        private router: Router,
        private shopService: ShopService,
        private wxService: JWeiXinService,
        private srv: ToptipsService,
        private ds: DialogService,
        private load: LoaderService,
        private favoriteService: FavoriteService,
        private srvt: ToastService
    ) {
        super(injector);
        this.activatedRoute.params.subscribe((params: Params) => {
            this.shopId = params['shopId'];
            this.isAudit = params['isAudit'];
        });
    }

    ngOnInit() {

        //this.load.loadScript('assets/libs/qrcode.min.js').then((res) => {
        //this.generateQRcode('wechat_qrcode', '11112');
        //});
        //微信JS SDK配置
        this.wxService.get().then((res) => {
            if (!res) {
                console.warn('jweixin.js 加载失败');
                return;
            }
            let url = this.CurrentUrl;//encodeURIComponent(location.href.split('#')[0]);
            this.settingsService.getJsApiConfig(url).subscribe(result => {
                if (result) {
                    result.jsApiList = ['openLocation'];//指定调用的接口名
                    // 1、通过config接口注入权限验证配置
                    wx.config(result.toJSON());
                    // 2、通过ready接口处理成功验证
                    wx.ready(() => {
                        // 注册各种onMenuShareTimeline & onMenuShareAppMessage
                    });
                    // 2、通过error接口处理失败验证
                    wx.error(() => {

                    });
                }
            });
        });

        // if (this.shopId) {
        //     this.isView = true;
        //     this.shopService.GetViewShopByIdAsync({ id: this.shopId, tenantId: this.settingsService.tenantId }).subscribe(res => {
        //         this.shop = res;
        //     });
        // }
        if (this.shopId) {
            this.isView = true;
            this.shopService.AddSingleTotalAsync({ articleId: this.shopId, openId: this.settingsService.openId, type: 3, tenantId: this.settingsService.tenantId }).subscribe(res => {
                this.shop = res;
            });
            // this.shopService.GetQrCodeUrl(this.shopId).subscribe(data => {
            //     this.qrCodeUrl = data;
            //     console.log("data")
            //     console.log(data)
            //     //生成微信二维码
            //     //this.generateQRcode('wechat_qrcode', data);
            // });
        }
        else {
            this.settingsService.getUser().subscribe(result => {
                this.user = result;
                this.x = this.user.isShopkeeper;
                if (this.user) {
                    //console.table(this.user);
                    if (this.user.userType != UserType.Retailer) { //不是零售客户需先绑定
                        this.router.navigate(['/personals/bind-retailer']);
                    } else {
                        if (!this.user.isShopkeeper && this.user.status == 0) {//不是店主 且 未审核
                            this.router.navigate(['/shops/wait-audit']);
                        } else {
                            this.shopService.GetShopByOpenId(this.WUserParams)
                                .subscribe(result => {
                                    this.shop = result;
                                    if (!this.shop) {//如果没有店铺 需要新增 
                                        this.router.navigate(['/shopadds/shop-add']);
                                    }
                                    this.shopId = this.shop.id;
                                    //  else {
                                    //     this.shopService.GetQrCodeUrl(this.shopId).subscribe(data => {
                                    //         this.qrCodeUrl = data;
                                    //         //生成微信二维码
                                    //         //this.generateQRcode('wechat_qrcode', data);
                                    //     });
                                    // }
                                });
                        }
                    }
                }
            });
        }
        this.IsCancelShop();
    }

    goEditShop() {
        this.router.navigate(['/shopadds/shop-add', { id: '1' }]);
    }

    onSelectProducts() {
        if (!this.shopProducts) {
            let params: any = { shopId: this.shop.id };
            if (this.settingsService.tenantId) {
                params.tenantId = this.settingsService.tenantId;
            }

            this.shopService.GetShopProductsByShopId(params).subscribe(result => {
                this.shopProducts = result;
                console.log(this.shopProducts);

                this.shopProductIds = this.shopProducts.map(s => { return s.id });
            });

            let params2: any = {};
            if (this.settingsService.tenantId) {
                params2.tenantId = this.settingsService.tenantId;
            }

            this.shopService.GetRareProduct(params2).subscribe(data => {
                this.cigaretteProducts = data.cigaretteProducts;
                this.specialProducts = data.specialProducts;
            });
        }
    }

    onProductPopup() {
        this.productPopup.show();
    }

    save() {
        console.table(this.shopProductIds);
        if (this.shopProductIds.length <= 0) {
            this.srv['warn']('请选择特色产品');
        } else {
            let params: any = { shopId: this.shop.id, productIds: this.shopProductIds };
            if (this.settingsService.tenantId) {
                params.tenantId = this.settingsService.tenantId;
            }
            this.shopService.SaveShopProducts(params).subscribe(result => {
                if (result && result.code == 0) {
                    this.shopProducts = ShopProduct.fromJSArray(result.data);
                    this.srv['success']('保存成功');
                    this.productPopup.close();
                } else {
                    this.srv['warn']('保存异常');
                }
            });
        }
    }

    //打开微信地图
    wxOpenLocation() {
        if (!this.shop.latitude || !this.shop.longitude) {
            this.srv['info']('当前店铺没有位置信息');
            return;
        }
        wx.openLocation({
            latitude: this.shop.qqLatitude, // 纬度，浮点数，范围为90 ~ -90
            longitude: this.shop.qqLongitude, // 经度，浮点数，范围为180 ~ -180。
            name: this.shop.name, // 位置名
            address: this.shop.address, // 地址详情说明
            scale: 16, // 地图缩放级别,整形值,范围从1~28。默认为最大 之前12
            infoUrl: AppConsts.remoteServiceBaseUrl + '/gawechat/index.html#/shops/shop' // 在查看位置界面底部显示的超链接,可点击跳转
        });
    }

    config = <DialogConfig>{
        title: '拒绝确认',
        content: '请填写拒绝理由，注意简洁明了',
        inputPlaceholder: '拒绝理由',
        inputError: '必填',
        inputRequired: true,
        skin: 'auto',
        type: 'prompt',
        confirm: '拒绝',
        cancel: '取消',
        input: 'textarea',
        inputValue: undefined,
        inputAttributes: {
            maxlength: 140,
            cn: 1
        },
        inputRegex: null
    }

    reason: string = '';

    onRejectPrompt() {
        this.autoAS.show().subscribe((res: any) => {
            if (res.result) {
                this.reason = JSON.stringify(res.result);
                //alert(this.reason);
                this.audit(0);
            }
            //console.log('prompt from component', res);
        });
    }

    //审核
    audit(status: any) {
        this.shopService.CheckShop({ id: this.shop.id, status: status, reason: this.reason }).subscribe((res) => {
            if (res) {
                this.shop.status = status;
                this.srv['success']('操作成功');
            } else {
                this.srv['warn']('操作异常');
            }
        });
    }

    showQrCode() {


        // this.generateQRcode('wechat_qrcode', this.qrCodeUrl);
        // this.content = '<div class="mdiv"><p>' + this.shop.name + '</p><div><img class="qrcode" src="' + AppConsts.remoteServiceBaseUrl + this.shop.qrUrl + '"></div><p>扫一扫，进入店铺</p></div>';
        // this.config2 = Object.assign({}, this.DEFCONFIG, <DialogConfig>{
        //     content: '<div class="mdiv"><p>' + this.shop.name + '</p><div id="wechat_qrcode" class="payment_wechat_img" ><img class="payment_wechat_icon" src="assets/images/logo.jpg"></div><p>扫一扫，进入店铺</p></div>',
        // });
        // this.ds.show(this.config2).subscribe((res: any) => {
        //     console.log(res);
        //     this.generateQRcode('wechat_qrcode', this.qrCodeUrl);
        // });
        this.qrcodeAS.show(this.shop);
    }

    generateQRcode(id: string, url: any) {
        let qrShopCode = new QRCode(id, {
            text: url,
            width: 230,
            height: 230,
            correctLevel: QRCode.CorrectLevel.H
        });
    }

    IsCancelShop() {
        setTimeout(() => {
            let params: any =
            {
                shopId: this.shopId,
                openId: this.settingsService.openId,
            };
            // this.favoriteService.GetUserIsCancelShopAsycn(params).subscribe(data => setTimeout(() => {
            //     {
            //         this.isCancel = data;
            //     }
            // }, 500));
            this.favoriteService.GetUserIsCancelShopAsycn(params).subscribe(data => {
                this.isCancel = data;
            });
        }, 1);
    }

    favorite(type: string) {
        if (type == 'favorite') {
            if (this.isCancel == true) {
                let params: any =
                {
                    shopId: this.shopId,
                    shopName: this.shop.name,
                    openId: this.settingsService.openId,
                    isCancel: false
                };
                this.favoriteService.AddOrCancelFavoriteShop(params).subscribe(data => {
                    if (data && data.code === 0) {
                        this.srvt['success']('收藏成功', 0);
                        this.isCancel = false;
                    }
                    else {
                        this.srvt['loading']('请重试');
                        this.isCancel = true;
                    }
                });
            }
        } else { //取消收藏
            if (this.isCancel == false) {
                let params: any =
                {
                    shopId: this.shopId,
                    openId: this.settingsService.openId,
                    isCancel: true
                };
                this.favoriteService.AddOrCancelFavoriteShop(params).subscribe(data => {
                    if (data && data.code === 0) {
                        this.srvt['success']('已取消收藏', 0);
                        this.isCancel = true;
                    }
                    else {
                        this.srvt['loading']('请重试');
                        this.isCancel = false;
                    }
                });
            }
        }
    }
}
