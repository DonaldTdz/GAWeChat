import { Component, ViewEncapsulation, Injector, OnInit } from '@angular/core';
import { AppComponentBase } from '../../components/app-component-base';
import { JWeiXinService } from 'ngx-weui/jweixin';
import { NearbyShop } from '../../../services/model';
import { ShopService, AppConsts } from '../../../services';
import { Router } from '@angular/router';

@Component({
    selector: 'wechat-nearby-shop',
    templateUrl: './nearby-shop.component.html',
    styleUrls: ['./nearby-shop.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class NearbyShopComponent extends AppComponentBase implements OnInit {

    myaddress: string = '定位中....';
    citylocation: any;
    options: any = {
        complete: ((res) => {
            //console.log(JSON.stringify(res));
            this.myaddress = res.detail.detail;
        }),
        error: (() => {
            this.myaddress = '定位失败';
        })
    }

    shops: NearbyShop[] = [];
    latitude: number;//当前纬度
    longitude: number;//当前经度
    gpslat: number;//当前纬度gps
    gpslong: number;//当前经度gps
    hostUrl: string = AppConsts.remoteServiceBaseUrl;

    constructor(injector: Injector,
        private wxService: JWeiXinService,
        private router: Router,
        private shopService: ShopService) {
        super(injector);
    }

    ngOnInit() {
        //alert(this.CurrentUrl);
        //调用城市服务信息
        this.citylocation = new qq.maps.CityService(this.options);
        this.wxService.get().then(res => {
            if (!res) {
                console.warn('weixin或qq map js加载失败');
                return;
            }
            let url = this.CurrentUrl; //encodeURIComponent(location.href.split('#')[0]);
            this.settingsService.getJsApiConfig(url).subscribe(result => {
                if (result) {
                    result.jsApiList = ['openLocation', 'getLocation'];//指定调用的接口名
                    //console.log(result.toJSON());
                    // 1、通过config接口注入权限验证配置
                    wx.config(result.toJSON());
                    // 2、通过ready接口处理成功验证
                    //wx.ready(() => {
                    // 注册各种onMenuShareTimeline & onMenuShareAppMessage
                    //    this.wxGetLocation();
                    //});
                    this.wxReady().then((res) => {
                        if (res) {
                            this.wxGetLocation();
                        }
                    });
                    // 2、通过error接口处理失败验证
                    wx.error(() => {

                    });
                }
            });
        });
        /*this.latitude = 39.920522;
        this.longitude = 116.194153;
        qq.maps.convertor.translate(new qq.maps.LatLng(this.latitude, this.longitude), 1,
            function (res) {
                console.log(JSON.stringify(res));
            });*/
        //this.getlocation();
        //this.getShops();
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
        this.myaddress = '定位中....';
        this.getWXLocation().then((res) => {
            //if (res && res.length > 0) {
            if (res) {
                this.latitude = res.latitude; //res[0].lat;
                this.longitude = res.longitude; //res[0].lng;
                //获取地址信息
                this.getlocation();
                this.getShops();
            }
        });
    }

    getlocation() {
        var latLng = new qq.maps.LatLng(this.latitude, this.longitude);
        this.citylocation.searchCityByLatLng(latLng);
    }

    getShops() {
        let param: any = {
            latitude: this.gpslat,
            longitude: this.gpslong,
            openId: this.settingsService.openId,
            tenantId: this.settingsService.tenantId
        };
        this.shopService.GetNearbyShopByLocationAsync(param).subscribe(res => {
            this.shops = res;
        });
    }

    goShop(id) {
        this.router.navigate(['/shops/shop', { shopId: id, isShowWindows: false }]);
    }
} 
