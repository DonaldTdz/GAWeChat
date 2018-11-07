import { Component, ViewEncapsulation, Injector, OnInit, ViewChild } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/timer';
import { AppComponentBase } from '../../../components/app-component-base';
import { WechatUser, UserType, Shop } from '../../../../services/model';
import { Router, Params } from '@angular/router';
import { UploaderOptions, FileItem, Uploader, UploaderHeaders } from 'ngx-weui';
import { ShopService, AppConsts } from '../../../../services';
import { ToptipsService } from "ngx-weui/toptips";
import { JWeiXinService } from 'ngx-weui/jweixin';
import { PopupComponent } from "ngx-weui/popup";
import { ToastService } from "ngx-weui/toast";

import { ImageCropperComponent, CropperSettings, Bounds, CropPosition } from 'ngx-img-cropper';

@Component({
    selector: 'wechat-shop-add',
    templateUrl: './shop-add.component.html',
    styleUrls: ['./shop-add.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class ShopAddComponent extends AppComponentBase implements OnInit {

    //图片裁剪 2018-06-07
    @ViewChild('imgCropperPopup') imgCropperPopup: PopupComponent;
    @ViewChild('shopCropper', undefined) shopCropper: ImageCropperComponent;
    public cropperSettings: CropperSettings;
    public imgData: any;
    fileName: string;

    showAddInfo: boolean = true;
    user: WechatUser;

    res: any = {};
    coverPhoto: string;
    img: any;
    imgShow: boolean = false;
    title: string = '新增店铺';
    hostUrl: string = AppConsts.remoteServiceBaseUrl;
    longitude: number;
    latitude: number;
    qqLongitude: number;
    qqLatitude: number;
    myaddress: string = '';
    locationInfo: string = '获取店铺位置....';
    citylocation: any;
    isReset: boolean = false;//是否是重新定位地址
    options: any = {
        complete: ((res) => {
            //console.log(JSON.stringify(res));
            this.locationInfo = res.detail.detail;
            let resArry = this.locationInfo.split(',');
            let cityName = '';
            if (resArry.length > 0) {
                let i = resArry.length - 2;
                while (i >= 0) {
                    cityName = (cityName + resArry[i]);
                    i--;
                }
                this.myaddress = cityName;
                if (this.isReset == true) {
                    this.res.address = this.myaddress;
                    this.isReset = false;
                }
            }
        }),
        error: (() => {
            this.locationInfo = '定位失败';
            this.myaddress = '';
        })
    }

    /*uploader: Uploader = new Uploader(<UploaderOptions>{
        url: AppConsts.remoteServiceBaseUrl + '/WeChatFile/FilesPosts?folder=shop',
        auto: true,
        limit: 1,
        //size: 153600,
        onUploadStart: ((file: FileItem) => {
            console.table(file._file);
            console.table(file.file);
            if (file.file.size > 153600) {
                this.srv['warn']('文件必须小于等于150KB');
                file.cancel();
            }
        }),
        onUploadSuccess: ((file: FileItem, response: string) => {
            //console.log('onUploadSuccess-' + response);
            let data = JSON.parse(response);
            if (data && data.success == true) {
                this.coverPhoto = data.result;
            }
        }),
        onError: (() => {
            this.srv['warn']('检查文件大小是否超过限制');
        }),
        onUploadComplete: function (file: FileItem, response: string) {
            //console.log('onUploadComplete-' + response, arguments);
        }
    });*/

    constructor(injector: Injector, private router: Router,
        private shopService: ShopService,
        private wxService: JWeiXinService,
        private srv: ToptipsService,
        private srvToast: ToastService) {
        super(injector);
        this.cropperSettings = new CropperSettings();
        let winWidth = window.document.body.clientWidth;
        this.cropperSettings.width = winWidth;
        this.cropperSettings.height = 200;

        this.cropperSettings.croppedWidth = winWidth;
        this.cropperSettings.croppedHeight = 200;

        //this.cropperSettings.canvasWidth = 500;
        //this.cropperSettings.canvasHeight = 300;
        this.cropperSettings.canvasWidth = winWidth;
        this.cropperSettings.canvasHeight = 400;

        this.cropperSettings.minWidth = 300;
        this.cropperSettings.minHeight = 150;

        this.cropperSettings.rounded = false;

        this.cropperSettings.cropperDrawSettings.strokeColor = 'rgba(255,255,255,1)';
        this.cropperSettings.cropperDrawSettings.strokeWidth = 2;

        this.cropperSettings.keepAspect = true;
        this.cropperSettings.preserveSize = true;
        this.cropperSettings.cropOnResize = false;
        this.cropperSettings.noFileInput = true;
        this.cropperSettings.compressRatio = 0.1;

        this.imgData = {};
    }

    ngOnInit() {
        if (this.id && this.id == '1') {
            this.showAddInfo = false;
        }
        //调用城市服务信息
        this.citylocation = new qq.maps.CityService(this.options);
        //微信JS SDK配置
        this.wxService.get().then((res) => {
            if (!res) {
                console.warn('jweixin.js 加载失败');
                return;
            }
            let url = this.CurrentUrl;//encodeURIComponent(location.href.split('#')[0]);
            this.settingsService.getJsApiConfig(url).subscribe(result => {
                if (result) {
                    result.jsApiList = ['openLocation', 'getLocation'];//指定调用的接口名
                    // 1、通过config接口注入权限验证配置
                    wx.config(result.toJSON());
                    // 2、通过ready接口处理成功验证
                    //wx.ready(() => {
                    // 注册各种onMenuShareTimeline & onMenuShareAppMessage
                    //    if (this.showAddInfo == true) {//当新增的时候 才自动定位
                    //        this.wxGetLocation();
                    //    }
                    //});
                    this.wxReady().then((res) => {
                        if (this.showAddInfo == true) {//当新增的时候 才自动定位
                            this.wxGetLocation();
                        }
                    });
                    // 2、通过error接口处理失败验证
                    wx.error(() => {

                    });
                }
            });
        });

        this.settingsService.getUser().subscribe(result => {
            this.user = result;
        });
        this.shopService.GetShopByOpenId(this.WUserParams).subscribe(result => {
            if (result) {
                this.res = result.toJSON();
                this.coverPhoto = this.res.coverPhoto;
                this.latitude = this.res.latitude;
                this.longitude = this.res.longitude;
                this.qqLatitude = this.res.qqLatitude;
                this.qqLongitude = this.res.qqLongitude;
                this.showAddInfo = false;
                this.getlocation();
                this.title = '修改店铺';
            }
        });
    }

    wxReady(): Promise<boolean> {
        return (new Promise<any>((resolve, reject) => {
            wx.ready(() => {
                resolve(true);
            });
        }));
    }

    onGallery(item: any) {
        if (item) {
            this.img = [{ file: item._file, item: item }];
        } else {
            this.img = this.hostUrl + this.coverPhoto;
        }
        this.imgShow = true;
    }

    onDel(item: any) {
        //console.log(item);
        /*if (item) {
            this.uploader.removeFromQueue(item.item);
        }*/
        this.coverPhoto = null;
    }

    onSave() {
        //console.table(this.res);
        if (!this.coverPhoto || this.coverPhoto == '') {
            this.srv['warn']('请上传店铺形象');
            return;
        }
        if (!this.latitude || !this.longitude) {
            this.srv['warn']('请获取位置信息');
            return;
        }
        this.res.coverPhoto = this.coverPhoto;
        this.res.latitude = this.latitude;
        this.res.longitude = this.longitude;
        this.res.qqLatitude = this.qqLatitude;
        this.res.qqLongitude = this.qqLongitude;
        //this.res.address = this.myaddress + this.res.address;
        this.shopService.WechatCreateOrUpdateShop({
            shop: this.res,
            tenantId: this.settingsService.tenantId,
            openId: this.settingsService.openId,
            host: this.hostUrl
        }).subscribe(data => {
            if (data) {
                this.srv['success']('保存成功');
                this.router.navigate(["/shops/shop", { isShowWindows: false }]);
            } else {
                this.srv['warn']('保存失败');
            }
        });
    }

    getWXLocation(): Promise<any> {
        return (new Promise<any>((resolve, reject) => {
            this.wxService.getLocation().then((res) => {
                //this.latitude = res.latitude;
                //this.longitude = res.longitude;
                //this.wxService.translate(res.latitude, res.longitude).then((result) => {
                //    resolve(result);
                //})
                resolve(res);
            });
        }));
    }

    //调用微信获取当前位置
    wxGetLocation() {
        this.isReset = true;
        this.locationInfo = '定位中....';
        this.getWXLocation().then((res) => {
            this.latitude = res.latitude;
            this.longitude = res.longitude;
            this.qqLatitude = res.latitude; //res[0].lat;
            this.qqLongitude = res.longitude; //res[0].lng;
            this.getlocation();
        });
    }
    //打开微信地图
    wxOpenLocation() {
        if (!this.latitude || !this.longitude) {
            this.srv['info']('请先获取位置信息');
            return;
        }
        wx.openLocation({
            latitude: (this.qqLatitude ? this.qqLatitude : this.latitude), // 纬度，浮点数，范围为90 ~ -90
            longitude: (this.qqLongitude ? this.qqLongitude : this.longitude), // 经度，浮点数，范围为180 ~ -180。
            name: this.res.name, // 位置名
            address: this.res.address, // 地址详情说明
            scale: 16, // 地图缩放级别,整形值,范围从1~28。默认为最大
            infoUrl: this.hostUrl + '/gawechat/index.html#/shops/shop' // 在查看位置界面底部显示的超链接,可点击跳转
        });
    }
    //获取城市位置信息
    getlocation() {
        var latLng = new qq.maps.LatLng(this.qqLatitude, this.qqLongitude);
        this.citylocation.searchCityByLatLng(latLng);
    }

    //裁剪图片后上传 2018-06-07
    goUploadImg() {
        this.imgData.image = this.shopCropper.cropper.getCroppedImage(true).src;
        if (!this.imgData.image) {
            this.srv['info']('请重新选择图片');
            this.imgCropperPopup.close();
            return;
        }
        //console.log(this.fileName);
        this.shopService.FilesPostsBase64({ fileName: this.fileName, imageBase64: this.imgData.image }).subscribe((res) => {
            //console.table(res);
            if (res && res.code == 0) {
                this.coverPhoto = res.data;
                this.imgCropperPopup.close();
                this.srv['success']('上传图片成功');
            } else {
                console.log(res);
                this.srv['warn'](res.msg);
            }
        });
    }
    cancelUploadImg() {
        this.imgCropperPopup.close();
    }
    showCropperPopup() {
        this.imgCropperPopup.show();
    }
    //选择图片
    fileChange($event) {
        this.imgCropperPopup.show();
        this.srvToast.show(null, 10000, null, 'loading');
        //const image: any = new Image();
        //const file: File = $event.target.files[0];
        var image: any = new Image();
        var file: File = $event.target.files[0];
        var myReader: FileReader = new FileReader();
        var that = this;
        //alert(file.size);
        this.setCompressRatio(file.size);
        this.fileName = file.name;
        //const myReader: FileReader = new FileReader();
        myReader.onloadend = function (loadEvent: any) { //(loadEvent: any) => {
            image.src = loadEvent.target.result;
            //this.shopCropper.setImage(image);
            that.shopCropper.setImage(image);
            that.srvToast.hide();
        };
        myReader.readAsDataURL(file);
    }

    //根据图片大小设置压缩率
    setCompressRatio(size: number) {
        //如果小于500kb 不压缩
        let msize = 1024 * 500;
        //alert(size)
        if (size <= msize) {
            this.shopCropper.settings.compressRatio = 1;
        } else {
            let ratio = msize / size;
            if (ratio > 0.02) {
                this.shopCropper.settings.compressRatio = ratio;
            } else {
                this.shopCropper.settings.compressRatio = 0.02;
            }
            //alert(this.shopCropper.settings.compressRatio)
        }
    }
}
