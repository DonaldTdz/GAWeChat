import { Component, ViewEncapsulation, Injector, OnInit, ViewChild } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/timer';
import { AppComponentBase } from '../../components/app-component-base';
import { WechatUser, UserType, Shop, ShopProduct } from '../../../services/model';
import { Router, Params } from '@angular/router';
import { ShopService, AppConsts, FavoriteService, WechatUserService } from '../../../services';

import { PopupComponent } from "ngx-weui/popup";
import { ToptipsService } from "ngx-weui/toptips";
import { JWeiXinService } from 'ngx-weui/jweixin';

import { DialogService, DialogConfig, DialogComponent } from 'ngx-weui/dialog';
import 'jsbarcode';
import { LoaderService } from 'ngx-weui/utils/loader.service';
import { ShopQrcodeComponent } from './shop-qrcode/shop-qrcode.component';
import { ToastService, ToastComponent } from 'ngx-weui/toast';
import { ToastModule, SkinType } from 'ngx-weui';

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
        skin: 'ios',
        backdrop: true,
        cancel: null,
        confirm: null,
    };
    config2: DialogConfig = {};
    //content = '';
    isCancel: boolean = false;
    goodsBarCode: string;// 商品码
    shopKeeperCode: string;
    host = AppConsts.remoteServiceBaseUrl;
    goods = [];
    isShowWindows: string = 'false';
    curCount: number = 0;
    limitFrequency: number = 0;
    gpslat: number;//当前纬度gps
    gpslong: number;//当前经度gps
    latitude: number;//当前纬度
    longitude: number;//当前经度
    citylocation: any;

    constructor(injector: Injector,
        private router: Router,
        private shopService: ShopService,
        private wxService: JWeiXinService,
        private srv: ToptipsService,
        private ds: DialogService,
        private load: LoaderService,
        private favoriteService: FavoriteService,
        private srvt: ToastService,
        private dia: DialogService,
        private wechatService: WechatUserService
    ) {
        super(injector);
        this.activatedRoute.params.subscribe((params: Params) => {
            this.shopId = params['shopId'];
            this.isAudit = params['isAudit'];
            this.isShowWindows = params['isShowWindows'];
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
                    result.jsApiList = ['openLocation', 'getLocation', 'scanQRCode'];//指定调用的接口名                   
                    // 1、通过config接口注入权限验证配置
                    wx.config(result.toJSON());
                    // 2、通过ready接口处理成功验证
                    // wx.ready(() => {
                    //     // 注册各种onMenuShareTimeline & onMenuShareAppMessage
                    // });
                    this.wxReady().then((res) => {
                        if (res) {
                            if (this.shopId) {
                                this.wxGetLocation();
                            }
                        }
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
            this.IsCancelShop();
            let params: any =
            {
                shopId: this.shopId,
                openId: this.settingsService.openId,
            };
            this.shopService.getIsCurShopKeeper(params).subscribe(data => {
                if (data == true) {
                    this.isShowWindows = 'false';
                }
            });
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
                                    this.IsCancelShop();
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

        // this.IsCancelShop();
        // setTimeout(() => {
        //     if (this.isShowWindows != 'false') {
        //         this.onShowBySrv('ios', true);
        //         // let data: any = {};
        //         // data.productName = '黄金叶';
        //         // data.price = 20;
        //         // data.userIntegral = 200;
        //         // this.goPurchaserecord('ios', true, data);
        //     }
        // }, 500);

        this.getLimitFrequency();
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
        // setTimeout(() => {
        //     let params: any =
        //     {
        //         shopId: this.shopId,
        //         openId: this.settingsService.openId,
        //     };
        //     // this.favoriteService.GetUserIsCancelShopAsycn(params).subscribe(data => setTimeout(() => {
        //     //     {
        //     //         this.isCancel = data;
        //     //     }
        //     // }, 500));
        //     this.favoriteService.GetUserIsCancelShopAsycn(params).subscribe(data => {
        //         this.isCancel = data;
        //     });
        // }, 1);
        setTimeout(() => {
            let params: any =
            {
                shopId: this.shopId,
                openId: this.settingsService.openId,
            };
            this.favoriteService.GetUserIsCancelShopAsycn(params).subscribe(data => {
                this.isCancel = data;
            });
        }, 500);
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

    //调用微信扫一扫
    wxScanQRCode(): Promise<string> {
        return new Promise<string>((resolve, reject) => {
            wx.scanQRCode({
                needResult: 1, // 默认为0，扫描结果由微信处理，1则直接返回扫描结果，
                //scanType: ["qrCode", "barCode"], // 可以指定扫二维码还是一维码，默认二者都有
                scanType: ['barCode'],
                success: ((res) => {
                    resolve(res.resultStr);
                })
            });
        });
    }

    scanOk(memberCode: string) {
        if (!memberCode) {
            this.srv['warn']('请先绑定手机号');
            this.router.navigate(["/personals/bind-member"]);
        } else {
            if (!this.goods || this.goods.length == 0) {
                this.srv['warn']('没有商品信息');
            } else {
                let param: any = {};
                param.shopProductList = this.goods;
                param.shopId = this.shop.id;
                param.shopName = this.shop.name;
                param.openId = this.settingsService.openId;//消费者id
                param.tenantId = this.settingsService.tenantId;
                param.operatorOpenId = this.settingsService.openId;//消费者自己操作
                param.operatorName = this.user.nickName;
                param.retailerId = this.shop.retailerId;
                param.host = this.host;
                this.shopService.ExchangeIntegral(param).subscribe(res => {
                    if (res && res.code == 0) {
                        this.curCount++;
                        this.goPurchaserecord('ios', true, res.data);
                    } else {
                        this.curCount--;
                        this.srv['warn']('兑换失败，请重试');
                    }
                });
                //扫码后清空列表
                this.goods = [];
            }
        }
    }

    /**
     * 商品扫码
     */
    setGoodsBarCode(res: string) {
        let resarry = res.split(',');
        if (resarry.length == 2) {
            if (resarry[0] != 'EAN_13') {
                this.srv['warn']('条码格式不匹配');
                return;
            }
            this.goodsBarCode = resarry[1];
            //获取卷烟数据
            let param: any = {};
            param.code = this.goodsBarCode;
            if (this.settingsService.tenantId) {
                param.tenantId = this.settingsService.tenantId;
            }
            this.shopService.GetShopProductByCode(param).subscribe(result => {
                if (result) {
                    this.goods.push(result);
                    let params: any =
                    {
                        shopId: this.shopId,
                        openId: this.settingsService.openId,
                        productId: result.id
                    };
                    this.shopService.GetPurchaseRecordCountByHour(params).subscribe(res => {
                        this.curCount = res;
                        if (this.curCount >= this.limitFrequency) {
                            this.productInfoDialog('ios', true, result);
                        } else {
                            this.getCustMemberCode();
                        }
                    });
                } else {
                    this.srv['warn']('没找到匹配商品');
                }
            });
        }
    }

    productInfoDialog(type: SkinType, backdrop: boolean = true, data: any) {
        this.DEFCONFIG = {
            confirm: '确认',
            cancel: null,
            btns: null
        };

        let content: string = `<div class="divLimit">
        <div class="divLeft">
        <img src="assets/images/shop/ScanOk.png" alt="">
        </div>
        <div class="divRight">
            <div class="textUp">扫码成功</div>
            <div class="textDown">${data.specification}</div>
            <div class="textDown">建议零售价(包):￥${data.price}</div>
            </div>
        </div>`
        this.config = Object.assign({}, this.DEFCONFIG, <DialogConfig>{
            skin: type,
            backdrop: backdrop,
            content: content
        });

        this.dia.show(this.config).subscribe((res: any) => {
            if (res.value == true) {
            }
        });
        return false;
    }

    onShowBySrv(type: SkinType, backdrop: boolean = true) {
        this.DEFCONFIG = {
            confirm: '扫一扫',
        };
        this.config = Object.assign({}, this.DEFCONFIG, <DialogConfig>{
            skin: type,
            backdrop: backdrop,
            content: '<div class="logoCssDiv"><img class="logoCss" src="assets/images/shop/ScanLogo.png" alt=""> </div>'
        });
        this.dia.show(this.config).subscribe((res: any) => {
            if (res.value == true) {
                this.wxScanQRCode().then((res) => {
                    this.setGoodsBarCode(res);
                });
            }
        });
        return false;
    }

    /**
     *兑换成功弹出框
     */
    goPurchaserecord(type: SkinType, backdrop: boolean = true, data: any) {
        if (this.curCount == this.limitFrequency) {
            this.DEFCONFIG = {
                confirm: '立即评价',
                cancel: '确定'
            };
        } else {
            this.DEFCONFIG = {
                confirm: '立即评价',
                cancel: '扫一扫'
            };
        }

        let content: string = `<div class="divFull">
        <div class="divLeft">
        <img src="assets/images/shop/ScanOk.png" alt="">
        </div>
        <div class="divRight">
            <div class="textUp">兑换成功</div>
            <div class="textDown">${data.productName}</div>
            <div class="textDown">建议零售价(包):￥${data.price}</div>
            <div class="textDown">获得积分:${data.userIntegral}</div>
        </div>
    </div>`
        this.config = Object.assign({}, this.DEFCONFIG, <DialogConfig>{
            skin: type,
            backdrop: backdrop,
            content: content
        });
        this.dia.show(this.config).subscribe((res: any) => {
            if (res.value == true) {
                this.router.navigate(['/purchaserecords/purchaserecord']);
            } else {
                if (this.curCount == this.limitFrequency) {
                } else {
                    this.wxScanQRCode().then((res) => {
                        this.setGoodsBarCode(res);
                    });
                }
            }
        });
        return false;
    }

    /**
     *获取消费者会员卡
     */
    getCustMemberCode() {
        this.wechatService.getCustMemberCode(this.settingsService.openId).subscribe(data => {
            this.user = data;
            this.scanOk(this.user.memberBarCode);
        });
    }

    goGoodDetail(id: string) {
        this.router.navigate(['/shops/shop-good-detail', { id: id }]);
    }

    getLimitFrequency() {
        this.shopService.getLimitFrequency().subscribe(res => {
            this.limitFrequency = res;
        });
    }

    wxReady(): Promise<boolean> {
        return (new Promise<any>((resolve, reject) => {
            wx.ready(() => {
                resolve(true);
            });
        }));
    }

    getWXLocation(): Promise<any> {
        return (new Promise<any>((resolve, reject) => {
            this.wxService.getLocation().then((res) => {
                this.gpslat = res.latitude;
                this.gpslong = res.longitude;
                //this.wxService.translate(res.latitude, res.longitude).then((result) => {
                //    resolve(result);
                //})
                resolve(res);
            });
        }));
    }

    //调用微信获取当前位置
    wxGetLocation() {
        this.getWXLocation().then((res) => {
            if (res) {
                this.latitude = res.latitude; //res[0].lat;
                this.longitude = res.longitude; //res[0].lng;
                //获取地址信息
                this.getShopWithRange();
            }
        });
    }

    getShopWithRange() {
        let param: any = {
            latitude: this.gpslat,
            longitude: this.gpslong,
            shopId: this.shopId,
        };
        this.shopService.GetShopWithRangeAsync(param).subscribe(res => {
            if (res == false) {
                this.isShowWindows = 'false';
            }
            // setTimeout(() => {
            if (this.isShowWindows != 'false') {
                this.onShowBySrv('ios', true);
                // let data: any = {};
                // data.productName = '黄金叶';
                // data.price = 20;
                // data.userIntegral = 200;
                // this.goPurchaserecord('ios', true, data);
            }
            // }, 500);
        });
    }
}
